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
using System.Threading.Tasks;
using ProgrammingSupport.Core.ViewModels;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for FirstViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class FirstView : MvxActivity, TextToSpeech.IOnInitListener
    {
		private WebView _webView;
		private RelativeLayout _click;
        private ImageView _text;
		private bool isRecording = false;
		private readonly int VOICE = 10;
		private TextToSpeech speaker;
		private string toSpeak;
		private Intent _voiceIntent;

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

			string fileName = "file:///android_asset/Content/southparkidle.gif";
			_webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<img style=\"width: 100%; height:100%; margin:0px; padding:0px;\" id=\"selector\" src=\"" + fileName + "\"></body></html>", "text/html", "utf-8", null);
			//_webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<img style=\"width: 100%; height:100%; margin:0px; padding:0px;\" id=\"selector\" src=\"https://media.giphy.com/media/1iUlZloL4DiH8HL2/giphy.gif\"></body></html>", "text/html", "utf-8", null);
			_webView.Settings.JavaScriptEnabled = true;
			_webView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            //FirstViewModel.GoToAnswerCommand.Execute(null);


            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
			if (rec != "android.hardware.microphone")
			{
				var alert = new AlertDialog.Builder(this);
				alert.SetTitle("You don't seem to have a microphone to record with");
				alert.Show();
			}
			else
				_click.Click += delegate
				{
                    _text.Visibility = ViewStates.Invisible;
					isRecording = !isRecording;
					if (isRecording)
					{
						// create the intent and start the activity
						_voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

						// put a message on the modal dialog
						_voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now!");

						// if there is more then 1.5s of silence, consider the speech over
						_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 2500);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 2500);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 20000);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

						// you can specify other languages recognised here, for example
						// voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
						// if you wish it to recognise the default Locale language and German
						// if you do use another locale, regional dialects may not be recognised very well

						_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.English);
						StartActivityForResult(_voiceIntent, VOICE);
					}
				};
        }

		private bool pizza = false;

		protected override async void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						string textInput = matches[0];

						// limit the output to 500 characters
						if (textInput.Length > 500)
							textInput = textInput.Substring(0, 500);
						var ditIsResult = textInput;
						if(!pizza)
						{
                            Random rnd = new Random();
                            if (rnd.Next(2) == 0)
                                _text.SetImageResource(Resource.Drawable.txtImSorry);
                            else
                                _text.SetImageResource(Resource.Drawable.txtPizza);
                            _text.Visibility = ViewStates.Visible;
                            Speak("I am sorry, I could not find " + ditIsResult + ". Would you like a pizza?");
							pizza = true;
							await Task.Delay(5000).ConfigureAwait(false);
							StartActivityForResult(_voiceIntent, VOICE);
						}
						else if(pizza)
						{
							if(ditIsResult.ToLower().Contains("yes"))
							{
								Speak("I will get you a pepperoni pizza, buddy!");
								pizza = false;
							}
							else if (ditIsResult.ToLower().Contains("no"))
							{
								Speak("More for me, asshole!");
								pizza = false;
							}
							else
							{
								Speak("Speak up, mumbling idiot!");
							}
						}
					}
					else
						Speak("No speech was recognised");
					isRecording = false;
				}
			}

			base.OnActivityResult(requestCode, resultVal, data);
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
			else {
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
	}
}
