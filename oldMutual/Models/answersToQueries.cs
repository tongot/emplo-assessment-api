

using System.ComponentModel.DataAnnotations;

namespace oldMutual.Models
{
    public class answersToQueries
    {
        public int answersToQueriesId { get; set; }
        [Required]
        public string answer { get; set; }

        /// <summary>
        /// answerd by (the employeeId)
        /// </summary>

        public int queryId { get; set; }
        public string answerBy { get; set; }
        public virtual Query query { get; set; }
    }
}