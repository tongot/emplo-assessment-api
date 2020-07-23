using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    /// <summary>
    /// this for presenting questions to the view to select for course allocation
    /// </summary>
    public class testViewModel
    {
        public List<Test> tests { get; set; }
        public int numberOfItems { get; set; }
    }
}