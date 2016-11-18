using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace ProgrammingSupport.Droid
{
    [Activity(
        Label = "ProgrammingSupport.Droid"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
		, ScreenOrientation = ScreenOrientation.Landscape)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
