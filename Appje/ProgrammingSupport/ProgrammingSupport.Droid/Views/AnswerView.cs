using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Webkit;
using Android.Views;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for AnswerViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AnswerView : MvxActivity
    {
		private WebView _webView;

        protected override void OnCreate(Bundle bundle)
        {
			ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AnswerView);
        }
    }
}
