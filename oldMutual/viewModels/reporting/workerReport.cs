using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels.reporting
{
    public class workerReport
    {
        public int reportId { get; set; }

        public string testName { get; set; }
        public int attempts { get; set; }
        public int attemptsLeft { get; set; }
        public double score { get; set; }
        public string  dateOfLastAttempt { get; set; }
        public string courseName { get; set; }
        public bool pass { get; set; }
    }
}