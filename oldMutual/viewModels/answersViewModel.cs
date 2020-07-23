using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class answersViewModel
    {
        public int questionId { get; set; }
        public List<Answer>  answers { get; set; }
    }
}