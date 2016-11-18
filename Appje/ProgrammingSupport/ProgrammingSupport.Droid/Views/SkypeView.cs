using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Webkit;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Content;
using Android.Speech;
using Android.Speech.Tts;
using System;
using System.Collections.Generic;
using ProgrammingSupport.Core.ViewModels;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for SkypeViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class SkypeView : MvxActivity
    {
		private WebView _webView;
		private RelativeLayout _click;
		private bool isRecording = false;
		private readonly int VOICE = 10;
		private TextToSpeech speaker;
		private string toSpeak;

        private FirstViewModel FirstViewModel
        {
            get { return (ViewModel as FirstViewModel); }
        }

        protected override void OnCreate(Bundle bundle)
        {
			ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);

			_webView = FindViewById<WebView>(Resource.Id.webview);
			_click = FindViewById<RelativeLayout>(Resource.Id.clickLayout);

			string skypeLink = "https://webchat.botframework.com/embed/ProgrammingSupport?s=Oxbi0KRMdao.cwA.LL0.UkEzZ_-KnDacY-AnmwF0ArHNpJIQfZVuTqzcoIDOlhg";
			_webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<iframe style=\"width: 100%; height:100%; margin:0px; padding:0px;\" src='" + skypeLink + "'></iframe></body></html>", "text/html", "utf-8", null);
			_webView.Settings.JavaScriptEnabled = true;
			_webView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
	}
}
