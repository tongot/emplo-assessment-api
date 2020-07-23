using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public string comment { get; set; }
        /// <summary>
        /// the one who commented
        /// </summary>
        public string commentedBy { get; set; }
        public int likes { get; set; }
        public int dislikes { get; set; }
        public DateTime dateCommented { get; set; }
        /// <summary>
        /// the article commented
        /// </summary>
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}