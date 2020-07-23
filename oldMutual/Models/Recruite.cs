using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Recruite
    {
        public int RecruiteId { get; set; }

        public  string recruitedBy { get; set; }
        public int CourseId { get; set; }

        public int EmployeeId { get; set; }
        public DateTime dateRecruited { get; set; }
        public Employee Employee { get; set; }
        public Course course { get; set; }

    }
}