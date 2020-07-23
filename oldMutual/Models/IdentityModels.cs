using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;

namespace oldMutual.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Employee : IdentityUser
    {
        //add other fileds here for your owne customisation
        public string name { get; set; }
        public string surname { get; set; }
        public string employeeNumber { get; set; }
        public string gender { get; set; }

        public int departmentId { get; set; }

        /// <summary>
        /// artcle relationships
        /// </summary>
        public ICollection<Article> articles { get; set; }
        /// <summary>
        /// employees comments
        /// </summary>
        public ICollection<Comment> comments { get; set; }
        public ICollection<Report> reports { get; set; }
        public ICollection<courseToEmployee> courses { get; set; }

        /// <summary>
        /// department relationship one to many
        /// </summary>
        public virtual Department department { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Employee> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
             
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<oldMutual.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.Article> Articles { get; set; }
        public System.Data.Entity.DbSet<oldMutual.Models.File> Files{ get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.warmUpQuestions> warmUpQuestions { get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.Test> Tests { get; set; }
        public System.Data.Entity.DbSet<oldMutual.Models.testToQuestions> testToQuestions { get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.courseToTest> courseToTest { get; set; }
        public System.Data.Entity.DbSet<oldMutual.Models.courseToArticles> courseToArticles { get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.Report> Reports { get; set; }


        public System.Data.Entity.DbSet<oldMutual.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.Answer> Answers { get; set; }
        public System.Data.Entity.DbSet<oldMutual.Models.Recruite> Recruites { get; set; }
        public System.Data.Entity.DbSet<oldMutual.Models.courseToEmployee> courseToEmployee { get; set; }
        public System.Data.Entity.DbSet<oldMutual.Models.commentReply> commentReply { get; set; }

        public System.Data.Entity.DbSet<oldMutual.Models.Comment> comment { get; set; }




    }
}