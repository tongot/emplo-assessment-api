using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using oldMutual.viewModels;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using oldMutual.Models;

namespace oldMutual.Controllers
{
   // [Authorize(Roles ="Admin")]
    public class DepartmentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Departments
        [Route("api/Department")]
        public pagerViewModel GetDepartments(int pageNumber,string searchValue, string category)
        {
            //department declaration
            IEnumerable<Department> deb=null;
            double itemCount;
            int perPage = 8;
            pagerViewModel pg = new pagerViewModel();
            //take care of search values
            if (searchValue != null)
            {
                if (category == "departmentName")
                {
                    List<Department> lsDep = db.Departments.Where(x => x.departmentName.Contains(searchValue)).ToList();
                    deb = lsDep.OrderBy(x => x.departmentName).Skip(perPage * (pageNumber - 1)).Take(perPage).ToList();
                    itemCount = lsDep.Count();
                }
                if (category == "location")
                {
                    List<Department> lsDep = db.Departments.Where(x => x.location.Contains(searchValue)).ToList();
                    deb = lsDep.OrderBy(x => x.location).Skip(perPage * (pageNumber - 1)).Take(perPage).ToList();
                    itemCount = lsDep.Count();
                }
            }
            else
            {
                deb = db.Departments.OrderBy(x => x.departmentName).Skip(perPage * (pageNumber - 1)).Take(perPage).ToList();
                itemCount = db.Departments.Count();
            }
            pg.obj = deb.ToList();
            itemCount = db.Departments.Count();
            pg.numberOfPages = (int)Math.Ceiling(itemCount / perPage);

            return pg;
        }
        [Route("api/DepartmentsList")]
        public IQueryable getDep()
        {
            return db.Departments;
        }
        // GET: api/Departments/5
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> GetDepartment(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> PostDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> DeleteDepartment(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            await db.SaveChangesAsync();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}