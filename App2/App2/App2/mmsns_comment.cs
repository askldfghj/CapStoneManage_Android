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
    class mmsns_comment : Java.Lang.Object
    {
        int commentNum;
        int snsNum;
        int studentNum;
        string body;
        string createDate;

        public mmsns_comment(int cnum, int snsnum, int stunum, string con, string date)
        {
            commentNum = cnum;
            snsNum = snsnum;
            studentNum = stunum;
            body = con;
            createDate = date;
        }
        public int CommentNum
        {
            set { commentNum = value; }
            get { return commentNum; }
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
    }
}