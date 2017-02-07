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
using Java.Lang;

namespace App2
{
    class TaskWorkAdapter : BaseAdapter
    {
        List<TaskWorking> datas;
        LayoutInflater inflater;

        public TaskWorkAdapter(LayoutInflater inf, List<TaskWorking> d)
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
            convertView = null;
            if (datas[position].FileNum == 0)
            { 
                convertView = inflater.Inflate(Resource.Layout.Task_menssion, null);
                TextView ismessionperson = convertView.FindViewById<TextView>(Resource.Id.ismenssionperson);
                TextView ismessionCdate = convertView.FindViewById<TextView>(Resource.Id.ismenssionCdate);
                TextView ismessiondata = convertView.FindViewById<TextView>(Resource.Id.ismenssiondata);
                ismessionperson.Text = datas[position].Person.ToString();
                ismessionCdate.Text = "작성일자 : " + datas[position].CreateDate;
                ismessiondata.Text = datas[position].Data;
            }
            else
            {
                convertView = inflater.Inflate(Resource.Layout.Task_file, null);
                TextView isfileperson = convertView.FindViewById<TextView>(Resource.Id.isfileperson);
                TextView isfileCdate = convertView.FindViewById<TextView>(Resource.Id.isfileCdate);
                TextView isfiledata = convertView.FindViewById<TextView>(Resource.Id.isfiledata);
                ViewGroup filelayout = convertView.FindViewById<ViewGroup>(Resource.Id.filelayout);
                filelayout.Click += delegate { isfiledata.Text = "파일 다운"; }; //여기다 파일 다운로더 적용하면 됨
                isfileperson.Text = datas[position].Person.ToString();
                isfileCdate.Text = "작성일자 : " + datas[position].CreateDate;
                isfiledata.Text = datas[position].Data;
            }

            

            return convertView;
        }
    }
}