using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class userViewModel
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string  surname { get; set; }
        public string email { get; set; }
        public string employeeNumber { get; set; }
        public string department { get; set; }
        public bool selected { get; set; }
    }
}