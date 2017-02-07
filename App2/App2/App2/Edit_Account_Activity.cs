using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


using System.Threading.Tasks;
using System.IO;

namespace App2
{
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class Edit_Account_Activity : Activity
    {
        ImageView close;

        EditText title;
        EditText price;
        EditText desc;
        CheckBox check;
        TextView done;
        TextView delete;

        AppContext AP;
        Client client;

        string ischeck;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Edit_Account_Layout);

            title = FindViewById<EditText>(Resource.Id.editaccounttitle);
            price = FindViewById<EditText>(Resource.Id.editaccountprice);
            desc = FindViewById<EditText>(Resource.Id.editaccountdescription);
            check = FindViewById<CheckBox>(Resource.Id.check);
            done = FindViewById<TextView>(Resource.Id.accountdone);
            delete = FindViewById<TextView>(Resource.Id.deleteaccount);
            close = FindViewById<ImageView>(Resource.Id.editaccountout);

            AP = (AppContext)ApplicationContext;
            client = AP.getClient();

            title.Text = Intent.GetStringExtra("ACC_NAME");
            price.Text = Intent.GetStringExtra("ACC_PRICE");
            desc.Text = Intent.GetStringExtra("ACC_DESC");
            ischeck = Intent.GetStringExtra("ACC_CHECK");

            if (ischeck == "1")
                check.Checked = true;

            done.Click += delegate
            {
                // 이제 서버로 수정되었음을 정보 전달
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqAccountingModify);
                bw.Write(2);  // 수정 요청이라는 뜻
                bw.Write(Convert.ToInt32(AP.getaccountContents().AccountNum));  // accountingNum 써줘야함
                bw.Write(AP.get_currentTeamNum());
                bw.Write(AP.get_currentTeamTitle());

                bw.Write(title.Text);
                bw.Write(desc.Text);
                bw.Write(Convert.ToInt32(price.Text));  // 가격
                if (check.Checked)
                    bw.Write(0);  // 출금
                else
                    bw.Write(1);  // 입금
                bw.Close();
                byte[] data = ms.ToArray();
                ms.Dispose();
                client.Send(data, 0, data.Length);
                data = null;

                while (!AP.getstate())
                {
                    Task.Delay(5).Wait();
                }

                AP.setstate(false);

                Intent intent = new Intent();
                intent.PutExtra("Done", "1");
                SetResult(Result.Ok, intent);
                Finish();
            };

            delete.Click += delegate { DeleteDone(); };

            close.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.close_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.close_up);
                        Intent intent = new Intent();
                        intent.PutExtra("Done", "0");
                        SetResult(Result.Ok, intent);
                        Finish();
                        break;
                    default:
                        break;
                }
            };
        }

        void DeleteDone()
        {
            // 이제 서버로 삭제되었음을 정보 전달
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqAccountingModify);
            bw.Write(3);  // 수정 요청이라는 뜻
            bw.Write(AP.get_currentTeamTitle());
            bw.Write(Convert.ToInt32(AP.getaccountContents().AccountNum));  // accountingNum 써줘야함
            bw.Close();
            byte[] data = ms.ToArray();
            ms.Dispose();
           client.Send(data, 0, data.Length);
            data = null;

            while (!AP.getstate())
            {
                Task.Delay(5).Wait();
            }

            AP.setstate(false);

            Intent intent = new Intent();
            intent.PutExtra("Done", "1");
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}