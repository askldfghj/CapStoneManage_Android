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
    class mmsStruct : Java.Lang.Object
    {
        int snsNum;
        int studentNum;
        string createDate;
        string body;
        List<mmsns_comment> comment_list = new List<mmsns_comment>();
        string last_comment = "";
        int last_commenter = 0;

        public mmsStruct(int snnum, int stunum, string con, string date)
        {
            snsNum = snnum;
            studentNum = stunum;
            body = con;
            createDate = date;
        }

        public int SnsNum
        {
            set { snsNum = value; }
            get { return snsNum; }
        }
        public int StudentNum
        {
            set { studentNum = value; }
            get { return studentNum; }
        }
        public int LastCommenter
        {
            get { return last_commenter; }
        }
        public string Body
        {
            set { body = value; }
            get { return body; }
        }
        public string CreateDate
        {
            set { createDate = value; }
            get { return createDate; }
        }
        public string LastComment
        {
            get { return last_comment; }
        }
        public void addcommentlist(mmsns_comment m)
        {
            comment_list.Add(m);
        }
        public List<mmsns_comment> getcommentlist()
        {
            return comment_list;
        }
        public void setLastcomment()
        {
            if (comment_list.Count > 0)
            {
                last_commenter = comment_list[comment_list.Count() - 1].StudentNum;
                last_comment = comment_list[comment_list.Count() - 1].Body;
            }
        }
    }
}