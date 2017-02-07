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
    public class ProjectEditActivity : Activity
    {
        ViewGroup editTile;
        TextView titleText;
        ImageView close;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProjectEditLayout);
            // Create your application here

            editTile = FindViewById<ViewGroup>(Resource.Id.edittitlelayout);
            titleText = FindViewById<TextView>(Resource.Id.edittitleresult);
            close = FindViewById<ImageView>(Resource.Id.projecteditout);
            editTile.Click += delegate { OpenEditTitle(); };

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
        }

        void OpenEditTitle()
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(EditTileActivity));
            StartActivityForResult(intent, 0);

        }

        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && data.GetStringExtra("Title") != "")
            {
                
                titleText.Text = data.GetStringExtra("Title");
            }
        }
    }
}   