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
    class ProjectAdapter : BaseAdapter
    {
        List<project_struct> datas;
        LayoutInflater inflater;

        public ProjectAdapter(LayoutInflater inf, List<project_struct> d)
        {
            this.datas = d;
            this.inflater = inf;
        }

        public override int Count
        {
            get
            {
                return datas.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return datas[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.team_list, null);
            }


            TextView projectname = convertView.FindViewById<TextView>(Resource.Id.project_name);
            TextView projectmember = convertView.FindViewById<TextView>(Resource.Id.project_member);

            projectname.Text = datas[position].Project_Name;
            string str = "";
            for (int i = 0; i < datas[position].getMembernum(); i++)
            {
                str += datas[position].getMemberIndex(i) + " ";
            }
            projectmember.Text = str;

            return convertView;
        }
    }
}