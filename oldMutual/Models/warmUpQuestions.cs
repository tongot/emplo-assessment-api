

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace oldMutual.Models
{
    public class warmUpQuestions
    {
        public int warmUpQuestionsId { get; set; }

        [Required]
        public string question { get; set; }
        public int articleId { get; set; }
        public virtual Article article { get; set; }

        public string Addedby { get; set; }

        public ICollection<testToQuestions> testToQuestions { get; set; }
        public ICollection<Answer> answers { get; set; }

        public List<Report> reports { get; set; }
    }
}