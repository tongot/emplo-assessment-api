using oldMutual.Models;
using System.Collections.Generic;


namespace oldMutual.viewModels
{
    /// <summary>
    /// this displays test to edit data
    /// </summary>
    public class testToeditViewModel
    {
        /// <summary>
        /// carries the test attributes
        /// </summary>
        public Test test{ get; set; }

        /// <summary>
        /// carries all questions assosiatted with the question
        /// </summary>
        public List<warmUpQuestions> QnAssociate { get; set; }

    }
}