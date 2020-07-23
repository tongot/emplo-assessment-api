using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class employeeRecruiteViewModel
    {
        public string employeeId { get; set; }
        public int courseId { get; set; }
        public string  Fullname { get; set; }
        public string Department { get; set; }
        public bool Checked { get; set; }
    }
}