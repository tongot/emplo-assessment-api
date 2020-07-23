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
    public class TestsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Tests
        [Route("api/Test/{pageNumber}")]
        public pagerViewModel GetTests(int pageNumber)
        {
            testViewModel ts = new testViewModel();
            ts.tests = new List<Test>();
            double itemCount;

            int takefrom = (pageNumber - 1) * 8;
            ts.tests = db.Tests.OrderBy(x => x.dateCreated).Skip(takefrom).Take(8).ToList();

            pagerViewModel pg = new pagerViewModel();
            pg.obj = ts.tests.ToList();
            itemCount = db.Tests.Count();
            pg.numberOfPages = (int)Math.Ceiling(itemCount / 8);

            return pg;
        }
        //GET: api/articleQuestion
        [HttpGet]
        [Route("api/articleQuestion/{currentPage}")]
        public async Task<articleQuestionListViewModel> getArticleQuestions(int currentPage)
        {
            articleQuestionListViewModel articleQuestionListViewModel = new articleQuestionListViewModel();
            articleQuestionListViewModel.listOfArticles = new List<Article>();


            //number of items per page
            int perPage = 6;
            if (currentPage == -1)
            {
                articleQuestionListViewModel.numberOfArticles = await db.Articles.CountAsync();
                currentPage += 2;
            }
            //get articles to load
            var articles = await db.Articles.OrderBy(x => x.dateAdded).ToListAsync();

            // loop thru to get each articles questions
            foreach (var article in articles)
            {
                Article art = new Article();
                art.article = article.article;
                art.articleBy = article.articleBy;
                art.ArticleId = article.ArticleId;
                art.dateAdded = article.dateAdded;
                art.title = article.title;
                art.warmUpQuestions = new List<warmUpQuestions>();
                var questions = await db.warmUpQuestions.Where(x => x.articleId == article.ArticleId).ToListAsync();

                //loop to assign the questions assosiated with the question
                foreach (var qn in questions)
                {
                    warmUpQuestions wrm = new warmUpQuestions();
                    wrm.Addedby = qn.Addedby;
                    wrm.question = qn.question;
                    wrm.warmUpQuestionsId = qn.warmUpQuestionsId;
                    art.warmUpQuestions.Add(wrm);
                }

                articleQuestionListViewModel.listOfArticles.Add(art);
            }


            return articleQuestionListViewModel;
        }
        // GET: api/Tests/5
        [ResponseType(typeof(Test))]
        public async Task<IHttpActionResult> GetTest(int id)
        {
            Test test = await db.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        // PUT: api/Tests/5
        [HttpPut]
        [Route("api/testEdit/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTest(int id, testToeditViewModel test)
        {
            if (test.test.name != test.test.oldTestName)
            {
                if(TestExists(test.test.name))
                {
                    return BadRequest("0");
                }
            }
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != test.test.TestId)
            {
                return BadRequest();
            }
            //delete old questions
            foreach (var item in db.testToQuestions.Where(x => x.TestId == test.test.TestId))
            {
                db.Entry(item).State = EntityState.Deleted;
            }
            //add new questions
            foreach (var item in test.QnAssociate)
            {
                testToQuestions tq = new testToQuestions(); tq.warmUpQuestionsId =
                  item.warmUpQuestionsId;
                tq.TestId = id;
                db.testToQuestions.Add(tq);
            }

            test.test.dateCreated = DateTime.Now.ToShortDateString();
            db.Entry(test.test).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
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

        // POST: api/Tests
        [ResponseType(typeof(Test))]
        public async Task<IHttpActionResult> PostTest(Test test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (TestExists(test.name))
            {
                //if the test already exist return 0
                return BadRequest("0");
            }
            test.SetBy = User.Identity.Name;
            test.dateCreated = DateTime.Now.ToShortDateString();
            db.Tests.Add(test);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = test.TestId }, test);
        }

        [Route("api/testQuestion")]
        public async Task<IHttpActionResult> postTestQuestion(testQuestionViewModel tstQn)
        {
            try
            {
                List<testToQuestions> tq = new List<testToQuestions>();
                if (tstQn != null)
                {
                    if (tstQn.questionId.Count() > 0)
                    {
                        foreach (var item in tstQn.questionId)
                        {
                            testToQuestions t = new testToQuestions();
                            t.TestId = tstQn.testId;
                            t.warmUpQuestionsId = item;

                            db.testToQuestions.Add(t);
                        }
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    return Content(HttpStatusCode.NotAcceptable, "Cannot add without any questions assined");
                }
                return Content(HttpStatusCode.NotAcceptable, "We did not recieve any recode");

            }
            catch (Exception e)
            {
                return Ok();
            }

        }

        // DELETE: api/Tests/5
        [ResponseType(typeof(Test))]
        public async Task<IHttpActionResult> DeleteTest(int id)
        {
            Test test = await db.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            db.Tests.Remove(test);
            await db.SaveChangesAsync();

            return Ok(test);
        }
        //___________________________________________________________________________get test to edit________________________________________________________
        /// <summary>
        /// for getting questions assosieted with a test
        /// </summary>
        /// <param name="id">test id to get data for </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/testToedit/{id}")]
        public async Task<testToeditViewModel> testToedit(int id)
        {
            Test test = await db.Tests.FindAsync(id);
            if (test == null)
            {
                return null;
            }
            //define new insances
            testToeditViewModel testToedit = new testToeditViewModel();
            testToedit.QnAssociate = new List<warmUpQuestions>();

            //assign values for test
            testToedit.test = new Test();
            testToedit.test.TestId = test.TestId;
            testToedit.test.time = test.time;
            testToedit.test.name = test.name;
            testToedit.test.oldTestName = test.name;
            testToedit.test.attempts = test.attempts;
            testToedit.test.minimumPassMark = test.minimumPassMark;
            testToedit.test.negetiveMarking = test.negetiveMarking;
            

            //get the quastions already assosiated with this test
            var QnAssociate = db.warmUpQuestions.OrderBy(x => x.warmUpQuestionsId).Where(x => x.testToQuestions.Where(c => c.TestId == id).Count() > 0).ToList();

            foreach (var item in QnAssociate)
            {
                warmUpQuestions wr = new warmUpQuestions();
                wr.Addedby = item.Addedby;
                wr.articleId = item.articleId;
                wr.question = item.question;
                wr.warmUpQuestionsId = item.warmUpQuestionsId;

                testToedit.QnAssociate.Add(wr);
            }

            return testToedit;
        }

        /// <summary>
        /// get the questions not assosieted to the given test id
        /// </summary>
        /// <param name="id">the test id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/OtherQuestions/{id}/{data}")]
        public List<warmUpQuestions> QuestionsNotTotest(int id, int data)
        {
            List<warmUpQuestions> questions = new List<warmUpQuestions>();
            //get all questions not assosiated with this test
            var OtherQuestions = db.warmUpQuestions.OrderBy(x => x.warmUpQuestionsId).Where(x => x.testToQuestions.Where(c => c.TestId == id).Count() == 0).Skip(data).Take(10).ToList();

            foreach (var item in OtherQuestions)
            {
                warmUpQuestions wr = new warmUpQuestions();
                wr.Addedby = item.Addedby;
                wr.articleId = item.articleId;
                wr.question = item.question;
                wr.warmUpQuestionsId = item.warmUpQuestionsId;

                questions.Add(wr);
            }
            return questions;

        }

        /// <summary>
        /// get the information on test for the course selected
        /// </summary>
        /// <param name="id"> course id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getTestForCourse/{id}")]
        public List<Test> getTestForCourse(int id)
        {
            string user = User.Identity.Name;
            List<Test> tests = new List<Test>();
            foreach (var item in db.courseToTest.Where(x=>x.CourseId==id))
            {
                int attemptByUser = db.Reports.Where(x => x.CourseId == id & x.TestId == item.TestId & x.ReportFor == user).Select(x=>x.attempts).FirstOrDefault();


                Test test = new Test();
                test.TestId = item.TestId;
                test.time = item.test.time;
                test.name = item.test.name;
                test.dateCreated = item.test.dateCreated;
                test.attempts = (item.test.attempts- attemptByUser);
                test.minimumPassMark = item.test.minimumPassMark;
                test.negetiveMarking = item.test.negetiveMarking;

                tests.Add(test);
            }
            return tests;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TestExists(string testName)
        {
            return db.Tests.Count(e => e.name.Trim() == testName.Trim()) > 0;
        }
        private bool TestExists(int id)
        {
            return db.Tests.Count(e => e.TestId == id) > 0;
        }
    }
}