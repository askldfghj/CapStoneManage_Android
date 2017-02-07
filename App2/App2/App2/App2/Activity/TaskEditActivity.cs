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
using Android.Provider;

using System.Threading.Tasks;
using System.IO;

namespace App2
{
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class TaskEditActivity : Activity
    {
        ImageView close;

        TextView deadline;
        TextView startline;
        TextView delete;

        EditText title;
        EditText desc;

        TextView editdone;
        TextView file;

        AppContext AP;
        Client client;

        string filepath = "";

        string str_startline = "";
        string str_deadline = "";

        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaskEditLayout);

            close = FindViewById<ImageView>(Resource.Id.taskeditout);
            deadline = FindViewById<TextView>(Resource.Id.taskeditdeadline);
            startline = FindViewById<TextView>(Resource.Id.taskeditstartline);
            delete = FindViewById<TextView>(Resource.Id.taskdelete);
            title = FindViewById<EditText>(Resource.Id.tasktitleresult);
            desc = FindViewById<EditText>(Resource.Id.taskeditdescription);
            editdone = FindViewById<TextView>(Resource.Id.taskdone);
            file = FindViewById<TextView>(Resource.Id.taskfile);

            title.Text = Intent.GetStringExtra("Title");
            startline.Text = Intent.GetStringExtra("Start");
            str_startline = Intent.GetStringExtra("Start");
            deadline.Text = Intent.GetStringExtra("Dead");
            str_deadline = Intent.GetStringExtra("Dead");
            desc.Text = Intent.GetStringExtra("Desc");


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
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "파일 선택"), PickImageId);
            };

            editdone.Click += delegate
            {
                // 이제 서버로 수정되었음을 정보 전달
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqTaskModify);
                bw.Write(2);  // 수정 요청이라는 뜻
                bw.Write(Convert.ToInt32(AP.get_currentTeamNum()));
                bw.Write(AP.get_currentTeamTitle());
                bw.Write(Convert.ToInt32(AP.getcurrentTask().TaskNum));  // taskNum 써줘야함


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


                bw.Write(title.Text);
                bw.Write(desc.Text);
                bw.Write(str_startline);
                bw.Write(str_deadline);
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
                intent.PutExtra("Edit", "1");
                SetResult(Result.Ok, intent);
                Finish();
            };


            AP.setstate(false);

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
            // Create your application here

            delete.Click += delegate { DeleteDone(); };
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra("Done", "0");
            SetResult(Result.Ok, intent);
            Finish();
        }

        void DeleteDone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqTaskModify);
            bw.Write(3);  // 수정 요청이라는 뜻
            bw.Write(AP.get_currentTeamTitle());
            bw.Write(Convert.ToInt32(AP.getcurrentTask().TaskNum));  // taskNum 써줘야함
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
            intent.PutExtra("Delete", "1");
            SetResult(Result.Ok, intent);
            Finish();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;


                filepath = GetRealPathFromURI(data.Data);
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