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
using System.Threading.Tasks;

namespace ProgrammingSupport.Droid.Views
{
    [Activity(Label = "View for QuestionViewModel", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class QuestionView : MvxActivity, TextToSpeech.IOnInitListener, IBeaconConsumer
    {
        private bool isRecording = false;
        private readonly int VOICE = 10;
        private TextToSpeech speaker;
        private string toSpeak;
        private Intent _voiceIntent;
        private readonly RangeNotifier _rangeNotifier;
        private List<Beacon> _beacons;
        private BeaconManager _beaconManager;
        AltBeaconOrg.BoundBeacon.Region _tagRegion, _emptyRegion;
        private double distanceClosestCSharp = 9999999d;
        private double distanceClosestJava = 9999999d;

        private WebView _webView;
        private RelativeLayout _click;
        private ImageView _text;

        private bool pizzaFlow = false;
        private bool skypeFlow = false;
        private bool stackOverflowFlow = false;

        private string _question;

        public QuestionView()
        {
            _rangeNotifier = new RangeNotifier();
            _beacons = new List<Beacon>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            ActionBar.Hide();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
            (ViewModel as QuestionViewModel).AnswerUpdated += OnAnswerUpdated;

            _webView = FindViewById<WebView>(Resource.Id.webview);
            _click = FindViewById<RelativeLayout>(Resource.Id.clickLayout);
            _text = FindViewById<ImageView>(Resource.Id.txtBubble);

            string fileName = "file:///android_asset/Content/southparkidle.gif";
            _webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<img style=\"width: 100%; height:100%; margin:0px; padding:0px;\" id=\"selector\" src=\"" + fileName + "\"></body></html>", "text/html", "utf-8", null);
            //_webView.LoadDataWithBaseURL(null, "<html><body style=\"width: 100%; height:100%; margin:0px; padding:0px;\">\n<img style=\"width: 100%; height:100%; margin:0px; padding:0px;\" id=\"selector\" src=\"https://media.giphy.com/media/1iUlZloL4DiH8HL2/giphy.gif\"></body></html>", "text/html", "utf-8", null);
            _webView.Settings.JavaScriptEnabled = true;
            _webView.SetBackgroundColor(Android.Graphics.Color.Transparent);


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

            //         ActionBar.Hide();
            //         base.OnCreate(bundle);
            //         SetContentView(Resource.Layout.QuestionView);

            //_click = FindViewById<LinearLayout>(Resource.Id.clickLayout);
            //string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            //if (rec != "android.hardware.microphone")
            //{
            //	var alert = new AlertDialog.Builder(this);
            //	alert.SetTitle("You don't seem to have a microphone to record with");
            //	alert.Show();
            //}
            //else
            //	_click.Click += delegate
            //	{
            //		isRecording = !isRecording;
            //		if (isRecording)
            //		{
            //			_voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now!");
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 20000);
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            //			_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.English);
            //			StartActivityForResult(_voiceIntent, VOICE);
            //		}
            //	};

            StartBeaconManager();

        }

        protected override async void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            //var skypeButton = FindViewById<Button>(Resource.Id.skypeButton);
            //var answerButton = FindViewById<Button>(Resource.Id.answerButton);
            //var kyleButton = FindViewById<Button>(Resource.Id.kyleButton);


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
                        var ditIsResult = textInput.ToLower();

                        if (stackOverflowFlow)
                        {
                            if (ditIsResult.Contains("yes"))
                            {
                                (ViewModel as QuestionViewModel).Question = _question;
                                stackOverflowFlow = false;
                            }
                            else if (ditIsResult.Contains("no"))
                            {
                                Speak("Stay ignorant, you hillbilly!");
                                pizzaFlow = false;
                                _text.SetImageResource(Resource.Drawable.txtPizzaNo);
                                _text.Visibility = ViewStates.Visible;
                                pizzaFlow = false;
                            }
                        }
                        else if (skypeFlow)
                        {
                            if (ditIsResult.Contains("yes"))
                            {
                                Speak("Opening Skype Bot for you.");
                                skypeFlow = false;
                                (ViewModel as FirstViewModel).GoToAnswerCommand.Execute(null);
                                skypeFlow = false;
                            }
                            else if (ditIsResult.Contains("no"))
                            {
                                Speak("Why did you ask then, you stupid asshole!");
                                skypeFlow = false;
                                _text.SetImageResource(Resource.Drawable.txtWhyAsk);
                                _text.Visibility = ViewStates.Visible;
                                skypeFlow = false;
                            }
                            else
                            {
                                Speak("Speak up, you mumbling idiot!");
                                _text.SetImageResource(Resource.Drawable.txtSpeakUp);
                                _text.Visibility = ViewStates.Visible;
                            }
                        }                        
                        else if (pizzaFlow)
                        {
                            if (ditIsResult.Contains("yes"))
                            {
                                Speak("I will get you a pepperoni pizza, buddy!");
                                pizzaFlow = false;
                                _text.SetImageResource(Resource.Drawable.txtPizzaYes);
                                _text.Visibility = ViewStates.Visible;
                                pizzaFlow = false;
                            }
                            else if (ditIsResult.Contains("no"))
                            {
                                Speak("More for me, you fat fuck!");
                                pizzaFlow = false;
                                _text.SetImageResource(Resource.Drawable.txtPizzaNo);
                                _text.Visibility = ViewStates.Visible;
                                pizzaFlow = false;
                            }
                            else
                            {
                                Speak("Speak up, you mumbling idiot!");
                                _text.SetImageResource(Resource.Drawable.txtSpeakUp);
                                _text.Visibility = ViewStates.Visible;
                            }
                        }
                        else if ((ditIsResult.Contains("Skype") && ditIsResult.Contains("open")) || (ditIsResult.Contains("Skype") && ditIsResult.Contains("bot")) || ditIsResult.Contains("Skype"))
                        {
                            //(ViewModel as QuestionViewModel).Question = textInput;
                            skypeFlow = true;
                            Speak("Do you want to see our Skype bot?");
                            _text.SetImageResource(Resource.Drawable.txtSkypeBot);
                            _text.Visibility = ViewStates.Visible;
                        }
                        else if ((ditIsResult.Contains("pizza")))
                        {
                            pizzaFlow = true;
                            Speak("Would you like a pizza?");
                            _text.SetImageResource(Resource.Drawable.txtPizza);
                            _text.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            Speak("You want to search Stackoverflow for: " + textInput + "?");
                            _question = textInput;
                            stackOverflowFlow = true;
                        }

                    }
                    else
                        Speak("No speech was recognised");
                    isRecording = false;
                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }

        private void OnAnswerUpdated(string answer)
        {
            if (answer == "NoResults!")
            {
                Speak("I am sorry, I could not find " + (ViewModel as QuestionViewModel).Question + ". Would you like a pizza?");
                _text.SetImageResource(Resource.Drawable.txtPizza);
                _text.Visibility = ViewStates.Visible;
                pizzaFlow = true;
            }
            else
            {
                (ViewModel as QuestionViewModel).GoToAnswerCommand.Execute(null);
            }
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
        public override void OnBackPressed()
        {
            speaker?.Stop();
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

                foreach (var beacon in _beacons)
                {
                    var uuid = beacon.Id1.ToString().ToUpper();
                    var major = beacon.Id2.ToString().ToUpper();
                    var minor = beacon.Id3.ToString().ToUpper();
                    var distanceOfBeacon = beacon.Distance;
                    if (distanceOfBeacon < distance)
                    {
                        distance = distanceOfBeacon;
                        if (minor == "1" && major == "1" && uuid == "EBEFD083-70A2-47C8-9837-E7B5634DF524") // CSharp
                        {
                            if (distance < 3)
                            {
                                BeaconStats.ClosestArea = EArea.CSharp;
                                BeaconStats.ProximityToClosestArea = EProximity.OnTop;
                            }
                            else if (distance >= 3 && distance < 10)
                            {
                                BeaconStats.ClosestArea = EArea.CSharp;
                                BeaconStats.ProximityToClosestArea = EProximity.Close;
                            }
                            else if (distance >= 10 && distance < 20)
                            {
                                BeaconStats.ClosestArea = EArea.CSharp;
                                BeaconStats.ProximityToClosestArea = EProximity.Medium;
                            }
                            else if (distance >= 20 && distance < 50)
                                BeaconStats.ProximityToClosestArea = EProximity.Far;
                            else
                                BeaconStats.ProximityToClosestArea = EProximity.Unknown;
                        }
                        else // Java
                        {
                            if (distance < 1)
                            {
                                BeaconStats.ClosestArea = EArea.Java;
                                BeaconStats.ProximityToClosestArea = EProximity.OnTop;
                            }
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

            //_tagRegion = new AltBeaconOrg.BoundBeacon.Region("Id", Identifier.Parse("EBEFD083-70A2-47C8-9837-E7B5634DF524"), null, null);
            _emptyRegion = new AltBeaconOrg.BoundBeacon.Region("Id", null, null, null);

            //_beaconManager.StartRangingBeaconsInRegion(_tagRegion);
            _beaconManager.StartRangingBeaconsInRegion(_emptyRegion);
        }
    }
}