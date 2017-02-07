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
    public class team_select_activity : Activity
    {
        AppContext AP;
        Client client;
        ListView listview;
        ProjectAdapter adapter = null;
        List<project_struct> datas = new List<project_struct>();
        TextView changelist;

        int current_team;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.team_select_layout);

            listview = FindViewById<ListView>(Resource.Id.teamlist);
            changelist = FindViewById<TextView>(Resource.Id.teamchange);
            AP = (AppContext)ApplicationContext;
            client = AP.getClient();

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((int)Commands.reqTeamList);  // 팀목록 요청 신호
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

            for (int i = 0; i < AP.getteamlistnum(); i++)
            {
                for (int j = 0; j < AP.getMyteamNumberCount(); j++)
                {
                    if (AP.getTeam(i).getProjectNum() == AP.getMYteamNumber(j))
                    {
                        datas.Add(AP.getTeam(i));
                        // 여기까지 '나의 팀' 채워넣기
                    }
                }
            }

            AP.setstate(false);
            AP.set_isMyteam(1);

            current_team = 1;
            adapter = new ProjectAdapter((LayoutInflater)GetSystemService(Context.LayoutInflaterService), datas);
            listview.Adapter = adapter;
            // Create your application here

            listview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                project_struct selected = (project_struct)(listview.GetItemAtPosition(e.Position));

                AP.set_currentTeamNum(selected.getProjectNum());
                AP.set_currentTeamTitle(selected.getProjectName());
                AP.set_currentteammember(selected.getMeberList());

                Intent another = new Intent();
                another.SetClass(this, typeof(MainActivity));
                StartActivityForResult(another, 0);
            };


            changelist.Click += (s, arg) =>
            {
                PopupMenu menu = new PopupMenu(this, changelist);
                menu.Inflate(Resource.Menu.popup_menu);

                menu.MenuItemClick += (s1, arg1) => 
                {
                    switch (arg1.Item.ItemId)
                    {
                        case Resource.Id.item1:
                            if (current_team == 1)
                            {
                                break;
                            }
                            else
                            {
                                datas.Clear();
                                for (int i = 0; i < AP.getteamlistnum(); i++)
                                {
                                    for (int j = 0; j < AP.getMyteamNumberCount(); j++)
                                    {
                                        if (AP.getTeam(i).getProjectNum() == AP.getMYteamNumber(j))
                                        {
                                            datas.Add(AP.getTeam(i));
                                        }
                                    }
                                }

                                adapter.NotifyDataSetChanged();
                                changelist.Text = "내 팀▼";
                                AP.set_isMyteam(1);
                            }
                            current_team = 1;
                            break;
                        case Resource.Id.item2:
                            if (current_team == 2)
                            {
                                break;
                            }
                            else
                            {
                                datas.Clear();

                                for (int i = 0; i < AP.getteamlistnum(); i++)
                                {
                                    for (int j = 0; j < AP.getMyteamNumberCount(); j++)
                                    {
                                        if (AP.getTeam(i).getProjectNum() != AP.getMYteamNumber(j) && AP.getTeam(i).getFinish() == 0)
                                        {
                                            datas.Add(AP.getTeam(i));
                                        }
                                    }
                                }

                                adapter.NotifyDataSetChanged();
                                changelist.Text = "진행중인 팀▼";
                                AP.set_isMyteam(0);
                            }
                            current_team = 2;
                            break;
                        case Resource.Id.item3:
                            if (current_team == 3)
                            {
                                break;
                            }
                            else
                            {
                                datas.Clear();
                                for (int i = 0; i < AP.getteamlistnum(); i++)
                                {
                                    for (int j = 0; j < AP.getMyteamNumberCount(); j++)
                                    {
                                        if (AP.getTeam(i).getFinish() == 1)
                                        {
                                            datas.Add(AP.getTeam(i));
                                        }
                                    }
                                }


                                adapter.NotifyDataSetChanged();
                                changelist.Text = "완료된 팀▼";
                                AP.set_isMyteam(0);
                            }
                            current_team = 3;
                            break;
                        default:
                            break;
                    }
                };

                menu.DismissEvent += (s2, arg2) =>
                {

                };

                menu.Show();
            };
        }
    }
}