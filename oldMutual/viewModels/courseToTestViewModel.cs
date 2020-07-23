using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    
    public class courseToTestViewModel
    {
        public int TestId { get; set; }
        public string name { get; set; }
        public int numberOfQuestions { get; set; }
        public bool Checked { get; set; }
        public double mark { get; set; }
        public int attempts { get; set; }
    }
    public class testForCourses
    {
        public int courseId { get; set; }
        
        public string courseName { get; set; }
        public List<courseToTestViewModel> Tests { get; set; }
        public double complition { get; set; }
        public double complitionCalc(int id,string reportFor)
        {
             ApplicationDbContext db = new ApplicationDbContext();
            //get number of test for course
            double Numberoftest = db.courseToTest.Where(x => x.CourseId == id).Count();
            //get the number of test on each course
            double testPassed = db.Reports.Where(x => x.ReportFor == reportFor & x.CourseId == id & x.pass == true).Count();

            double progress=Numberoftest!=0?((testPassed / Numberoftest) * 100):0;
            return progress;
         }
    }
}