using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels.questionAnswering
{
    public class reportUpdate
    {
         public int ReportId { get; set; }
         public int CourseId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public int NumberOfFailed { get; set; }
        public string answeringBehaviour { get; set; }
        public bool pass { get; set; }
    }
}