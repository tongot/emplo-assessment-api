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
using oldMutual.viewModels;

namespace oldMutual.Controllers
{
    public class warmUpQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/warmUpQuestions
        public IQueryable<warmUpQuestions> GetwarmUpQuestions()
        {
            return db.warmUpQuestions;
        }

        // GET: api/warmUpQuestions/5
        [ResponseType(typeof(warmUpQuestions))]
        public async Task<IHttpActionResult> GetwarmUpQuestions(int id)
        {
            warmUpQuestions warmUpQuestions = await db.warmUpQuestions.FindAsync(id);
            if (warmUpQuestions == null)
            {
                return NotFound();
            }

            return Ok(warmUpQuestions);
        }
        // GET: api/warmUpQuestions/ArticleQn/5
        [Route("api/warmUpQuestions/ArticleQn/{id}")]
        public  viewModelQuestions GetQuestionByArticleid(int id)
        {
            viewModelQuestions q = new viewModelQuestions();
            q.questions = new List<warmUpQuestions>();

            IQueryable<warmUpQuestions> qns=  db.warmUpQuestions.Where(x => x.articleId == id);

            foreach (var item in qns)
            {
                warmUpQuestions wq = new warmUpQuestions();
                wq.Addedby = item.Addedby;
                wq.question = item.question;
                wq.warmUpQuestionsId = item.warmUpQuestionsId;
                wq.articleId = item.articleId;

                q.questions.Add(wq);
            }

            return q;
        }

        // PUT: api/warmUpQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutwarmUpQuestions(int id, warmUpQuestions warmUpQuestions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != warmUpQuestions.warmUpQuestionsId)
            {
                return BadRequest();
            }

            db.Entry(warmUpQuestions).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!warmUpQuestionsExists(id))
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

        // POST: api/warmUpQuestions
        [ResponseType(typeof(warmUpQuestions))]
        public async Task<IHttpActionResult> PostwarmUpQuestions(warmUpQuestions warmUpQuestions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            warmUpQuestions.Addedby = User.Identity.Name;
            db.warmUpQuestions.Add(warmUpQuestions);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = warmUpQuestions.warmUpQuestionsId }, warmUpQuestions);
        }

        // DELETE: api/warmUpQuestions/5
        [ResponseType(typeof(warmUpQuestions))]
        public async Task<IHttpActionResult> DeletewarmUpQuestions(int id)
        {
            warmUpQuestions warmUpQuestions = await db.warmUpQuestions.FindAsync(id);
            if (warmUpQuestions == null)
            {
                return NotFound();
            }

            db.warmUpQuestions.Remove(warmUpQuestions);
            await db.SaveChangesAsync();

            return Ok(warmUpQuestions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool warmUpQuestionsExists(int id)
        {
            return db.warmUpQuestions.Count(e => e.warmUpQuestionsId == id) > 0;
        }
    }
}