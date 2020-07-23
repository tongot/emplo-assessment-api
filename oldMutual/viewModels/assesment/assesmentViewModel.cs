using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class assesmentViewModel
    {
        public userViewModel user { get; set; }
        public List<testForCourses> courses { get; set; }
    }
}