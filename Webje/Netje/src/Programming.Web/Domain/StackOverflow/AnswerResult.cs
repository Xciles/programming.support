using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Web.Domain.StackOverflow
{
    public class AnswerResult
    {
        public Answer[] items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }

    public class Answer
    {
        public Owner owner { get; set; }
        public bool is_accepted { get; set; }
        public int score { get; set; }
        public int last_activity_date { get; set; }
        public int last_edit_date { get; set; }
        public int creation_date { get; set; }
        public int answer_id { get; set; }
        public int question_id { get; set; }
        public string body { get; set; }
    }
}
