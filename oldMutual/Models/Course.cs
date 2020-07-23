using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oldMutual.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]

        public string description { get; set; }

        public string createdBy { get; set; }

        public DateTime createdOn { get; set; }
        [Required]

        public DateTime expireryDate { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department department { get; set; }
        /// <summary>
        /// the number of days course is valid
        /// </summary>
        public string daysOfValidity { get {
                return expireryDate.Subtract(createdOn).ToString();
            } }
        public ICollection<courseToTest> courseToTest { get; set; }
        public ICollection<courseToArticles> courseToArticles { get; set; }
        public ICollection<courseToEmployee> courseToEmployee { get; set; }

    }
}