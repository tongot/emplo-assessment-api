using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class MinReport
    {
        public int MinReportId { get; set; }
        public float mark{ get; set; }

        public DateTime dateTaken { get; set; }
        public int TestId { get; set; }
        public DateTime dateLastUpdated { get; set; }
        public string reportFor { get; set; }

        public virtual Test test { get; set; }

    }
}