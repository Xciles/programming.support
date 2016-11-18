using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Webkit;
using Android.Views;
using ProgrammingSupport.Core.ViewModels;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for FirstViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class FirstView : MvxActivity
    {
		private WebView _webView;

        private FirstViewModel FirstViewModel
        {
            get { return (ViewModel as FirstViewModel); }
        }

        protected override void OnCreate(Bundle bundle)
        {
			ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);

			string fileName = "file:///android_asset/Content/southparkidle.gif";

			_webView = FindViewById<WebView>(Resource.Id.webview);
			_webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<img style=\"width: 100%; height:100%; margin:0px; padding:0px;\" id=\"selector\" src=\"" + fileName + "\"></body></html>", "text/html", "utf-8", null);

			//_webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<img style=\"width: 100%; height:100%; margin:0px; padding:0px;\" id=\"selector\" src=\"https://media.giphy.com/media/1iUlZloL4DiH8HL2/giphy.gif\"></body></html>", "text/html", "utf-8", null);
			_webView.Settings.JavaScriptEnabled = true;
			_webView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            //FirstViewModel.GoToAnswerCommand.Execute(null);
        }
    }
}
