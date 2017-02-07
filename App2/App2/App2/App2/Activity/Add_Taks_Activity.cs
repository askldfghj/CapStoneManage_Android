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
using Android.Util;
using Android.Net;
using Android.Database;
using Android.Provider;

using System.Threading.Tasks;
using System.IO;

namespace App2
{
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class Add_Taks_Activity : Activity
    {
        ImageView close;

        TextView deadline;
        TextView startline;
        TextView file;
        EditText title;
        EditText desc;

        TextView done;

        string str_startline = "";
        string str_deadline = "";
        

        AppContext AP;
        Client client;

        string filepath = "";
        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Add_Task_Layout);
            // Create your application here


            close = FindViewById<ImageView>(Resource.Id.taskaddout);
            deadline = FindViewById<TextView>(Resource.Id.taskadddeadline);
            startline = FindViewById<TextView>(Resource.Id.taskaddstartline);
            title = FindViewById<EditText>(Resource.Id.addtitle);
            desc = FindViewById<EditText>(Resource.Id.taskdescription);
            done = FindViewById<TextView>(Resource.Id.addtaskresult);
            file = FindViewById<TextView>(Resource.Id.taskfile);

            AP = (AppContext)ApplicationContext;
            client = AP.getClient();

            deadline.Click += delegate (object sender, EventArgs eventArgs)
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    deadline.Text = time.Year + "년 " + time.Month + "월 " + time.Day + "일 까지";
                    str_deadline = time.Year + "/";
                    if (time.Month < 10)
                    {
                        str_deadline += "0" + time.Month + "/";
                    }
                    else
                    {
                        str_deadline += time.Month + "/";
                    }
                    if (time.Day < 10)
                    {
                        str_deadline += "0" + time.Day;
                    }
                    else
                    {
                        str_deadline += time.Day;
                    }
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            startline.Click += delegate (object sender, EventArgs eventArgs)
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    startline.Text = time.Year + "년 " + time.Month + "월 " + time.Day + "일 부터";
                    str_startline = time.Year + "/";
                    if (time.Month < 10)
                    {
                        str_startline += "0" + time.Month + "/";
                    }
                    else
                    {
                        str_startline += time.Month + "/";
                    }
                    if (time.Day < 10)
                    {
                        str_startline += "0" + time.Day;
                    }
                    else
                    {
                        str_startline += time.Day;
                    }
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            file.Click += delegate 
            {
                Intent = new Intent();
                Intent.SetType("*/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "파일 선택"), PickImageId);
            };

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

            done.Click += delegate 
            {
                if (title.Text == "")
                {
                    title.RequestFocus();
                }
                else if (startline.Text == "시작 일정")
                {

                }
                else if (deadline.Text == "마감 일정")
                {

                }
                else Done();

            };

        }

        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    Intent intent = new Intent();
        //    intent.PutExtra("Done", "0");
        //    SetResult(Result.Ok, intent);
        //    Finish();
        //}

        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra("Done", "0");
            SetResult(Result.Ok, intent);
            Finish();
        }

        void Done()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqTaskModify);
            bw.Write(1);  // 추가 요청이라는 뜻
            bw.Write(Convert.ToInt32(AP.get_currentTeamNum()));  // 팀번호 프로젝트번호
            bw.Write(AP.get_currentTeamTitle());  // 팀 주제 로그용
            bw.Write(AP.get_user_num());  // 학번


            if (String.Compare(filepath, "") != 0)  // 첨부파일이 존재한다면
            {
                byte[] b = File.ReadAllBytes(filepath);
                bw.Write((int)b.Length);
                bw.Write(Convert.ToString(Path.GetFileName(filepath)));
                bw.Write(b);
            }
            else
            {
                bw.Write(0);  // 첨부파일이 존재하지 않음을 알림
            }



            bw.Write(title.Text);  // 주제
            bw.Write(desc.Text);  // 내용
            bw.Write(str_startline);  // 시작일
            bw.Write(str_deadline);  // 종료일
            bw.Write(0);  // 추가할땐 무조건 아직 안끝났기 때문에 0 삽입
            bw.Close();
            byte[] data = ms.ToArray();
            ms.Dispose();
            client.Send(data, 0, data.Length);
            data = null;

            while (!AP.getstate())
            {
                Log.Info("wait", "답신을 기다리는중");
                Task.Delay(5).Wait();
            }

            AP.setstate(false);

            Intent intent = new Intent();
            intent.PutExtra("Done", "1");

            SetResult(Result.Ok, intent);
            Finish();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;

                if (string.Compare(data.Data.Scheme, "content") == 0)
                {
                    filepath = GetRealPathFromURI(data.Data);
                }
                else
                {
                    filepath = data.Data.Path;
                }
                file.Text = Path.GetFileName(filepath);
            }
        }

        private string GetRealPathFromURI(Android.Net.Uri contentURI)
        {
            var cursor = ContentResolver.Query(contentURI, null, null, null, null);
            cursor.MoveToFirst();
            string documentId = cursor.GetString(0);
            documentId = documentId.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;

        }
    }
}