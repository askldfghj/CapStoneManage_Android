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
    class project_struct : Java.Lang.Object
    {
        string project_name;
        List<string> project_member = new List<string>();
        int project_finish;
        int project_num;
        string introduction;

        public project_struct(int num, string title, string intro, int finish)
        {
            project_num = num;
            project_name = title;
            introduction = intro;
            project_finish = finish;
        }

        public string Project_Name
        {
            set { project_name = value; }
            get { return project_name; }
        }

        public string Introduction
        {
            set { introduction = value; }
            get { return introduction; }
        }

        public void addMember(string member)
        {
            project_member.Add(member);
        }

        public string getMemberIndex(int i)
        {
            return project_member[i];
        }

        public List<string> getMeberList()
        {
            return project_member;
        }

        public int getMembernum()
        {
            return project_member.Count;
        }

        public void setFinish(int n)
        {
            project_finish = n;
        }

        public int getFinish()
        {
            return project_finish;
        }

        public void setProjectNum(int n)
        {
            project_num = n;
        }

        public int getProjectNum()
        {
            return project_num;
        }

        public void setProjectName(string n)
        {
            project_name = n;
        }

        public string getProjectName()
        {
            return project_name;
        }
    }
}