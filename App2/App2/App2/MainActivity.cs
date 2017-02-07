using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace App2
{
    //[Activity(MainLauncher = true, Theme = "@style/Theme.NoTitle",
    //    ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    [Activity(Theme = "@style/Theme.NoTitle",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : Activity
    {
        int count = 1;
        //Button btn;
        LinearLayout dynamicLinear;
        AppContext AP;
        Client client;
        private const int TASK_LIST_ID = 0x8000;
        private const int TEXT_VIEW_ID = 0x7000;
        private const int BUTTON_ID = 10;
        private const int HEADER_TEXT_SIZE = 23;
        List<TaskData_Title> datas = new List<TaskData_Title>();
        ImageView toAccount;
        ImageView toProject;
        ListView listview;
        TextView Userid;
        TextView teamTitle;
        ImageView toSns;
        ImageView taskplus;
        TaskDataAdapter adapter = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            AP = (AppContext)ApplicationContext;
            // Get our button from the layout resource,
            // and attach an event to it
            // btn = FindViewById<Button>(Resource.Id.mainbutton);



            listview = FindViewById<ListView>(Resource.Id.listview);
            toAccount = FindViewById<ImageView>(Resource.Id.toaccount);
            toSns = FindViewById<ImageView>(Resource.Id.tosns);
            Userid = FindViewById<TextView>(Resource.Id.user);
            taskplus = FindViewById<ImageView>(Resource.Id.taskplus);
            toProject = FindViewById<ImageView>(Resource.Id.toproject);
            teamTitle = FindViewById<TextView>(Resource.Id.projectname);

            client = AP.getClient();

            if (AP.get_isMyteam() == 0)
            {
                toAccount.Visibility = ViewStates.Invisible;
                taskplus.Visibility = ViewStates.Invisible;
            }

            teamTitle.Text = AP.get_currentTeamTitle();

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqTaskList);  // 일정 화면 요청 신호
            bw.Write((int)AP.get_isMyteam());  // 내 팀인지 아닌지 보냄
            bw.Write(AP.get_currentTeamNum());  // 팀 번호 보냄
            bw.Write(AP.get_currentTeamTitle());  // 팀 주제 보냄
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

            datas = AP.gettasklist();


            adapter = new TaskDataAdapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), datas);

            listview.Adapter = adapter;

            Userid.Text = AP.get_user_num().ToString();
            

            toSns.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.sns_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.sns_up);
                        Intent account = new Intent();
                        account.SetClass(this, typeof(mmsns_Activity));
                        StartActivity(account);
                        break;
                    default:
                        break;
                }
            };


            toAccount.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.account_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.account_up);
                        Intent account = new Intent();
                        account.SetClass(this, typeof(AccountActivity));
                        StartActivity(account);
                        break;
                    default:
                        break;
                }
            };

            toProject.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.project_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.project_up);
                        Finish();
                        break;
                    default:
                        break;
                }
            };

            taskplus.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
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
                        addtask.SetClass(this, typeof(Add_Taks_Activity));
                        StartActivityForResult(addtask, 0);
                        break;
                    default:
                        break;
                }
            };

            LinearLayout HeaderLayout = new LinearLayout(this);
            LinearLayout.LayoutParams HeaderParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            HeaderParams.SetMargins(30, 30, 30, 30);

            TextView TaskListHeader = new TextView(this);
            TaskListHeader.Text = "진행중";
            TaskListHeader.SetTextColor(Android.Graphics.Color.Blue);
            TaskListHeader.TextSize = HEADER_TEXT_SIZE;
            TaskListHeader.LayoutParameters = HeaderParams;
            HeaderLayout.AddView(TaskListHeader);

            LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);

            listview.AddHeaderView((LinearLayout)inflater.Inflate(Resource.Layout.HeaderLayout,HeaderLayout), null, false);

            if (AP.get_isMyteam() == 1)
            {
                listview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
                {
                    TaskData_Title selected = (TaskData_Title)(listview.GetItemAtPosition(e.Position));
                    Intent another = new Intent();
                    AP.setcurrentTask(selected);
                    string author = selected.StudentNum.ToString();
                    string str = selected.Title;
                    string deadline = selected.DeadLine;
                    string con = selected.Contents;
                    another.PutExtra("TASK_AUTHOR", author);
                    another.PutExtra("TASK_TITLE", str);
                    another.PutExtra("TASK_DEADLINE", deadline);
                    another.PutExtra("TASK_CONTENTS", con);
                    another.SetClass(this, typeof(another_activity));
                    StartActivityForResult(another, 0);
                };
            }

            //btn.Click += delegate { btnclick(); };
            //dynamicLinear = FindViewById<LinearLayout>(Resource.Id.inflatedLayout);  성공했던거

            //LinearLayout inflatedLayout = FindViewById<LinearLayout>(Resource.Id.inflatedLayout);
            //LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
            //inflater.Inflate(Resource.Layout.Inflated_layout, inflatedLayout);
            //btn.Click += delegate { onClick(); };
        }

        //public void onClick()
        //{
        //    //LinearLayout newLinear = new LinearLayout(this);
        //    //newLinear.Id = DYNAMIC_VIEW_ID + count;
        //    //LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
        //    //newLinear = (LinearLayout)inflater.Inflate(Resource.Layout.Inflated_layout, newLinear);
        //    //TextView task = newLinear.FindViewById<TextView>(Resource.Id.textview1);
        //    //task.Text = "태스크" + Convert.ToString(count);
        //    //dynamicLinear.AddView(newLinear);

        //    LinearLayout newLayout = new LinearLayout(this);
        //    newLayout.Orientation = Orientation.Vertical;

        //    newLayout.Id = TASK_LIST_ID + count;
  
        //    LinearLayout.LayoutParams param = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
        //    param.SetMargins(50, 32, 0, 0);

        //    TextView textv = new TextView(this);
        //    textv.Text = "태스크" + Convert.ToString(count);
        //    textv.Id = TEXT_VIEW_ID+count;
        //    count++;

        //    textv.LayoutParameters = param;
        //    textv.TextSize = 22;
        //    newLayout.AddView(textv);
            
        //    newLayout.Touch += viewtouch;
            
        //    LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
        //    dynamicLinear.AddView((LinearLayout)inflater.Inflate(Resource.Layout.Inflated_layout, newLayout));

        //    //텍스트뷰 하나로만(성공)
        //    //TextView dynamicText = new TextView(this);
        //    //dynamicText.Id = DYNAMIC_VIEW_ID + count;
        //    //dynamicText.Touch += viewtouch;
        //    //dynamicText.Text = "태스크" + Convert.ToString(count);
        //    //count++;
        //    //dynamicLinear.AddView(dynamicText);
        //}
        public void viewtouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            int Id = 0;
            LinearLayout tv = (LinearLayout)sender;
            switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    tv.SetBackgroundColor(Android.Graphics.Color.Gray);
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    tv.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
                    break;

                default:
                    break;
            }
        }

        public void btnclick()
        {
            //TextView tx = FindViewById<TextView>(Resource.Id.textview1);
            //tx.Text = "!";
            datas.Clear();
            //TaskData_Title sample = new TaskData_Title("제거", "2", "2");
            //datas.Add(sample);

            TaskDataAdapter adapter = new TaskDataAdapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), datas);

            listview.Adapter = adapter;

            listview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                TaskData_Title selected = (TaskData_Title)(listview.GetItemAtPosition(e.Position));
                Intent another = new Intent();
                another.PutExtra("TASK_TITLE", selected.Title);
                another.SetClass(this, typeof(another_activity));
                StartActivity(another);
            };
        }

        void ProjectEdit(object sender, View.TouchEventArgs touchEventArgs)
        {
            Intent edit = new Intent();
            edit.SetClass(this, typeof(ProjectEditActivity));
            StartActivity(edit);
        }

        void ToAccount()
        {
            Intent account = new Intent();
            account.SetClass(this, typeof(AccountActivity));
            StartActivity(account);
        }

        protected override void OnResume()
        {
            base.OnResume();
            //Toast.MakeText(this, "!", ToastLength.Short).Show();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            //Toast.MakeText(this, "!", ToastLength.Short).Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && data.GetStringExtra("Done") == "1" || resultCode == Result.Ok && data.GetStringExtra("TaskDelete") == "1")
            {
                datas.Clear();
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

                datas = AP.gettasklist();

                adapter.NotifyDataSetChanged();


            }
        }
    }

}

