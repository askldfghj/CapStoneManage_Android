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
    public class Add_Account_Activity : Activity
    {
        ImageView close;

        EditText title;
        EditText price;
        EditText desc;
        CheckBox check;
        TextView done;

        AppContext AP;
        Client client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Add_Account_Layout);
            title = FindViewById<EditText>(Resource.Id.accounttitle);
            price = FindViewById<EditText>(Resource.Id.accountprice);
            desc = FindViewById<EditText>(Resource.Id.accountdescription);
            check = FindViewById<CheckBox>(Resource.Id.check);
            done = FindViewById<TextView>(Resource.Id.addaccountresult);
            close = FindViewById<ImageView>(Resource.Id.addaccountout);

            AP = (AppContext)ApplicationContext;
            client = AP.getClient();

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
                        Finish();
                        break;
                    default:
                        break;
                }
            };

            done.Click += delegate
            {
                if (title.Text == "")
                {
                    title.RequestFocus();
                }
                else if (price.Text == "")
                {
                    price.RequestFocus();
                }
                else Done();

            };
        }

        void Done()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqAccountingModify);
            bw.Write(1);  // 추가 요청이라는 뜻
            bw.Write(Convert.ToInt32(AP.get_currentTeamNum()));  // 팀번호 프로젝트번호
            bw.Write(AP.get_currentTeamTitle());  // 팀 주제 로그용

            bw.Write(title.Text);  // 주제
            bw.Write(desc.Text);  // 내용
            bw.Write(Convert.ToInt32(price.Text));  // 가격
            if (check.Checked)
                bw.Write(0);
            else
                bw.Write(1);
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