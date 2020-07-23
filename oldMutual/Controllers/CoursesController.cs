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
    public class CoursesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Courses
        public pagerViewModel GetCourses(int pageNumber,string searchValue, string category)
        {
            List<courseViewModel> cs = new List<courseViewModel>();
            pagerViewModel pg = new pagerViewModel();
            IEnumerable<Course> course=null;
            double itemCount=0;
            int perPage = 8;

            if (searchValue != null)
            {
                if (category == "courseName")
                {
                    List<Course> lsCou = db.Courses.Where(x => x.name.Contains(searchValue)).ToList();
                    course = lsCou.OrderBy(x => x.name).Skip(perPage * (pageNumber - 1)).Take(perPage).ToList();
                    itemCount = lsCou.Count();
                }
                if (category == "createdBy")
                {
                    List<Course> lsCou = db.Courses.Where(x => x.createdBy.Contains(searchValue)).ToList();
                    course = lsCou.OrderBy(x => x.createdBy).Skip(perPage * (pageNumber - 1)).Take(perPage).ToList();
                    itemCount = lsCou.Count();
                }
            }
            else
            {
                course = db.Courses.OrderBy(x => x.name).Skip(perPage * (pageNumber - 1)).Take(perPage).ToList();
                itemCount = db.Courses.Count();
            }
            
            

            foreach (var item in course)
            {
                courseViewModel c = new courseViewModel();
                c.CourseId = item.CourseId;
                c.name = item.name;
                c.dateCreated = item.createdOn.ToShortDateString();
                c.discription = item.description;
                c.expiryDate = item.expireryDate.ToShortDateString();
                c.creater = item.createdBy;

                cs.Add(c);
            }

            pg.numberOfPages = (int)Math.Ceiling(itemCount / perPage);
            pg.obj = cs.ToList();
            return pg;
        }
        //get courses for user logged in
        [HttpGet]
        [Route("api/getCourseForUser")]
        public List<courseViewModel> GetCourse()
        {
            List<courseViewModel> cs = new List<courseViewModel>();
            //coursesList clst = new coursesList();
            //clst.courses = new List<courseViewModel>();
            string employeeId = db.Users.Where(x => x.UserName == User.Identity.Name).Select(x => x.Id).FirstOrDefault();

            foreach (var item in db.courseToEmployee.Where(x=>x.Id==employeeId))
            {
                courseViewModel c = new courseViewModel();
                c.CourseId = item.CourseId;
                c.name = item.courses.name;
                c.dateCreated = item.courses.createdOn.ToShortDateString();
                c.discription = item.courses.description;
                c.expiryDate = item.courses.expireryDate.ToShortDateString();
                c.creater = item.courses.createdBy;

                cs.Add(c);
            }

            return cs;
        }

        // GET: api/Courses/5
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> GetCourse(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet]
        [Route("api/getTestForCouse/{id}")]
        public List<courseToTestViewModel> getTestForCouse(int id)
        {
            List<courseToTestViewModel> ctt = new List<courseToTestViewModel>();

            var cousTosTes = db.courseToTest.Where(x => x.CourseId == id);

            foreach (var item in db.Tests)
            {
                courseToTestViewModel test = new courseToTestViewModel();
                foreach (var item2 in cousTosTes)
                {
                    if(item.TestId==item2.TestId)
                    {
                        test.Checked = true;
                    }
                }
                test.name = item.name;
                test.TestId = item.TestId;
                test.numberOfQuestions = db.testToQuestions.Where(x => x.TestId == item.TestId).Count();

                ctt.Add(test);
            }

            return ctt;
        }

        [HttpGet]
        [Route("api/getArtclesForCouse/{id}")]
        public List<courseToArticleViewModel> getArtclesForCouse(int id)
        {
            List<courseToArticleViewModel> courseToArt = new List<courseToArticleViewModel>();

            var cousTosArtcle = db.courseToArticles.Where(x => x.CourseId == id);

            foreach (var item in db.Articles)
            {
                courseToArticleViewModel courseToArt1= new courseToArticleViewModel();
                foreach (var item2 in cousTosArtcle)
                {
                    if (item.ArticleId == item2.ArticleId)
                    {
                        courseToArt1.Checked = true;
                    }
                }
                courseToArt1.name = item.title;
                courseToArt1.ArticleId = item.ArticleId;

                courseToArt.Add(courseToArt1);
            }

            return courseToArt;
        }


        [Route("api/TestC")]
        public async Task<IHttpActionResult> TestC(testForCourses test)
        {
            //delete all associates
            foreach (var item in db.courseToTest.Where(x=>x.CourseId==test.courseId))
            {
                db.Entry(item).State = EntityState.Deleted;
            }
            //save new change
            foreach (var item in test.Tests)
            {
                if(item.Checked)
                {
                    courseToTest ct = new courseToTest();
                    ct.CourseId = test.courseId;
                    ct.TestId = item.TestId;
                    db.courseToTest.Add(ct);
                }
            }

            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        [Route("api/ArticleC")]
        public async Task<IHttpActionResult> ArticleC(listOfArt article)
        {
            //delete all associates
            foreach (var item in db.courseToArticles.Where(x => x.CourseId == article.courseId))
            {
                db.Entry(item).State = EntityState.Deleted;
            }
            //save new change
            foreach (var item in article.Articles)
            {
                if (item.Checked)
                {
                    courseToArticles art = new courseToArticles();
                    art.CourseId = article.courseId;
                    art.ArticleId = item.ArticleId;
                    db.courseToArticles.Add(art);
                }
            }

            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Courses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseId)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> PostCourse(courseViewModel course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //date convertion
            DateTime t = DateTime.ParseExact(course.expiryDate, "dd-MM-yyyy",null);

            Course cs = new Course();
            cs.name = course.name;
            cs.description = course.discription;
            cs.createdBy = User.Identity.Name;
            cs.createdOn = DateTime.Now;
            cs.DepartmentId = db.Users.Where(x => x.UserName == User.Identity.Name).Select(x => x.departmentId).FirstOrDefault();
            cs.expireryDate = t;
            //validate date for expiry {must be greater than today}
            if (cs.expireryDate<DateTime.Now)
            {
                return null;
            }
            db.Courses.Add(cs);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }
            

            course.CourseId = cs.CourseId;
            course.duration = cs.expireryDate-cs.createdOn;
            course.dateCreated = cs.createdOn.ToShortDateString();
            course.creater = cs.createdBy;

            return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> DeleteCourse(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            await db.SaveChangesAsync();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}