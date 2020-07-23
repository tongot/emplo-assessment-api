using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        [Required]
        public string answer { get; set; }

        [Required]
        public double score { get; set; }

        public bool correct { get; set; }
        public int warmUpQuestionsId { get; set; }
        public virtual warmUpQuestions question { get; set; }

    }
}