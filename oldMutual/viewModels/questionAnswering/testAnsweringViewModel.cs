using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels.questionAnswering
{
    /// <summary>
    /// holds the whole test questions
    /// </summary>
    public class testAnsweringViewModel
    {
        public int testId { get; set; }
        public double minMark { get; set; }
        public int attempts { get; set; }
        public bool negetiveMarking { get; set; }
        public int time { get; set; }
        public int courseId { get; set; }
        public List<questionAsweringViewModel> questions { get; set; }
    }
}