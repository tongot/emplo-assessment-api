
using oldMutual.Models;
using System.Collections.Generic;

namespace oldMutual.viewModels
{
    public class articleQuestionViewModel
    {
        public Article article { get; set; }
        public List<warmUpQuestions> questions { get; set; }
        

    }
}