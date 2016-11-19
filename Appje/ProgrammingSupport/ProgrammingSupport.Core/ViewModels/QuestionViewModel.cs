using MvvmCross.Core.ViewModels;
using ProgrammingSupport.Core.Business;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgrammingSupport.Core.ViewModels
{
    public class QuestionViewModel : MvxViewModel
    {
		private string _question = string.Empty;
		private string _answer = string.Empty;

        public EActionType NextAction { get; set; }

		public QuestionViewModel()
		{
			
		}

        public System.Action<string> AnswerUpdated;

        public void Init(string question)
        {
            Question = question;
        }

        public void InvokeAnswerUpdated(string answer)
        {
            var handler = AnswerUpdated;
            if (handler != null)
            {
                handler.Invoke(answer);
            }
        }

        public string Question
        {
            get { return _question; }
            set
            {
                SetProperty(ref _question, value);

                if (_question?.Length > 4)
                    GetAnswer(_question).ConfigureAwait(false);
            }
        }

        public string Answer
        {
            get { return _answer; }
            set
            {
                SetProperty(ref _answer, value);
                InvokeAnswerUpdated(Answer);
            }
        }

        public async Task GetAnswer(string question)
        {
            if (BeaconStats.ProximityToClosestArea == EProximity.OnTop || BeaconStats.ProximityToClosestArea == EProximity.Close || BeaconStats.ProximityToClosestArea == EProximity.Medium)
                question = question + " " + BeaconStats.ClosestArea.ToString();
            Answer = await StackAnswerer.AnswerMe(question);
        }

        public ICommand GoToAnswerCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<AnswerViewModel>(new { answer = Answer });
                });
            }
        }

        public ICommand GoToSkypeCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<SkypeViewModel>();
                });
            }
        }

        public ICommand GoToKyleCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<FirstViewModel>();
                });
            }
        }
    }

    public enum EActionType
    {
        Answer = 0,
        Skype = 1,
    }
}
