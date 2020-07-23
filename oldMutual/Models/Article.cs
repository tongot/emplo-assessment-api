using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        [Required]
        public string title { get; set; }
        [Required]

        public string article { get; set; }
        public DateTime dateAdded { get; set; }
        /// <summary>
        /// the employee who uploaded the article
        /// </summary>
        /// 
        public string articleBy { get; set; }

        public int publish { get; set; }
        public int hits { get; set; }

        /// <summary>
        /// qeuries associated with the article
        /// </summary>
        public ICollection<Query> queries {get;set;}
        public ICollection<File> files { get; set; }

        /// <summary>
        /// associated questions to articles
        /// </summary>
        public ICollection<warmUpQuestions> warmUpQuestions { get; set; }
        public ICollection<courseToArticles> coursesToArticles { get; set; }

        [NotMapped]
        public List<string> filenames
        {
            get; set;
        }

    }
}