using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int CourseId { get; set; }
        public string ReportFor { get; set; }
        public int TestId { get; set; }
        public float Score { get; set; }
        public DateTime dateTaken  { get; set; }

        public int? NumberOfFailed { get; set; }
        public string questionsFailed { get; set; }
        public string  answeringBehaviour { get; set; }
        public double courseComplitionStage { get; set; }
        public int attempts { get; set; }
        public bool pass { get; set; }

        [NotMapped]
        public int attemptsLeft { get; set; }

    }
}