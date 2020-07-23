using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using oldMutual.Models;
using System.IO;
using System.Web;
using System.Web.Http.Cors;
using oldMutual.viewModels;

namespace oldMutual.Controllers
{
    [Authorize]
    public class ArticlesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/Articles

        [HttpGet]
        [Route("api/Articlesd/{currentPage}")]
        [Authorize(Roles ="Admin")]
        public pagerViewModel GetArticles(int currentPage,string searchValue,string category)
        {
            ArticleViewModel vm = new ArticleViewModel();
            double itemCount=0;
            int perPage = 8;
            pagerViewModel pg = new pagerViewModel();

            
            //take care of search values
            if (searchValue != null)
            {
                if (category == "compiledBy")
                {
                    List<Article> lsArt = db.Articles.Where(x => x.articleBy.Contains(searchValue)).ToList();
                    vm.Articles = lsArt.OrderBy(x => x.dateAdded).Skip(perPage * (currentPage - 1)).Take(perPage).ToList();
                    itemCount = lsArt.Count();
                }
                if (category == "title")
                {
                    List<Article> lsArt = db.Articles.Where(x => x.title.Contains(searchValue)).ToList();
                    vm.Articles=lsArt.OrderBy(x => x.dateAdded).Skip(perPage * (currentPage - 1)).Take(perPage).ToList();
                    itemCount = lsArt.Count();
                }
            }
            else
            {
                vm.Articles = db.Articles.OrderBy(x => x.dateAdded).Skip(perPage * (currentPage - 1)).Take(perPage).ToList();
                itemCount = db.Articles.Count();
            }
            pg.numberOfPages = (int)Math.Ceiling(itemCount / perPage);
            pg.obj = vm.Articles;
            return pg;
        }



        // GET: api/Articles/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> GetArticle(int id)
        {
            Article article = await db.Articles.FindAsync(id);
            List<string> names = new List<string>();
            if (article == null)
            {
                return NotFound();
            }
            //add filename that are on article already
            foreach (var filename in getFileNames())
            {
                if (article.article.Contains(filename))
                {
                    names.Add(filename);
                }
            }
            if(names.Count>0)
            {
                article.filenames = names;
            }
            return Ok(article);
        }

        // PUT: api/Articles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArticle(int id, articlesEditViewModel article)
        {
            if (string.IsNullOrEmpty(article.article))
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(article.title))
            {
                return BadRequest(ModelState);
            }

            if (id != article.ArticleId)
            {
                return BadRequest();
            }
            //check if there are delete images
            if (article.filenames != null)
            {
                deleteFilesEdited(article, null);
            }
           

            Article art = db.Articles.Find(id);
            art.article = article.article;
            art.title = article.title;

