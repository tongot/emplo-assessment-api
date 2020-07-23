using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        [Required]
        public string reply { get; set; }

        /// <summary>
        /// the commennt that has the reply
        /// </summary>
        public int commentId { get; set; }
        public DateTime dateReplied { get; set; }
        /// <summary>
        /// employee who replied
        /// </summary>
        public string repliedBy { get; set; }
        public virtual Comment comment { get; set; }

    }
}