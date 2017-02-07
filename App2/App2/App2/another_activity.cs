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
    public class another_activity : Activity
    {
        TextView TaskTitle;
        TextView TaskDeadline;
        TextView TaskMaster;
        TextView Contents;
        ImageView close;
        Button commenbtn;
        ListView listview;
        ImageView taskedit;
        EditText commenttext;

        ViewGroup filecomponent;
        ViewGroup filelayout;
        TextView filedown;

        AppContext AP;
        Client client;

        List<TaskWorking> Taskwork = new List<TaskWorking>();
        TaskWorkAdapter adapter;
        InputMethodManager imm;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            AP = (AppContext)ApplicationContext;
            client = AP.getClient();


            if (AP.getcurrentTask().FileNum == 0)
            {
                SetContentView(Resource.Layout.another_layout_nonfile);
            }
            else
            {
                SetContentView(Resource.Layout.another_layout);
                filecomponent = FindViewById<ViewGroup>(Resource.Id.filecom);
                filelayout = FindViewById<ViewGroup>(Resource.Id.filelayout);
                filedown = FindViewById<TextView>(Resource.Id.filename);
                filedown.Text = AP.getcurrentTask().FileName;

                filelayout.Click += delegate
                {
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);
                    bw.Write((int)Commands.reqFileDownload);
                    bw.Write(filedown.Text.ToString());
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
                }; //여기다 파일 다운로더 적용하면 됨
            }
            listview = FindViewById<ListView>(Resource.Id.intasklist);
            TaskTitle = FindViewById<TextView>(Resource.Id.taskTitle);
            TaskDeadline = FindViewById<TextView>(Resource.Id.taskdeadline);
            TaskMaster = FindViewById<TextView>(Resource.Id.taskmaster);
            close = FindViewById<ImageView>(Resource.Id.taskout);
            taskedit = FindViewById<ImageView>(Resource.Id.taskedit);
            Contents = FindViewById<TextView>(Resource.Id.contents);
            commenbtn = FindViewById<Button>(Resource.Id.commentbtn);
            commenttext = FindViewById<EditText>(Resource.Id.commenttext);
            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);

            TaskTitle.Text = Intent.GetStringExtra("TASK_TITLE");
            TaskDeadline.Text = "기한 : " + Intent.GetStringExtra("TASK_DEADLINE");
            TaskMaster.Text = "작성자 : " + Intent.GetStringExtra("TASK_AUTHOR");
            Contents.Text = Intent.GetStringExtra("TASK_CONTENTS");

            Taskwork = AP.getcurrentTask().getreplylist();

            adapter = new TaskWorkAdapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), Taskwork);

            listview.Adapter = adapter;
            // Create your application here

            if (AP.get_user_num() != AP.getcurrentTask().StudentNum) taskedit.Visibility = ViewStates.Invisible;

            taskedit.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
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
                        toEdit.SetClass(this, typeof(TaskEditActivity));

                        string title = AP.getcurrentTask().Title;
                        string start = AP.getcurrentTask().CreateDate;
                        string dead = AP.getcurrentTask().DeadLine;
                        string desc = AP.getcurrentTask().Contents;

                        toEdit.PutExtra("Title", title);
                        toEdit.PutExtra("Start", start);
                        toEdit.PutExtra("Dead", dead);
                        toEdit.PutExtra("Desc", desc);
                        StartActivityForResult(toEdit, 0);
                        break;
                    default:
                        break;
                }
            };

            commenbtn.Click += delegate 
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqAddTaskComment);
                bw.Write(AP.getcurrentTask().TaskNum);  // 일정 번호
                bw.Write(AP.get_user_num());  // 학번 기입
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


                AP.cleantasklist();
                Taskwork.Clear();

                ms = new MemoryStream();
                bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqTaskList);  // 일정 화면 요청 신호
                bw.Write((int)AP.get_isMyteam());  // 내 팀인지 아닌지 보냄
                bw.Write(AP.get_currentTeamNum());  // 팀 번호 보냄
                bw.Write(AP.get_currentTeamTitle());  // 팀 주제 보냄
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

                List<TaskData_Title> datas = new List<TaskData_Title>();

                datas = AP.gettasklist();

                for (int i = 0; i < datas.Count(); i++)
                {
                    if (AP.getcurrentTask().TaskNum == datas[i].TaskNum)
                    {
                        Contents.Text = datas[i].Contents;
                        AP.setcurrentTask(datas[i]);
                        break;
                    }
                }

                commenttext.Text = "";
                imm.HideSoftInputFromWindow(commenttext.WindowToken, 0);

                Taskwork = AP.getcurrentTask().getreplylist();

                adapter = new TaskWorkAdapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), Taskwork);

                listview.Adapter = adapter;

                TaskTitle.RequestFocus();

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
                        Finish();
                        break;
                    default:
                        break;
                }
            };
        }

        


        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok && data.GetStringExtra("Done") == "1")
            {
                if (data.GetStringExtra("Delete") == "1")
                {
                    Intent intent = new Intent();
                    intent.PutExtra("TaskDelete", "1");
                    SetResult(Result.Ok, intent);
                    Finish();
                }
                else if ((data.GetStringExtra("Edit") == "1"))
                {
                    TaskData_Title temp = new TaskData_Title(1, 1, 1,1,"", "", "", "", "", 1);
                    temp = AP.getcurrentTask();

                    AP.cleantasklist();

                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);
                    bw.Write((int)Commands.reqTaskList);  // 일정 화면 요청 신호
                    bw.Write((int)AP.get_isMyteam());  // 내 팀인지 아닌지 보냄
                    bw.Write(AP.get_currentTeamNum());  // 팀 번호 보냄
                    bw.Write(AP.get_currentTeamTitle());  // 팀 주제 보냄
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

                    List<TaskData_Title> datas = new List<TaskData_Title>();

                    datas = AP.gettasklist();

                    for (int i = 0; i < datas.Count(); i++)
                    {
                        if (AP.getcurrentTask().TaskNum == datas[i].TaskNum)
                        {
                            TaskTitle.Text = datas[i].Title;
                            TaskDeadline.Text = "기한 : " + datas[i].DeadLine;
                            TaskMaster.Text = "작성자 : " + datas[i].StudentNum.ToString();
                            Contents.Text = datas[i].Contents;
                            AP.setcurrentTask(datas[i]);
                            break;
                        }
                    }
                    Intent intent = new Intent();
                    intent.PutExtra("Done", "1");
                    SetResult(Result.Ok, intent);
                }
            }
        }
    }

}