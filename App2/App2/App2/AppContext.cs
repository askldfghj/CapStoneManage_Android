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
using System.Net.Sockets;

namespace App2
{
    [Application]
    class AppContext : Application
    {
        private static Client client;
        static bool isReceiving = false;
        static int regist_comp = 0;
        static string user_id;
        static int user_num;
        static int thisMyteam;
        static List<int> MyteamNum = new List<int>();
        static List<project_struct> _teamList = new List<project_struct>();
        static string current_teamTitle;
        static int current_teamNum;
        static List<string> current_teamMember = new List<string>();
        static List<TaskData_Title> current_Tasklist = new List<TaskData_Title>();
        static TaskData_Title current_Task;
        static Account_Contents current_Account;
        static mmsStruct current_sns;
        static List<Account_Contents> account_list = new List<Account_Contents>();
        static List<mmsStruct> mms_list = new List<mmsStruct>();

        public AppContext(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {

        }
        public override void OnCreate()
        {
            // If OnCreate is overridden, the overridden c'tor will also be called.
            base.OnCreate();
        }

        public void setClient(Client cl)
        {
            client = cl;
        }

        public Client getClient()
        {
            return client;
        }

        public void setstate(bool s)
        {
            isReceiving = s;
        }

        public bool getstate()
        {
            return isReceiving;
        }

        public void setregi_state(int s)
        {
            regist_comp = s;
        }

        public int getregi_state()
        {
            return regist_comp;
        }

        public void set_user_num(int s)
        {
            user_num = s;
        }

        public int get_user_num()
        {
            return user_num;
        }

        public void set_user_id(string s)
        {
            user_id = s;
        }

        public string get_user_id()
        {
            return user_id;
        }

        public void set_currentTeamTitle(string s)
        {
            current_teamTitle = s;
        }

        public string get_currentTeamTitle()
        {
            return current_teamTitle;
        }

        public void set_isMyteam(int s)
        {
            thisMyteam = s;
        }

        public int get_isMyteam()
        {
            return thisMyteam;
        }

        public void set_currentTeamNum(int s)
        {
            current_teamNum = s;
        }

        public int get_currentTeamNum()
        {
            return current_teamNum;
        }

        public void addMyteamNumber(int n)
        {
            MyteamNum.Add(n);
        }

        public int getMyteamNumberCount()
        {
            return MyteamNum.Count;
        }

        public int getMYteamNumber(int i)
        {
            return MyteamNum[i];
        }
        public void cleanMyteamNum()
        {
            MyteamNum.Clear();
        }
        public void addteamlist(project_struct p)
        {
            _teamList.Add(p);
        }

        public int getteamlistnum()
        {
            return _teamList.Count;
        }

        public project_struct getTeam(int i)
        {
            return _teamList[i];
        }

        public void cleanteamlist()
        {
            _teamList.Clear();
        }

        public void set_currentteammember(List<string> s)
        {
            current_teamMember = s;
        }

        public string getteammemberIndex(int i)
        {
            return current_teamMember[i];
        }

        public void cleanteammember()
        {
            current_teamMember.Clear();
        }

        public void addtasklist(TaskData_Title p)
        {
            current_Tasklist.Add(p);
        }

        public int gettasklistnum()
        {
            return current_Tasklist.Count;
        }

        public TaskData_Title gettask(int i)
        {
            return current_Tasklist[i];
        }
        public List<TaskData_Title> gettasklist()
        {
            return current_Tasklist;
        }

        public void cleantasklist()
        {
            current_Tasklist.Clear();
        }

        public void setcurrentTask(TaskData_Title t)
        {
            current_Task = t;
        }
        public TaskData_Title getcurrentTask()
        {
            return current_Task;
        }

        public void setaccountContents(Account_Contents t)
        {
            current_Account = t;
        }
        public Account_Contents getaccountContents()
        {
            return current_Account;
        }

        public void setmmsnsContents(mmsStruct t)
        {
            current_sns = t;
        }
        public mmsStruct getmmsnsContents()
        {
            return current_sns;
        }

        public void addaccountlist(Account_Contents p)
        {
            account_list.Add(p);
        }

        public int getaccountlistnum()
        {
            return account_list.Count;
        }

        public Account_Contents getaccountIndex(int i)
        {
            return account_list[i];
        }
        public List<Account_Contents> getaccountlist()
        {
            return account_list;
        }

        public void cleanaccountlist()
        {
            account_list.Clear();
        }

        public void addmmslist(mmsStruct p)
        {
            mms_list.Add(p);
        }

        public int getmmslistnum()
        {
            return mms_list.Count;
        }

        public mmsStruct getmmsIndex(int i)
        {
            return mms_list[i];
        }
        public List<mmsStruct> getmmslist()
        {
            return mms_list;
        }

        public void cleanmmslist()
        {
            mms_list.Clear();
        }
    }
}