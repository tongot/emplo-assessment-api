using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class courseViewModel
    {

        public int CourseId { get; set; }
        [Required]
        public string expiryDate { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string discription { get; set; }
        public string dateCreated { get; set; }

        public string creater { get; set; }

        public  TimeSpan duration{ get; set; }
    
    }
    public class coursesList
    {
        public List<courseViewModel> courses { get; set; }
    }
}