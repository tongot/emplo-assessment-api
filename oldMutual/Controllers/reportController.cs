using oldMutual.Models;
using oldMutual.viewModels;
using oldMutual.viewModels.reporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace oldMutual.Controllers
{
    public class reportController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [Route("api/reportListWorker")]
        public List<workerReport> reportListWorker()
        {
            string username = User.Identity.Name;
            List<workerReport> workerReport = new List<workerReport>();
            //get the report for the logged in user 
            var report = db.Reports.Where(X => X.ReportFor == username).ToList();
            if(report.Count()>0)
            {
                foreach (var item in report)
                {
                    workerReport wr = new workerReport();

                    wr.pass = item.pass;    
                    wr.testName = db.Tests.Find(item.TestId).name;
                    wr.attempts = item.attempts;
                    wr.attemptsLeft =db.Tests.Find(item.TestId).attempts - wr.attempts;
                    wr.courseName = db.Courses.Find(item.CourseId).name;
                    wr.dateOfLastAttempt = item.dateTaken.ToShortDateString();
                    wr.reportId = item.ReportId;
                    wr.score = item.Score;

                    workerReport.Add(wr);
                }
            }
            
            return workerReport;
        }
        /// <summary>
        /// individual list of progress on the courses they are recruited in 
        /// </summary>
        /// <returns></returns>
        [Route("api/asses/{numberOfItem}")]
        public pagerViewModel getEmployeesAssesment(int numberOfItem, string searchValue,int pageNumber)
        {
            List<assesmentViewModel> asvms = new List<assesmentViewModel>();
            pagerViewModel pg = new pagerViewModel();
            try
            {
                var user = searchValue != null ? db.Users.Where(x => x.surname.Contains(searchValue)).OrderBy(x => x.surname)
                    .Skip(numberOfItem * (pageNumber - 1)).Take(numberOfItem) : db.Users.OrderBy(x => x.surname)
                    .Skip(numberOfItem * (pageNumber - 1)).Take(numberOfItem);

                //get the count of the detailes to be posted
                double numberOfRecords = searchValue != null ? db.Users.Where(x => x.surname.Contains(searchValue)).Count():
                    db.Users.Count();
                pg.numberOfPages = (int)Math.Ceiling(numberOfRecords / numberOfItem);
                //get result for all the users that are in the database 
            
            if (user != null)
            {
                foreach (var item in user)
                {
                    assesmentViewModel asvm = new assesmentViewModel();
                    //set new user  
                    asvm.user = new userViewModel();
                    //set cousrse for the user  
                    asvm.courses = new List<testForCourses>();
                    //user details
                    asvm.user.name = item.name;
                    asvm.user.surname = item.surname;
                    asvm.user.email = item.Email;

                    //assign courses in which user is recruited 
                    var courses = db.courseToEmployee.Where(x => x.Id == item.Id);
                    foreach (var course in courses)
                    {
                        //get courses of the user
                        testForCourses tc = new testForCourses();
                        tc.courseId = course.CourseId;
                        tc.courseName = course.courses.name;
                        tc.complition = tc.complitionCalc(course.CourseId, item.UserName);
                        //set test associated with the coursen selected 
                        tc.Tests = new List<courseToTestViewModel>();
                        var tests = db.courseToTest.Where(x => x.CourseId == course.CourseId);
                        foreach (var test in tests)
                        {
                            //get test one by 1 
                            courseToTestViewModel ctv = new courseToTestViewModel();
                            var testReport = db.Reports.Where(x => x.TestId == test.TestId&x.CourseId==course.CourseId
                            &x.ReportFor==item.UserName).FirstOrDefault();
                            ctv.name = test.test.name;
                            ctv.TestId = test.TestId;
                            ctv.attempts = testReport != null ? testReport.attempts : 0;
                            ctv.mark = testReport != null ? testReport.Score : 0;
                            //set test
                            tc.Tests.Add(ctv);
                        }
                        //set course
                        asvm.courses.Add(tc);
                    }
                    //set report
                    asvms.Add(asvm);
                }
            }
                pg.obj = asvms;
                return pg;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get the values of the schedule requested 
        /// </summary>
        /// <param name="search">search values for the schedule to be pulled</param>
        /// <returns></returns>
         //[Route("api/markSchedule")]
        //public pagerViewModel getMarkSchedule(searchScheduleViewModel search)
        //{
            
        //}
    }
}
