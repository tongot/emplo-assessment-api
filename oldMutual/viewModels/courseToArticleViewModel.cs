using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class courseToArticleViewModel
    {
            public int ArticleId { get; set; }
            public string name { get; set; }
            public bool Checked { get; set; }
       
    }
    public class listOfArt
        {
            public int courseId { get; set; }
            public List<courseToArticleViewModel> Articles { get; set; }
        }
}