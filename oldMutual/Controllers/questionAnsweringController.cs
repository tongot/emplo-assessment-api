using oldMutual.Models;
using oldMutual.viewModels;
using oldMutual.viewModels.questionAnswering;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace oldMutual.Controllers
{
    /// <summary>
    /// question answering controller 
    /// only for answering 
    /// </summary>
    public class questionAnsweringController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// get all the questions for test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getQuestionsToanswer/{testId}")]
        public testAnsweringViewModel getQuestionsToanswer(int testId) {
            Test testDetail;
            try
            {
                 testDetail = db.Tests.Find(testId);
            }
            catch (Exception)
            {
                return null;
            }
            testAnsweringViewModel test = new testAnsweringViewModel();
            test.questions = new List<questionAsweringViewModel>();

            //get the questions of the test given
            foreach (var item in db.testToQuestions.Where(x=>x.TestId==testId))
            {
                questionAsweringViewModel qn = new questionAsweringViewModel();
                qn.QuestionId = item.warmUpQuestionsId;
                qn.Question = item.question.question;
                qn.answers = new List<answerViewModel>();
                //get the answer for question
                foreach (var  answer in db.Answers.Where(x=>x.warmUpQuestionsId==item.warmUpQuestionsId))
                {
                    //create object answer 
                    answerViewModel ans = new answerViewModel();
                    ans.answer = answer.answer;
                    ans.AnswerId = answer.AnswerId;
                    ans.correct = answer.correct;
                    ans.selected = false;
                    ans.Check = "square";
                    ans.score = answer.score;
                    qn.answers.Add(ans);

                }
                test.questions.Add(qn);
            }
            test.testId = testId;




            test.time = testDetail.time;
            test.attempts = testDetail.attempts;
            test.minMark = testDetail.minimumPassMark;
            test.negetiveMarking = testDetail.negetiveMarking;
            return test;

        }
        /// <summary>
        /// set the report for the first time to record an attempt
        /// report initialisaion
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/setReport",Name ="SetReportOnes")]
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> setReport(Report report)
        {

            string user = User.Identity.Name;
            var repo = db.Reports.Where(x => x.ReportFor == user & x.TestId == report.TestId & x.CourseId == report.CourseId);

            report.attempts = 1;
            if (repo.Count()>0)
            {
                Report rpt = repo.FirstOrDefault();
                
                if (repo.Select(x => x.attempts).FirstOrDefault()+1 > (db.Tests.Find(report.TestId).attempts))
                {
                    report.attempts=0;
                    report.ReportId = rpt.ReportId;
                    //no attempts left (0 attempts )
                    return CreatedAtRoute("SetReportOnes", new { id = rpt.ReportId }, report);
                }
                if (rpt.pass == true)
                {
                    report.attempts =-1 ;
                    return CreatedAtRoute("SetReportOnes", new { id = rpt.ReportId }, report);
                }
                rpt.attempts += 1;
                report.ReportId = rpt.ReportId;
                db.Entry(rpt).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //already made an attempt
                return CreatedAtRoute("SetReportOnes", new { id = rpt.ReportId }, report);
            }
            // first time to take the test
            if (!ModelState.IsValid)            {
                return BadRequest(ModelState);
            }
            report.ReportFor = user;
            report.attempts = 1;
            report.dateTaken = DateTime.Now;
            db.Reports.Add(report);
            await db.SaveChangesAsync();

            return CreatedAtRoute("SetReportOnes", new { id = report.ReportId }, report);
        }

        /// <summary>
        /// get the result of the candidate taking the test  
        /// </summary>
        /// <param name="id">the id of the effected report</param>
        /// <param name="report">report detailes</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/report/{id}")]
        public async Task<IHttpActionResult> PutCourse(int id, reportUpdate report)
        {
            //get the user name 
            string name = User.Identity.Name;
            Report rp = db.Reports.Find(report.ReportId);


            if (id != report.ReportId)
            {
                return BadRequest();
            }
            //assining the values from the front end 
            rp.Score = report.Score;
            rp.NumberOfFailed = report.NumberOfFailed;
            rp.answeringBehaviour = report.answeringBehaviour;
            rp.pass = report.pass;


            //get the number of tests per course 
            double numberOfTestPerCouse=db.courseToTest.Where(x => x.CourseId == report.CourseId).Count();

            //get the number of test passed
            double numberofTestPassed = db.Reports.Where(x => x.ReportFor == name & x.CourseId == report.CourseId & x.pass == true).Count();
            numberofTestPassed += rp.pass==true?1:0;
            //get the date of the report 
            rp.dateTaken = DateTime.Now;

            //calculate test completion stage in percentage 
            rp.courseComplitionStage = ((numberofTestPassed / numberOfTestPerCouse) * 100);
            //modify in database

            db.Entry(rp).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
