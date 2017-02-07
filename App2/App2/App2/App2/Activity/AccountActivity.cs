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

using System.Threading.Tasks;
using System.IO;

namespace App2
{
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class AccountActivity : Activity
    {
        ListView listview;
        ImageView close;
        TextView leaveaccount;
        List<Account_Contents> acclist = new List<Account_Contents>();
        ImageView accountplus;

        AppContext AP;
        Client client;

        account_adapter acc_adapter;

        int total;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account_layout);
            // Create your application here

            listview = FindViewById<ListView>(Resource.Id.account_list_view);
            close = FindViewById<ImageView>(Resource.Id.accountout);
            leaveaccount = FindViewById<TextView>(Resource.Id.leave_account);
            accountplus = FindViewById<ImageView>(Resource.Id.accountplus);

            AP = (AppContext)ApplicationContext;
            client = AP.getClient();
            total = 0;

            if (AP.get_isMyteam() == 0)
            {
                accountplus.Visibility = ViewStates.Invisible;
            }

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
            

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqAccountingList);  // 회계 화면 요청 신호
            bw.Write(AP.get_isMyteam());  // 내 팀인지 아닌지 보냄
            bw.Write(AP.get_currentTeamTitle());  // 팀 주제 보냄
            bw.Write(AP.get_currentTeamNum());
            bw.Close();
            byte[] data = ms.ToArray();
            ms.Dispose();
            client.Send(data, 0, data.Length);
            data = null;

            while (!AP.getstate())
            {
                Log.Info("wait_account", "답신을 기다리는중");
                Task.Delay(5).Wait();
            }

            AP.setstate(false);

            acclist = AP.getaccountlist();

            for (int i = 0; i < acclist.Count(); i++)
            {
                if (acclist[i].IsPlusMinus == 0)
                {
                    total += acclist[i].AccountCost;
                }
                else
                {
                    total -= acclist[i].AccountCost;
                }
            }

            leaveaccount.Text = total.ToString();


            acc_adapter = new account_adapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), acclist);

            listview.Adapter = acc_adapter;


            accountplus.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
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
                        addtask.SetClass(this, typeof(Add_Account_Activity));
                        StartActivityForResult(addtask, 0);
                        break;
                    default:
                        break;
                }
            };


            if (AP.get_isMyteam() == 1)
            {
                listview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
                {
                    Account_Contents selected = (Account_Contents)(listview.GetItemAtPosition(e.Position));
                    Intent editacc = new Intent();
                    AP.setaccountContents(selected);
                    string title = selected.AccountName;
                    int price = selected.AccountCost;
                    string desc = selected.AccountDesc;
                    editacc.PutExtra("ACC_NAME", title);
                    editacc.PutExtra("ACC_PRICE", price.ToString());
                    editacc.PutExtra("ACC_DESC", desc);
                    if (selected.IsPlusMinus == 0)
                        editacc.PutExtra("ACC_CHECK", "1");
                    else
                        editacc.PutExtra("ACC_CHECK", "0");
                    editacc.SetClass(this, typeof(Edit_Account_Activity));
                    StartActivityForResult(editacc, 0);
                };
            }
        }

        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && data.GetStringExtra("Done") == "1")
            {
                acclist.Clear();
                AP.cleanaccountlist();

                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqAccountingList);  // 회계 화면 요청 신호
                bw.Write(AP.get_isMyteam());  // 내 팀인지 아닌지 보냄
                bw.Write(AP.get_currentTeamTitle());  // 팀 주제 보냄
                bw.Write(AP.get_currentTeamNum());
                bw.Close();
                byte[] data1 = ms.ToArray();
                ms.Dispose();
                client.Send(data1, 0, data1.Length);
                data1 = null;

                while (!AP.getstate())
                {
                    Log.Info("wait_account", "답신을 기다리는중");
                    Task.Delay(5).Wait();
                }

                AP.setstate(false);

                acclist = AP.getaccountlist();

                total = 0;
                for (int i = 0; i < acclist.Count(); i++)
                {
                    if (acclist[i].IsPlusMinus == 0)
                    {
                        total += acclist[i].AccountCost;
                    }
                    else
                    {
                        total -= acclist[i].AccountCost;
                    }
                }

                leaveaccount.Text = total.ToString();

                acc_adapter.NotifyDataSetChanged();


            }
        }
    }
}