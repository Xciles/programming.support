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
using AltBeaconOrg.BoundBeacon;
using System.Linq;
using System;
using ProgrammingSupport.Core;

namespace ProgrammingSupport.Droid.Views
{
	[Activity(Label = "View for QuestionViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class QuestionView : MvxActivity, TextToSpeech.IOnInitListener, IBeaconConsumer
    {
		private LinearLayout _click;
		private bool isRecording = false;
		private readonly int VOICE = 10;
		private TextToSpeech speaker;
		private string toSpeak;
		private Intent _voiceIntent;
		private readonly RangeNotifier _rangeNotifier;
		private List<Beacon> _beacons;
		private BeaconManager _beaconManager;
		AltBeaconOrg.BoundBeacon.Region _tagRegion, _emptyRegion;

		public QuestionView()
		{
			_rangeNotifier = new RangeNotifier();
			_beacons = new List<Beacon>();
		}

        protected override void OnCreate(Bundle bundle)
        {
			ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.QuestionView);

			_click = FindViewById<LinearLayout>(Resource.Id.clickLayout);
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
					isRecording = !isRecording;
					if (isRecording)
					{
						_voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now!");
						_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 20000);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
						_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.English);
						StartActivityForResult(_voiceIntent, VOICE);
					}
				};

			StartBeaconManager();

        }

		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
            var skypeButton = FindViewById<Button>(Resource.Id.skypeButton);
            var answerButton = FindViewById<Button>(Resource.Id.answerButton);


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
						(ViewModel as QuestionViewModel).Question = textInput;

					    if (textInput.Contains("Skype") && textInput.Contains("open") || textInput.Contains("bot"))
					    {
					        (ViewModel as QuestionViewModel).Question = textInput;

					        skypeButton.Visibility = ViewStates.Visible;

					        answerButton.Visibility = ViewStates.Gone;

					        Speak("Do you want to see our Skype bot?");
                        }
					    else
					    {
                            skypeButton.Visibility = ViewStates.Invisible;
                            answerButton.Visibility = ViewStates.Visible;

                            Speak("You want to search Stackoverflow for: " + textInput + "?");
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
        public override void OnBackPressed()
        {
            //speaker.Stop();
            base.OnBackPressed();
        }

		private void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			var allBeacons = new List<Beacon>();
			if (e.Beacons.Count > 0)
			{
				foreach (var b in e.Beacons)
				{
					allBeacons.Add(b);
				}

				_beacons = allBeacons.OrderBy(b => b.Distance).ToList();

				double distance = 9999999999d;

				foreach(var beacon in _beacons)
				{
					var uuid = beacon.Id1.ToString().ToUpper();
					var major = beacon.Id2.ToString().ToUpper();
					var minor = beacon.Id3.ToString().ToUpper();
					var distanceOfBeacon = beacon.Distance;
					if (distanceOfBeacon < distance)
					{
						distance = distanceOfBeacon;
						if(minor == "1" && major == "1" && uuid == "EBEFD083-70A2-47C8-9837-E7B5634DF524") // CSharp
						{
							BeaconStats.ClosestArea = EArea.CSharp;
							if (distance < 3)
								BeaconStats.ProximityToClosestArea = EProximity.OnTop;
							else if(distance >= 3 && distance < 10)
								BeaconStats.ProximityToClosestArea = EProximity.Close;
							else if (distance >= 10 && distance < 20)
								BeaconStats.ProximityToClosestArea = EProximity.Medium;
							else if (distance >= 20 && distance < 50)
								BeaconStats.ProximityToClosestArea = EProximity.Far;
							else
								BeaconStats.ProximityToClosestArea = EProximity.Unknown;
						}
						else // Java
						{
							BeaconStats.ClosestArea = EArea.Java;
							if (distance < 3)
								BeaconStats.ProximityToClosestArea = EProximity.OnTop;
							else if (distance >= 3 && distance < 10)
								BeaconStats.ProximityToClosestArea = EProximity.Close;
							else if (distance >= 10 && distance < 20)
								BeaconStats.ProximityToClosestArea = EProximity.Medium;
							else if (distance >= 20 && distance < 50)
								BeaconStats.ProximityToClosestArea = EProximity.Far;
							else
								BeaconStats.ProximityToClosestArea = EProximity.Unknown;
						}
					}
				}
			}
		}

		private void StartBeaconManager()
		{
			_beaconManager = BeaconManager.GetInstanceForApplication(this);
			var iBeaconParser = new BeaconParser();
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			_beaconManager.BeaconParsers.Add(iBeaconParser);
			_beaconManager.Bind(this);
			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

			if (_beaconManager.IsBound(this))
			{
				_beaconManager.SetBackgroundMode(false);
			}
		}

		void IBeaconConsumer.OnBeaconServiceConnect()
		{
			_beaconManager.SetForegroundBetweenScanPeriod(1000); 

			_beaconManager.SetRangeNotifier(_rangeNotifier);

			_tagRegion = new AltBeaconOrg.BoundBeacon.Region("Id", Identifier.Parse("EBEFD083-70A2-47C8-9837-E7B5634DF524"), null, null);

			_beaconManager.StartRangingBeaconsInRegion(_tagRegion);
		}
	}
}