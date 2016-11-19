using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Android.Webkit;
using Android.Views;
using Android.Widget;
using Android.Speech.Tts;
using Android.Content;
using Android.Speech;
using ProgrammingSupport.Core.ViewModels;
using System.Collections.Generic;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for AnswerViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AnswerView : MvxActivity, TextToSpeech.IOnInitListener
    {
		private WebView _webView;
        private LinearLayout _click;
        private bool isRecording = false;
        private readonly int VOICE = 10;
        private TextToSpeech speaker;
        private string toSpeak;
        private Intent _voiceIntent;

        protected override void OnCreate(Bundle bundle)
        {
			ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AnswerView);

            (ViewModel as AnswerViewModel).AnswerUpdated += OnAnswerUpdated;

        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            base.OnActivityResult(requestCode, resultVal, data);
        }

        private void OnAnswerUpdated()
        {
            Speak((ViewModel as AnswerViewModel).Answer);
        }

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(this, this);
                speaker.SetPitch(1.5f);
                speaker.SetSpeechRate(1.5f);
                speaker.SetLanguage(Java.Util.Locale.English);
            }
            else
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, p);
            }
        }

        void TextToSpeech.IOnInitListener.OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, p);
            }

        }

        public  override void OnBackPressed()
        {
            //speaker.Stop();
            base.OnBackPressed();
        }
    }
}
