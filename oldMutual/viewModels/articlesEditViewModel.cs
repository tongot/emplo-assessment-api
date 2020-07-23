using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class articlesEditViewModel
    {
        public int ArticleId { get; set; }
        [Required]
        public string title { get; set; }
        [Required]

        public string article { get; set; }
        /// <summary>
        /// the employee who uploaded the article
        /// </summary>
        /// 
        public string articleBy { get; set; }

        /// <summary>
        /// qeuries associated with the article
        /// </summary>
        public ICollection<Query> queries { get; set; }
        public ICollection<File> files { get; set; }

        /// <summary>
        /// associated questions to articles
        /// </summary>
        public ICollection<warmUpQuestions> warmUpQuestions { get; set; }

        [NotMapped]
        
        public List<string> filenames
        {
            get; set;
        }
    }
    public class fileN
    {
         public string filename { get; set; }
    }
}