using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace App2
{
    public class TaskDataAdapter : BaseAdapter
    {
        List<TaskData_Title> datas;
        LayoutInflater inflater;

        public TaskDataAdapter(LayoutInflater inf, List<TaskData_Title> d)
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
                convertView = inflater.Inflate(Resource.Layout.Inflated_layout, null);
            }


            TextView taskName = convertView.FindViewById<TextView>(Resource.Id.taksname);
            TextView taskCreatedate = convertView.FindViewById<TextView>(Resource.Id.createdate);
            TextView taskDeadLine = convertView.FindViewById<TextView>(Resource.Id.deadline);

            taskName.Text = datas[position].Title;
            taskCreatedate.Text = "시작 : " + datas[position].CreateDate;
            taskDeadLine.Text = "마감 : " + datas[position].DeadLine;

            return convertView;
        }
    }
}