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

using System.IO;
using System.Threading.Tasks;

namespace App2
{
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class register_activity : Activity
    {

        EditText id;
        EditText pass;
        EditText name;
        EditText number;
        CheckBox check;
        Button donebtn;


        AppContext AP;
        Client client;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register_layout);
            // Create your application here

            id = FindViewById<EditText>(Resource.Id.id);
            pass = FindViewById<EditText>(Resource.Id.pass);
            name = FindViewById<EditText>(Resource.Id.name);
            number = FindViewById<EditText>(Resource.Id.number);
            donebtn = FindViewById<Button>(Resource.Id.donebtn);
            check = FindViewById<CheckBox>(Resource.Id.check);


            AP = (AppContext)ApplicationContext;

            client = AP.getClient();

            donebtn.Click += delegate
            {
                if (id.Text == "")
                {
                    id.RequestFocus();
                }
                else if (pass.Text == "")
                {
                    pass.RequestFocus();
                }
                else if (name.Text == "")
                {
                    name.RequestFocus();
                }
                else if (number.Text == "")
                {
                    number.RequestFocus();
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);
                    bw.Write((int)Commands.reqRegister);  // 회원가입 요청 신호
                    bw.Write(id.Text);
                    bw.Write(pass.Text);
                    bw.Write(name.Text);
                    bw.Write((int)Convert.ToInt32(number.Text));

                    if (check.Checked)
                    {
                        bw.Write(1);  // 교수이면 1
                    }
                    else
                        bw.Write(0);  // 학생이면 0
                    bw.Close();
                    byte[] data = ms.ToArray();
                    ms.Dispose();
                    client.Send(data, 0, data.Length);
                    data = null;
                }

                while (AP.getregi_state() == 0)
                {
                    Task.Delay(5).Wait();
                }
                if (AP.getregi_state() == 1)
                {
                    Toast.MakeText(this, "회원가입이 되었습니다.", ToastLength.Short).Show();
                    Finish();
                    AP.setregi_state(0);
                }
                else
                {
                    Toast.MakeText(this, "문제가 발생했습니다.", ToastLength.Short).Show();
                    AP.setregi_state(0);
                }
            };
        }
    }
}