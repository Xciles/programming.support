using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Webkit;
using Android.Views;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for PizzaViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class PizzaView : MvxActivity
    {
		private WebView _webView;

        protected override void OnCreate(Bundle bundle)
        {
			ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PizzaView);
        }
    }
}
