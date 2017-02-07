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
    public class TaskWorking : Java.Lang.Object
    {
        int fileNum;
        int commentNum;
        int taskNum;
        string fileName;
        int person;
        string createDate;
        string data;

        public TaskWorking(int FileN, int CommentN, int TaskN, string FileT, int StuN, string CreateD, string comment)
        {
            fileNum = FileN;
            commentNum = CommentN;
            taskNum = TaskN;
            fileName = FileT;
            person = StuN;
            createDate = CreateD;
            data = comment;
        }

        public int FileNum
        {
            set { fileNum = value; }
            get { return fileNum; }
        }
        public int CommentNum
        {
            set { commentNum = value; }
            get { return commentNum; }
        }
        public string FileName
        {
            set { fileName = value; }
            get { return fileName; }
        }
        public int Person
        {
            set { person = value; }
            get { return person; }
        }
        public string CreateDate
        {
            set { createDate = value; }
            get { return createDate; }
        }
        public string Data
        {
            set { data = value; }
            get { return data; }
        }

    }


}