            try
                {
                    db.Entry(art).State = EntityState.Modified;
                }
                catch (Exception ex)
                {

                    throw;
                }
            

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/Publish/{id}")]
        public async Task<IHttpActionResult> Publish(int id, int publish)
        {
            Article acticle = db.Articles.Find(id);
            acticle.publish = publish;
            db.Entry(acticle).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Articles
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> PostArticle(Article article)
        {
            //List<string> names = new List<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check if ther were files that the user attempted to add before 
            if (article.filenames != null)
            {
                deleteFilesEdited(null,article);
            }
            //get the details of the user adding the article
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            article.dateAdded = DateTime.Now;
            article.articleBy = user.name + " " + user.surname;

            db.Articles.Add(article);

           

            await db.SaveChangesAsync();

            Models.File f = new Models.File();
            f.ArticleId = article.ArticleId;

            return CreatedAtRoute("DefaultApi", new { id = article.ArticleId }, article);
        }

        // DELETE: api/Articles/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> DeleteArticle(int id)
        {
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            //check if the article has any files 
            deleteAttchments(article);

            db.Articles.Remove(article);
            await db.SaveChangesAsync();

            return Ok(article);
        }

        [ResponseType(typeof(Uri))]
        [Route("api/Articles/imageUpload")]
        [HttpPost]
        public async Task<IHttpActionResult> imageUpload()
        {
            string host = "http://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port + "/api/Articles/getImages/";

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            //string filePath = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartMemoryStreamProvider();
            string Url = "";
            try
            {

                await Request.Content.ReadAsMultipartAsync(provider);
                string filename = "";
                foreach (HttpContent file in provider.Contents)
                {

                    //string filename = (file.Headers.ContentDisposition.FileName.Trim('\"'));
                    filename = string.Format(@"{0}", Guid.NewGuid());

                    byte[] buffer = file.ReadAsByteArrayAsync().Result;
                    string filePath = HttpContext.Current.Server.MapPath("~/App_Data/images/" + filename);
                    using (var sw = new FileStream(filePath, FileMode.Create))
                    {
                        sw.Write(buffer, 0, buffer.Length);
                    }
                    Url = host + filename;
                }


                return Ok(Url);
            }
            catch (Exception a)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Articles/getImages/{name}")]
        public async Task<HttpResponseMessage> getImages(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/images/" + name);
            try
            {

                byte[] imagebytdata = System.IO.File.ReadAllBytes(filePath);
                MemoryStream ms = new MemoryStream(imagebytdata);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                return response;

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }
        //_____________________________________________________________________________________helpers fo rthe controller____________________________________

        /// <summary>
        /// method to get all the file names of images in server
        /// </summary>
        /// <returns>return a list of names in server</returns>
        public List<string> getFileNames()
        {
            List<string> fileNames = new List<string>();
            DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/App_Data/images/"));
            FileInfo[] Files = d.GetFiles();
            if (Files.Count() > 0)
            {
                foreach (FileInfo file in Files)
                {
                    fileNames.Add(file.Name);
                }
            }

            return fileNames;
        }
        //______________________________________________________________________________course and articles________________________________________________________
        [HttpGet]
        [Route("api/ArticlesForCourse/{id}")]
        public List<Article> ArticlesForCourse(int id)
        {
            List<courseToArticles> art = new List<courseToArticles>();
            List<Article> articles = new List<Article>();
            art = db.courseToArticles.Where(x=>x.CourseId==id).ToList();
            foreach (var item in art)
            {
                if (item.article.publish == 1)
                {
                    Article at = new Article();
                    at.title = item.article.title;
                    at.article = item.article.article;
                    at.ArticleId = item.article.ArticleId;

                    articles.Add(at);
                }
               
            }
            return articles;
        }

        /// <summary>
        /// delete waste attchment that were delete by the user during compilation of article
        /// </summary>
        /// <param name="article">the articl to clean the attachment</param>
        public void deleteAttchments(Article article)
        {
            foreach (string name in getFileNames())
            {
                if (article.article.Contains(name))
                {
                    //delete assciaeted files if any
                    string filePath = HttpContext.Current.Server.MapPath("~/App_Data/images/" + name);
                    System.IO.File.Delete(filePath);
                }
            }
        }
        /// <summary>
        /// delete all removed imaged tthat were removed during edit
        /// </summary>
        /// <param name="article">the edited article</param>

        private void deleteFilesEdited(articlesEditViewModel article, Article art2)
        {
            if (article == null)
            {
                try
                {
                    foreach (string item in art2.filenames)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            bool hasImage = art2.article.Contains(item);
                            if (!hasImage)
                            {
                                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/images/" + item);
                                System.IO.File.Delete(filePath);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                try
                {
                    foreach (var item in article.filenames)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            bool hasImage = article.article.Contains(item);
                            if (!hasImage)
                            {
                                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/images/" + item);
                                System.IO.File.Delete(filePath);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        //________________________________________________________________________________________________________________________________________________________
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Count(e => e.ArticleId == id) > 0;
        }
    }
}