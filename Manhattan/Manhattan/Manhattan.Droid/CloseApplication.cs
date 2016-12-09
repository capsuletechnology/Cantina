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
using Xamarin.Forms;
using Manhattan.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace Manhattan.Droid
{
    public class CloseApplication : ICloseApplication
    {
        public CloseApplication() { }

        public void closeApplication()
        {
            //var activity = (Activity)Forms.Context;
            //activity.FinishAffinity();

            Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}