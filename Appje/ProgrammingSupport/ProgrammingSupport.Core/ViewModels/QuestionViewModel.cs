using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace ProgrammingSupport.Core.ViewModels
{
    public class QuestionViewModel : MvxViewModel
    {
		private string _question = string.Empty;

        public EActionType NextAction { get; set; }

		public QuestionViewModel()
		{
			
		}

        public string Question
        { 
            get { return _question; }
            set
            {
                SetProperty (ref _question, value);               
            }
        }

        public ICommand GoToAnswerCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
					ShowViewModel<AnswerViewModel>(new { question = Question});
                });
            }
        }

        public ICommand GoToSkypeCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<SkypeViewModel>(new { question = Question });
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
