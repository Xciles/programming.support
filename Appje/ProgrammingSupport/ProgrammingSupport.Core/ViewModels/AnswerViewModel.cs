using ProgrammingSupport.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace ProgrammingSupport.Core.ViewModels
{
    public class AnswerViewModel : MvxViewModel
    {
        private string _question;
        private string _answer;

        public Action AnswerUpdated;

		public void Init(string answer, string question)
		{
			Answer = answer;
			Question = _question;
		    InvokeAnswerUpdated();
		}

        public void InvokeAnswerUpdated()
        {
            var handler = AnswerUpdated;
            if (handler != null)
            {
                handler.Invoke();
            }
        }

        public string Question
        {
            get { return _question; }
            set
            {
                SetProperty(ref _question, value);
            }
        }

        public string Answer
        {
            get { return _answer; }
            set
            {
                SetProperty(ref _answer, value);
                InvokeAnswerUpdated();
            }
        }

		public async Task GetAnswer(string question)
		{
			if (BeaconStats.ProximityToClosestArea == EProximity.OnTop || BeaconStats.ProximityToClosestArea == EProximity.Close || BeaconStats.ProximityToClosestArea == EProximity.Medium)
				question = question + " " + BeaconStats.ClosestArea.ToString();
            Answer = await StackAnswerer.AnswerMe(question);
        }

        public ICommand GoToPizzaCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<PizzaViewModel>(new { question = Question });
                });
            }
        }

    }
}
