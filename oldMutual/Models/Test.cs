using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace oldMutual.Models
{
    public class Test
    {
        public int TestId { get; set; }
        [Required]
        public string name{ get; set; }
        public string SetBy {get; set;}

        public int time { get; set; }

        public double maxmumMark { get; set; }

        [Range(0,100)]
        public double minimumPassMark { get; set; }
        public int attempts{ get; set; }

        public bool negetiveMarking { get; set; }
        public string dateCreated { get; set; }
        /// <summary>
        /// determine if the test is to be taken
        /// </summary>
        public bool ask { get; set; }
        public ICollection<courseToTest> courseToTest { get; set; }
        public ICollection<testToQuestions> testToQuestions { get; set; }

        [NotMapped]
        public string oldTestName { get; set; }
        


    }
}