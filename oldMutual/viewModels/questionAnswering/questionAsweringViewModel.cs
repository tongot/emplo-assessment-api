using oldMutual.Models;
using oldMutual.viewModels.questionAnswering;
using System.Collections.Generic;

namespace oldMutual.viewModels
{
    /// <summary>
    /// this model is for handling the list of questions to present to the user
    /// </summary>
    public class questionAsweringViewModel
    {
        /// <summary>
        /// the id of question to be answered
        /// </summary>
        public int QuestionId  { get; set; }
        public string Question { get; set; }

        /// <summary>
        /// the list of answers
        /// </summary>
        public List<answerViewModel> answers { get; set; }
    }
}