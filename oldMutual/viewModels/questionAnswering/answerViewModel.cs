using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels.questionAnswering
{
    public class answerViewModel
    {
        public int AnswerId { get; set; }
        public string answer { get; set; }
        public double score { get; set; }
        public bool correct { get; set; }
        public bool selected { get; set; }
        public string Check { get; set; }

    }
}