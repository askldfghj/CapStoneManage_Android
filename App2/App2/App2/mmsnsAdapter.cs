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
    class mmsnsAdapter : BaseAdapter
    {
        List<mmsStruct> datas;
        LayoutInflater inflater;


        public mmsnsAdapter(LayoutInflater inf, List<mmsStruct> d)
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

            if (datas[position].LastCommenter == 0)
            {
                convertView = inflater.Inflate(Resource.Layout.mmsns_list_layout_solo, null);
                TextView mmsMan = convertView.FindViewById<TextView>(Resource.Id.sns_writer);
                TextView mmsDate = convertView.FindViewById<TextView>(Resource.Id.sns_write_date);
                TextView mmsBody = convertView.FindViewById<TextView>(Resource.Id.sns_body);

                mmsMan.Text = datas[position].StudentNum.ToString();
                mmsDate.Text = datas[position].CreateDate;
                mmsBody.Text = datas[position].Body;
            }
            else
            {
                convertView = inflater.Inflate(Resource.Layout.mmsns_list_layout, null);
                TextView mmsMan = convertView.FindViewById<TextView>(Resource.Id.sns_writer);
                TextView mmsDate = convertView.FindViewById<TextView>(Resource.Id.sns_write_date);
                TextView mmsBody = convertView.FindViewById<TextView>(Resource.Id.sns_body);
                TextView mmscmntMan = convertView.FindViewById<TextView>(Resource.Id.sns_comment_writer);
                TextView mmscmntBody = convertView.FindViewById<TextView>(Resource.Id.sns_comment_body);

                mmsMan.Text = datas[position].StudentNum.ToString();
                mmsDate.Text = datas[position].CreateDate;
                mmsBody.Text = datas[position].Body;
                mmscmntMan.Text = datas[position].LastCommenter.ToString();
                mmscmntBody.Text = datas[position].LastComment;
            }

            


            return convertView;
        }
    }
}