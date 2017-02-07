using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace App2
{
    [Activity(MainLauncher = true, Theme = "@style/Theme.NoTitle")]
    //[Activity(Theme = "@style/Theme.NoTitle")]
    public class LoginActivity : Activity
    {
        private Client client;
        AppContext AP;
        Button loginbtn;
        EditText id;
        EditText pass;
        Button resibtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);
            // Create your application here
            AP = (AppContext)ApplicationContext;
            loginbtn = FindViewById<Button>(Resource.Id.loginbtn);
            id = FindViewById<EditText>(Resource.Id.id);
            pass = FindViewById<EditText>(Resource.Id.pass);
            resibtn = FindViewById<Button>(Resource.Id.registerbtn);

            AP.cleantasklist();
            AP.cleanteamlist();
            AP.cleanteammember();
            AP.cleanMyteamNum();
            AP.cleanaccountlist();
            AP.cleanmmslist();

            client = new Client();
            try
            {
                if (!client.Connected)
                {
                    client.Connect("203.230.253.114", 3301);  // 203.230.253.114
                }
                //textview1.Text = "접속성공";
            }
            catch (SocketException se)
            {

            }
            catch (Exception ex)
            {

            }

            client.DataReceived += new Client.DataReceivedEventHandler(client_DataReceived);
            client.Disconnected += new Client.DisconnectedEventHandler(client_Disconnected);
            client.ReceiveAsync();

            AP.setClient(client);
            client = AP.getClient();


            pass.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    loginbtn.RequestFocus();
                }
                else
                {
                    e.Handled = false;
                }
            };


            loginbtn.Click += delegate (object sender, EventArgs e)
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write((int)Commands.reqLogin);  // 로그인 요청 신호
                bw.Write(id.Text.ToString());
                bw.Write(pass.Text.ToString());
                bw.Close();
                byte[] data = ms.ToArray();
                ms.Dispose();
                client.Send(data, 0, data.Length);
                data = null;
            };

            resibtn.Click += delegate (object sender, EventArgs e)
            {
                Intent resi = new Intent();
                resi.SetClass(this, typeof(register_activity));
                StartActivity(resi);
            };
        }

        public class AsyncObject
        {
            public Byte[] Buffer;
            public Socket WorkingSocket;
            public AsyncObject(Int32 bufferSize)
            {
                this.Buffer = new Byte[bufferSize];
            }
        }



        void client_Disconnected(Client sender)
        {
            sender.Close();
            sender = null;
        }

        void client_DataReceived(Client sender, ReceiveBuffer e)
        {
            BinaryReader r = new BinaryReader(e.BufStream);

            Commands header = (Commands)r.ReadInt32();

            switch (header)
            {
                case Commands.reqLogin:  // 로그인
                    {
                        string s = r.ReadString();

                        if (String.Compare(s, "1") == 0)
                        {
                            //성공
                            //
                            Intent select = new Intent();
                            select.SetClass(this, typeof(team_select_activity));
                            AP.set_user_id(id.Text);
                            StartActivity(select);
                            Finish();
                        }
                        else
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "아이디, 혹은 비밀번호를 확인해주십시오", ToastLength.Short).Show());
                            //MessageBox.Show("아이디 혹은 비밀번호가 잘못되었습니다.");
                        }
                    }
                    break;
                case Commands.reqTeamList:
                    {
                        int myStudentNumber = r.ReadInt32();
                        AP.set_user_num(myStudentNumber);
                        int myTeamCount = r.ReadInt32();  // 내 팀 갯수를 받는다.
                        for (int i = 0; i < myTeamCount; i++)
                        {
                            AP.addMyteamNumber(r.ReadInt32());
                        }
                        int teamcount = r.ReadInt32();
                        for (int i = 0; i < teamcount; i++)
                        {
                            int teamNum = r.ReadInt32();
                            string teamTitle = r.ReadString();
                            int teamFinish = r.ReadInt32();
                            string introduction = r.ReadString();

                            project_struct project = new project_struct(teamNum, teamTitle, introduction, teamFinish);

                            int teamc = r.ReadInt32();

                            for (int j = 0; j < teamc; j++)
                            {
                                project.addMember(String.Copy(r.ReadString()));
                            }
                            AP.addteamlist(project);
                        }

                        AP.setstate(true);
                    }
                    break;
                case Commands.reqRegister:
                    {
                        string s = r.ReadString();

                        if (String.Compare(s, "1") == 0)
                        {
                            AP.setregi_state(1);
                        }
                        else
                        {
                            AP.setregi_state(2);
                        }
                    }
                    break;
                case Commands.reqTaskList:
                    {
                        //RunOnUiThread(() => Toast.MakeText(this, "good", ToastLength.Short).Show());
                        AP.cleantasklist();

                        //DuMMy
                        int barCount = r.ReadInt32();  // 막대 갯수를 받는다.
                        for (int i = 0; i < barCount; i++)
                        {
                            int barNum = r.ReadInt32();
                            int projectNum = r.ReadInt32();
                            string work = r.ReadString();
                            int rowIndex = r.ReadInt32();
                            string startDate = r.ReadString();
                            string endDate = r.ReadString();
                            string color = r.ReadString();
                        }
                        //DuMMy

                        int taskCount = r.ReadInt32();  // 일정갯수
                        for (int i = 0; i < taskCount; i++)
                        {
                            int taskNum = r.ReadInt32();
                            int projectNum = r.ReadInt32();
                            int studentNum = r.ReadInt32();
                            int fileNum = r.ReadInt32();  // 파일이 없으면 0, NULL이 아님 넣을때도 그렇게 할거임
                            string fileName = r.ReadString();
                            string title = r.ReadString();
                            string contents = r.ReadString();
                            string startDate = r.ReadString();
                            string endDate = r.ReadString();
                            int isFinish = r.ReadInt32();
                            
                            TaskData_Title temptask = new TaskData_Title(taskNum, projectNum, studentNum, fileNum, fileName, title, contents, startDate, endDate, isFinish);
                            AP.addtasklist(temptask);

                            int taskCommentCount = r.ReadInt32();  // 일정 덧글 갯수
                            TaskWorking tempTaskwork;
                            for (int j = 0; j < taskCommentCount; j++)
                            {
                                int commentNum = r.ReadInt32();
                                int tNum = r.ReadInt32();
                                int fnum = r.ReadInt32();
                                string fname = r.ReadString();
                                int sNum = r.ReadInt32();
                                string cont = r.ReadString();
                                string date = r.ReadString();

                                tempTaskwork = new TaskWorking(fnum, commentNum, tNum, fname, sNum, date, cont);
                                AP.gettask(i).addreplylist(tempTaskwork);
                            }                       
                        }

                        AP.setstate(true);
                    }
                    break;
                case Commands.reqTaskModify:
                    {
                        int whatSignal = r.ReadInt32();

                        if (whatSignal == 1)  // 추가 완료
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "일정을 추가했습니다.", ToastLength.Short).Show());                 
                        }
                        else if (whatSignal == 2)  // 수정 완료
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "일정을 수정했습니다.", ToastLength.Short).Show());
                        }
                        else  // 삭제 완료
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "일정을 삭제했습니다.", ToastLength.Short).Show());
                        }

                        AP.setstate(true);
                    }
                    break;
                case Commands.reqAccountingList:
                    {
                        AP.cleanaccountlist();
                        int accountingCount = r.ReadInt32();  // 회계 내역 갯수를 받는다.
                        for (int i = 0; i < accountingCount; i++)
                        {
                            int accountingNum = r.ReadInt32();
                            int projectNum = r.ReadInt32();
                            string title = r.ReadString();
                            string contents = r.ReadString();
                            int price = r.ReadInt32();
                            int isPlusMinus = r.ReadInt32();

                            Account_Contents accounting = new Account_Contents(accountingNum, projectNum, title, contents, price, isPlusMinus);
                            AP.addaccountlist(accounting);
                        }

                        AP.setstate(true);
                    }
                    break;
                case Commands.reqAccountingModify:
                    {
                        int whatSignal = r.ReadInt32();

                        if (whatSignal == 1)  // 추가 완료
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "내역을 추가했습니다.", ToastLength.Short).Show());
                        }
                        else if (whatSignal == 2)  // 수정 완료
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "내역을 수정했습니다.", ToastLength.Short).Show());
                        }
                        else  // 삭제 완료
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "내역을 삭제했습니다.", ToastLength.Short).Show());
                        }

                        AP.setstate(true);
                    }
                    break;
                case Commands.reqAddTaskComment:
                    {
                        int whatSignal = r.ReadInt32();

                        if (whatSignal == 1)
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "답글을 달았습니다.", ToastLength.Short).Show());
                        }

                        AP.setstate(true);
                    }
                    break;
                case Commands.reqSNSList:
                    {
                        AP.cleanmmslist();
                        int SNSCount = r.ReadInt32();  // SNS 글 갯수
                        for (int i = 0; i < SNSCount; i++)
                        {
                            int SNSNum = r.ReadInt32();
                            int studentNum = r.ReadInt32();
                            string contents = r.ReadString();
                            string date = r.ReadString();

                            mmsStruct sns = new mmsStruct(SNSNum, studentNum, contents, date);

                            AP.addmmslist(sns);

                            int SNSCommentCount = r.ReadInt32();  // SNS 글의 덧글 갯수
                            for (int j = 0; j < SNSCommentCount; j++)
                            {
                                int commentNum = r.ReadInt32();
                                int SNum = r.ReadInt32();
                                int stNum = r.ReadInt32();
                                string c = r.ReadString();
                                string d = r.ReadString();

                                mmsns_comment tempSC = new mmsns_comment(commentNum, SNum, stNum, c, d);
                                AP.getmmsIndex(i).addcommentlist(tempSC);
                            }
                        }

                        AP.setstate(true);
                    }
                    break;

                case Commands.reqAddSNSComment:
                    {
                        int whatSignal = r.ReadInt32();
                        RunOnUiThread(() => Toast.MakeText(this, "답글을 달았습니다.", ToastLength.Short).Show());
                        AP.setstate(true);
                    }
                    break;
                case Commands.reqSNSModify:
                    {
                        int whatSignal = r.ReadInt32();

                        if (whatSignal == 1)
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "작성이 완료되었습니다.", ToastLength.Short).Show());
                        }
                        else
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "수정했습니다.", ToastLength.Short).Show());
                        }
                        AP.setstate(true);
                    }
                    break;
                case Commands.reqFileDownload:
                    {
                        int fileSize = r.ReadInt32();
                        string fileName = r.ReadString();
                        byte[] b = r.ReadBytes(fileSize);

                        ByteArrayToFile(fileName, b);

                        RunOnUiThread(() => Toast.MakeText(this, "파일을 다운로드 하였습니다.", ToastLength.Short).Show());

                        AP.setstate(true);
                    }
                    break;
            }
        }
        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)  // byte[] -> file
        {
            try
            {
                // Open file for reading
                string directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                string dir = Path.Combine(directory, AP.getcurrentTask().FileName);
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(dir, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                

                // close file stream
                _FileStream.Close();
                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }
    }
}