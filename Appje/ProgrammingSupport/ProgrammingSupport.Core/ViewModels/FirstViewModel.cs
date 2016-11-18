using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace ProgrammingSupport.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private string _hello = "Hello MvvmCross";
        public string Hello
        { 
            get { return _hello; }
            set { SetProperty (ref _hello, value); }
        }

        public ICommand GoToAnswerCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<AnswerViewModel>();
                });
            }
        }
    }
}
