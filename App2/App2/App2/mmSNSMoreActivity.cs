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
using Android.Views.InputMethods;

using System.Threading.Tasks;
using System.IO;


namespace App2
{
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class mmSNSMoreActivity : Activity
    {

        ListView listview;
        ImageView close;
        List<mmsns_comment> commentlist = new List<mmsns_comment>();

        TextView stunum;
        TextView date;
        TextView body;
        Button commenbtn;
        EditText commenttext;
        ImageView editsns;

        AppContext AP;
        Client client;
        InputMethodManager imm;

        mmsns_comment_adapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mmsns_more_layout);

            listview = FindViewById<ListView>(Resource.Id.mmsns_comment_view);
            close = FindViewById<ImageView>(Resource.Id.mmsnsout);
            stunum = FindViewById<TextView>(Resource.Id.sns_writer);
            date = FindViewById<TextView>(Resource.Id.sns_write_date);
            body = FindViewById<TextView>(Resource.Id.sns_body);
            commenbtn = FindViewById<Button>(Resource.Id.commentbtn);
            commenttext = FindViewById<EditText>(Resource.Id.commenttext);
            editsns = FindViewById<ImageView>(Resource.Id.snsedit);

            AP = (AppContext)ApplicationContext;
            client = AP.getClient();
            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            if (AP.get_user_num() != AP.getmmsnsContents().StudentNum)
            {
                editsns.Visibility = ViewStates.Invisible;
            }

            commentlist = AP.getmmsnsContents().getcommentlist();

            adapter = new mmsns_comment_adapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), commentlist);

            listview.Adapter = adapter;

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

            stunum.Text = Intent.GetStringExtra("AUTHOR");
            date.Text = "작성일자 : " + Intent.GetStringExtra("DATE");
            body.Text = "작성자 : " + Intent.GetStringExtra("BODY");

            editsns.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.edit_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.edit_up);
                        Intent toEdit = new Intent();
                        toEdit.SetClass(this, typeof(SnsEditActivity));

                        string body = AP.getmmsnsContents().Body;

                        toEdit.PutExtra("BODY", body);

                        StartActivityForResult(toEdit, 0);
                        break;
                    default:
                        break;
                }
            };


            commenbtn.Click += delegate
            {
                //mmsns 코멘트 쓰기
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqAddSNSComment);  // SNS 덧글 추가 요청 신호
                bw.Write(Convert.ToInt32(AP.getmmsnsContents().SnsNum));
                bw.Write(AP.get_user_num());
                bw.Write(commenttext.Text);
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


                AP.cleanmmslist();
                commentlist.Clear();

                ms = new MemoryStream();
                bw = new BinaryWriter(ms);
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

                List<mmsStruct> datas = new List<mmsStruct>();

                datas = AP.getmmslist();

                for (int i = 0; i < datas.Count(); i++)
                {
                    if (AP.getmmsnsContents().SnsNum == datas[i].SnsNum)
                    {
                        AP.setmmsnsContents(datas[i]);
                        break;
                    }
                }
                commenttext.Text = "";
                imm.HideSoftInputFromWindow(commenttext.WindowToken, 0);
                commentlist = AP.getmmsnsContents().getcommentlist();

                adapter = new mmsns_comment_adapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), commentlist);

                listview.Adapter = adapter;

                Intent intent = new Intent();
                intent.PutExtra("Done", "1");
                SetResult(Result.Ok, intent);

            };

        }
        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok && data.GetStringExtra("Done") == "1")
            {
                commentlist.Clear();
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

                List<mmsStruct> datas = new List<mmsStruct>();
                datas = AP.getmmslist();

                for (int i = 0; i < datas.Count(); i++)
                {
                    if (AP.getmmsnsContents().SnsNum == datas[i].SnsNum)
                    {
                        AP.setmmsnsContents(datas[i]);
                        break;
                    }
                }

                body.Text = AP.getmmsnsContents().Body;

                commentlist = AP.getmmsnsContents().getcommentlist();

                adapter = new mmsns_comment_adapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), commentlist);

                listview.Adapter = adapter;

                Intent intent = new Intent();
                intent.PutExtra("Done", "1");
                SetResult(Result.Ok, intent);

            }
        }
    }
}