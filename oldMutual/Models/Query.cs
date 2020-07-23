using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Query
    {

        public int QueryId { get; set; }
        [Required]
        public string query { get; set; }
        public int articleId { get; set; }
        public string askedBy { get; set; }
        public virtual Article article  { get; set; }

        public ICollection<answersToQueries> answers { get; set; }
    }
}