using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class ArticleViewModel
    {
        public List<Article>Articles { get; set; }
        public int  numberOfArticles { get; set; }
    }
}