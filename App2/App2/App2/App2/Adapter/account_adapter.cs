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
    class account_adapter : BaseAdapter
    {
        List<Account_Contents> datas;
        LayoutInflater inflater;

        public account_adapter(LayoutInflater inf, List<Account_Contents> d)
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
                convertView = inflater.Inflate(Resource.Layout.account_list, null);
            }

            TextView accountName = convertView.FindViewById<TextView>(Resource.Id.isaccname);
            TextView accountCost = convertView.FindViewById<TextView>(Resource.Id.isacccost);
            TextView accountDesc = convertView.FindViewById<TextView>(Resource.Id.isaccdesc);

            accountName.Text = "이름 : " + datas[position].AccountName;
            accountCost.Text = "가격 : " + datas[position].AccountCost.ToString();
            accountDesc.Text = "내용 : " + datas[position].AccountDesc;

            return convertView;
        }
    }
}