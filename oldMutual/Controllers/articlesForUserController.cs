using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace oldMutual.Controllers
{
    public class articlesForUserController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/userArticle")]
        public List<Article> userArticle()
        {
            List<Article> articles = new List<Article>();
            articles = db.Articles.Where(x => x.publish == 1).ToList();
            return articles;

        }

        [HttpGet]
        [Route("api/getArticleToRead/{id}")]
        public Article getArticleToRead(int id)
        {
            Article article;
            article = db.Articles.Find(id);
            return article;
        }

    }
}
