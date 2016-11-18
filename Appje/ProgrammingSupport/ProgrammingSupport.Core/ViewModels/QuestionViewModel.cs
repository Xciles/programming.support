using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace ProgrammingSupport.Core.ViewModels
{
    public class QuestionViewModel : MvxViewModel
    {
		private string _question = string.Empty;

		public QuestionViewModel()
		{
			
		}

        public string Question
        { 
            get { return _question; }
            set { SetProperty (ref _question, value); }
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
    }
}
