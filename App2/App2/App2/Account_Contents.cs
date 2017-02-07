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

namespace App2
{
    class Account_Contents : Java.Lang.Object
    {
        int accountNum;
        int projectNum;
        string accountName;
        int accountCost;
        string accountDesc;
        int isPlusMinus;



        public Account_Contents(int accnum, int pjnum, string name, string con, int money, int ispm)
        {
            accountNum = accnum;
            projectNum = pjnum;
            accountName = name;
            accountCost = money;
            accountDesc = con;
            isPlusMinus = ispm;
        }
        public int AccountNum
        {
            set { accountNum = value; }
            get { return accountNum; }
        }
        public int ProjectNum
        {
            set { projectNum = value; }
            get { return projectNum; }
        }

        public string AccountName
        {
            set { accountName = value; }
            get { return accountName; }
        }
        public int AccountCost
        {
            set { accountCost = value; }
            get { return accountCost; }
        }
        public string AccountDesc
        {
            set { accountDesc = value; }
            get { return accountDesc; }
        }
        public int IsPlusMinus
        {
            set { isPlusMinus = value; }
            get { return isPlusMinus; }
        }
    }
}