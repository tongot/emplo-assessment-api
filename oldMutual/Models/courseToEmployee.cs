using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class courseToEmployee
    {
        [Key]
        public int courseToEmployeeId { get; set; }
        public int CourseId { get; set; }
        public string Id { get; set; }

        public virtual Course courses { get; set; }
        public virtual Employee employee {get;set; }
    }
}