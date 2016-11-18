using ProgrammingSupport.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace ProgrammingSupport.Core.ViewModels
{
    public class AnswerViewModel : MvxViewModel
    {
        private string _question;
        private string _answer;

		public void Init(string question)
		{
			Question = question;
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
            set { SetProperty(ref _answer, value); }
        }

        public async Task  GetAnswer(string question)
        {
            Answer = await StackAnswerer.AnswerMe(question);
        }
    }
}
