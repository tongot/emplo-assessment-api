using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class pagerViewModel
    {
        public IEnumerable<object> obj { get; set; }
        public int numberOfPages { get; set; }
    }
}