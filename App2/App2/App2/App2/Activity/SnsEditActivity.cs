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
    public class SnsEditActivity : Activity
    {
        ImageView close;
        EditText snsbody;
        TextView done;
       
        AppContext AP;
        Client client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sns_edit_layout);


            snsbody = FindViewById<EditText>(Resource.Id.snsbody);
            done = FindViewById<TextView>(Resource.Id.editsnsresult);
            close = FindViewById<ImageView>(Resource.Id.editsnsout);

            AP = (AppContext)ApplicationContext;
            client = AP.getClient();


            snsbody.Text = Intent.GetStringExtra("BODY");

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
                if (snsbody.Text == "")
                {
                    snsbody.RequestFocus();
                }
                else Done();

            };
        }
        void Done()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqSNSModify);
            bw.Write(2);  // 수정 요청이라는 뜻
            bw.Write(AP.getmmsnsContents().SnsNum);
            bw.Write(AP.getmmsnsContents().StudentNum);
            bw.Write(snsbody.Text);
            bw.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
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