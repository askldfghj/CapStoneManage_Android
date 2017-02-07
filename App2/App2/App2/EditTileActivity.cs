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
    [Activity(Theme = "@style/Theme.NoTitle")]
    public class EditTileActivity : Activity
    {
        EditText edi;
        TextView done;
        ImageView close;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditTitle_layout);
            // Create your application here

            edi = FindViewById<EditText>(Resource.Id.edittitletext);
            done = FindViewById<TextView>(Resource.Id.projecteditdone);
            close = FindViewById<ImageView>(Resource.Id.projectnameeditout);
            close.Touch += delegate (object sender, View.TouchEventArgs touchEventArgs)
            {
                ImageView tv = (ImageView)sender;
                switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        tv.SetImageResource(Resource.Drawable.close_down);
                        break;
                    case MotionEventActions.Up:
                        tv.SetImageResource(Resource.Drawable.close_up);
                        Finish();
                        break;
                    default:
                        break;
                }
            };
            done.Click += delegate { Done(); };
        }

        void Done()
        {
            string str = edi.Text;
            Intent intent = new Intent();
            intent.PutExtra("Title", str);
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}