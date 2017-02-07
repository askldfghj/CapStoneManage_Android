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
    public class TaskData_Title : Java.Lang.Object
    {
        int taskNum;
        int projectNum;
        int studentNum;
        int filenum;
        string filename;
        string TaskTitle;
        string contents;
        string TaskCreateDate;
        string TaskDeadLine;
        int isFinish;
        List<TaskWorking> reply_list = new List<TaskWorking>();

        public TaskData_Title(int TaskNum, int ProjectNum, int StudentNum, int fn, string fname, string Title, string Contents, string CreateDate, string DeadLine, int IsFinish)
        {
            taskNum = TaskNum;
            projectNum = ProjectNum;
            studentNum = StudentNum;
            filenum = fn;
            filename = fname;
            TaskTitle = Title;
            contents = Contents;
            TaskCreateDate = CreateDate;
            TaskDeadLine = DeadLine;
            isFinish = IsFinish;
        }

        public int TaskNum
        {
            set { taskNum = value; }
            get { return taskNum; }
        }
        public int ProjectNum
        {
            set { projectNum = value; }
            get { return projectNum; }
        }
        public int StudentNum
        {
            set { studentNum = value; }
            get { return studentNum; }
        }
        public int FileNum
        {
            set { filenum = value; }
            get { return filenum; }
        }
        public string FileName
        {
            set { filename = value; }
            get { return filename; }
        }
        public string Title
        {
            set { TaskTitle = value; }
            get { return TaskTitle; }
        }
        public string Contents
        {
            set { contents = value; }
            get { return contents; }
        }
        public string CreateDate
        {
            set { TaskCreateDate = value; }
            get { return TaskCreateDate; }
        }
        public string DeadLine
        {
            set { TaskDeadLine = value; }
            get { return TaskDeadLine; }
        }
        public int IsFinish
        {
            set { isFinish = value; }
            get { return IsFinish; }
        }
        public void addreplylist(TaskWorking t)
        {
            reply_list.Add(t);
        }
        public List<TaskWorking> getreplylist()
        {
            return reply_list;
        }
        public TaskWorking getreplylistIndex(int i)
        {
            return reply_list[i];
        }
        public void cleanreplylist()
        {
            reply_list.Clear();
        }
    }
}