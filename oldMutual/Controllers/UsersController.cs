using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using oldMutual.Models;
using oldMutual.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace oldMutual.Controllers
{
    public class UsersController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: api/Users/2
        //get all the users to display 
        [HttpGet]
        [Route("api/user/{currentPage}")]
        public usersViewModel useres(int currentPage)
        {
            usersViewModel uv = new usersViewModel();
            uv.users = new List<userViewModel>();
            int perPage = 8;
            if (currentPage == -1)
            {
                uv.numberOfEmployees = db.Users.Count();
                currentPage += 2;
            }
            List<Employee> employees = db.Users.OrderBy(x => x.name).Skip(perPage * (currentPage - 1)).Take(perPage).ToList();
            foreach (var item in employees)
            {
                userViewModel user = new userViewModel();
                user.userId = item.Id;
                user.email = item.Email;
                user.employeeNumber = item.employeeNumber;
                user.name = item.name;
                user.surname = item.surname;
                user.department = item.department.departmentName;

                uv.users.Add(user);
            }

            return uv;
        }

        
        [HttpGet]
        [Route("api/roles/{userId}")]
        public async Task<List<viewModelRoles>> GetRoles(string userId)
        {
           
            //get all users from roles table
            var allRoles =  db.Roles.Select(x => x.Name).ToList();
            List<viewModelRoles> roles = new List<viewModelRoles>();

            //get the roles allocated to the selected user 
            var userRoles =await UserManager.GetRolesAsync(userId);

            //check if the user has any assigned roles
            foreach (var item in allRoles)
            {
                viewModelRoles r = new viewModelRoles();
                r.roleName = item;
                foreach (var item2 in userRoles)
                {
                    if (item == item2)
                    {
                        r.isChecked = true;
                    }
                }
                roles.Add(r);
            }

            return roles;
        }

        // POST: api/addRole
        [Route("api/addRoles")]
        public async Task<IHttpActionResult> Post(userRoleViewModel user)
        {
            List<string> removeRole = new List<string>();
            string[] addRole = user.userRoles.Where(x => x.isChecked == true).Select(x=>x.roleName).ToArray();


            foreach (var item in user.userRoles.Select(x => x.roleName).ToArray())
            {
                if (UserManager.IsInRole(user.userId, item))
                {
                    removeRole.Add(item);
                }
            }

            try
            {      
                   IdentityResult result = await UserManager.RemoveFromRolesAsync(user.userId, removeRole.ToArray());
                   IdentityResult result2 =UserManager.AddToRoles(user.userId,addRole);
 
            }catch(Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }


         /// <summary>
         /// search the users to recruite for a course 
         /// </summary>
         /// <param name="id">the id of course selected</param>
         /// <param name="category">the category to search with </param>
         /// <param name="search">the seach string provided by the uuser </param>
         /// <returns></returns>
        [HttpGet]
        [Route("api/getEmployeeSearch/{id}")]
        public List<employeeRecruiteViewModel> getSearch(int id,string category, string search)
        {
            List<Employee> employees = new List<Employee>();
            if (category == "name")
            {
                 employees = db.Users.Where(x => x.name.Contains(search) || x.surname.Contains(search)).ToList();
            }
            if (category == "department")
            {
                 employees = db.Users.Where(x => x.department.departmentName.Contains(search)).ToList();
            }
            //set the values of employee to the view model
            List<employeeRecruiteViewModel> employeeReruite = new List<employeeRecruiteViewModel>();
            if (employees.Count() > 0)
            {
                foreach (var employee in employees)
                {
                    employeeRecruiteViewModel empl = new employeeRecruiteViewModel();
                    empl.employeeId = employee.Id;
                    empl.Fullname = employee.name + " " + employee.surname;
                    empl.Department = employee.department.departmentName;
                    empl.Checked = db.courseToEmployee.Where(x => x.CourseId == id & x.Id == employee.Id).Count() > 0;

                    employeeReruite.Add(empl);
                }
            }
            else
            {
                return null;
            }
            return employeeReruite;
        }

        /// <summary>
        /// the mothods to post the data for course to employeed 
        /// </summary>
        /// <param name="id">the id of the course</param>
        /// <param name="employees">the empoyees to be effected</param>
        /// 
        [HttpPost]
        [Route("api/uploadRecruites/{id}")]
        public async Task<IHttpActionResult> uploadRecruites(int id, employePostrecuite employees)
        {

            //first delete all the people assosiated
            var employeCourse = db.courseToEmployee.Where(x => x.CourseId == id);

            foreach (var employee in employeCourse)
            {
                db.Entry(employee).State = System.Data.Entity.EntityState.Deleted;
            }

            //add all the people assosiated
            foreach (var item in employees.employees)
            {
                if (item.Checked)
                {
                    courseToEmployee c = new courseToEmployee();
                    c.CourseId = id;
                    c.Id = item.employeeId;
                    db.courseToEmployee.Add(c);
                }
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
        }
    }
}
