using oldMutual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace oldMutual.Controllers
{
    public class recruitsController : ApiController
    { 
        private ApplicationDbContext db = new ApplicationDbContext();

        //public async Task<IHttpActionResult> recruiteEmployee(string username, Recruite recruite)
        //{

        //    Employee emp = db.Users.Where(x => x.UserName == username).FirstOrDefault();
        //    if (emp == null)
        //    {
        //        return BadRequest();
        //    }
            
        //}
    }
}
