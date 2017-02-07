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
    public class mmsns_Activity : Activity
    {
        ListView listview;
        ImageView close;
        List<mmsStruct> snslist = new List<mmsStruct>();

        AppContext AP;
        Client client;

        ImageView plus_sns;

        mmsnsAdapter snsAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mmssns_layout);

            listview = FindViewById<ListView>(Resource.Id.mmsns_list_view);
            close = FindViewById<ImageView>(Resource.Id.mmsnsout);
            plus_sns = FindViewById<ImageView>(Resource.Id.snsplus);

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

            
            // SNS로 갈때
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqSNSList);  // SNS 화면 요청 신호
            bw.Write(AP.get_user_id());
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

            snslist = AP.getmmslist();

            for (int i = 0; i < snslist.Count(); i++)
            {
                snslist[i].setLastcomment();
            }

            snslist.Reverse();

            snsAdapter = new mmsnsAdapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), snslist);

            listview.Adapter = snsAdapter;

            listview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                mmsStruct selected = (mmsStruct)(listview.GetItemAtPosition(e.Position));
                Intent another = new Intent();
                AP.setmmsnsContents(selected);
                int stunum = selected.StudentNum;
                string day = selected.CreateDate;
                string con = selected.Body;
                another.PutExtra("AUTHOR", stunum.ToString());
                another.PutExtra("DATE", day);
                another.PutExtra("BODY", con);
                another.SetClass(this, typeof(mmSNSMoreActivity));
                StartActivityForResult(another, 0);
            };

            plus_sns.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.plus_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.plus_up);
                        Intent addtask = new Intent();
                        addtask.SetClass(this, typeof(SnsWriteActivity));
                        StartActivityForResult(addtask, 0);
                        break;
                    default:
                        break;
                }
            };
        }

        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && data.GetStringExtra("Done") == "1")
            {
                snslist.Clear();
                AP.cleanmmslist();


                // SNS로 갈때
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqSNSList);  // SNS 화면 요청 신호
                bw.Write(AP.get_user_id());
                bw.Close();
                byte[] data1 = ms.ToArray();
                ms.Dispose();
                client.Send(data1, 0, data1.Length);
                data1 = null;

                while (!AP.getstate())
                {
                    Task.Delay(5).Wait();
                }

                AP.setstate(false);

                snslist = AP.getmmslist();

                for (int i = 0; i < snslist.Count(); i++)
                {
                    snslist[i].setLastcomment();
                }

                snslist.Reverse();

                snsAdapter.NotifyDataSetChanged();


            }
        }
    }
}