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
    class mmsns_comment_adapter : BaseAdapter
    {
        List<mmsns_comment> datas;
        LayoutInflater inflater;

        public mmsns_comment_adapter(LayoutInflater inf, List<mmsns_comment> d)
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
                convertView = inflater.Inflate(Resource.Layout.mmsns_comment_layout, null);
            }

            TextView mmsMan = convertView.FindViewById<TextView>(Resource.Id.sns_comment_writer);
            TextView mmsDate = convertView.FindViewById<TextView>(Resource.Id.sns_comment_day);
            TextView mmsBody = convertView.FindViewById<TextView>(Resource.Id.sns_comment_body);

            mmsMan.Text = datas[position].StudentNum.ToString();
            mmsDate.Text = datas[position].CreateDate;
            mmsBody.Text = datas[position].Body;


            return convertView;
        }
    }
}