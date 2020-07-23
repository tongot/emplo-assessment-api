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
using System.Web;
using System.IO;
using oldMutual.viewModels;

namespace oldMutual.Controllers
{
    public class FilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Files
        public IQueryable<Models.File> GetFiles()
        {
            return db.Files;
        }

        // GET: api/Files/5
        [ResponseType(typeof(Models.File))]
        public async Task<IHttpActionResult> GetFile(int id)
        {
            Models.File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // PUT: api/Files/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFile(int id, Models.File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != file.FileId)
            {
                return BadRequest();
            }

            db.Entry(file).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
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

        // POST: api/Files
        [ResponseType(typeof(Models.File))]
        public async Task<IHttpActionResult> PostFile(Models.File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Files.Add(file);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = file.FileId }, file);
        }

        // DELETE: api/Files/5
        [ResponseType(typeof(Models.File))]
        public async Task<IHttpActionResult> DeleteFile(int id)
        {
            Models.File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            db.Files.Remove(file);
            await db.SaveChangesAsync();

            return Ok(file);
        }
        //recieve files
        [HttpPost]
        [Route("api/Files/fileUpload")]
        public async Task<IHttpActionResult> fileUpload()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                char[] removeChar = { '/', '\\', '"' };

                await Request.Content.ReadAsMultipartAsync(provider);
                string filenameOnServer, fileOrgName = "";
                int id = Convert.ToInt32(provider.Contents[1].Headers.ContentDisposition.Name.Trim(removeChar));
                fileOrgName = provider.Contents[0].Headers.ContentDisposition.FileName;


                 filenameOnServer = string.Format(@"{0}", Guid.NewGuid());

                 byte[] buffer = provider.Contents[0].ReadAsByteArrayAsync().Result;

                 string filePath = HttpContext.Current.Server.MapPath("~/App_Data/documents/" + filenameOnServer);
                 using (var sw = new FileStream(filePath, FileMode.Create))
                 {
                     await sw.WriteAsync(buffer, 0, buffer.Length);
                 }
                 //create file model

                Models.File file = new Models.File();
                file.ArticleId = id;
                file.fileName = fileOrgName.Trim('"');
                file.filePath = filePath;
                file.fileExt = "document";

                db.Files.Add(file);
                //save to the database
                await db.SaveChangesAsync();

                return Ok(file);
            }
            catch (Exception a)
            {
                throw;
            }
        }
        //download file
        [HttpGet]
        [Route("api/Files/downloads/{id}")]
        public async Task<HttpResponseMessage> downloadFile(int? id)
            {
            if(id!=null)
            {
                Models.File file = await db.Files.FindAsync(id.Value);
                    
                 string path= file.filePath;
                    if(System.IO.File.Exists(path))
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        var fileStream = new FileStream(path, FileMode.Open);
                        response.Content = new StreamContent(fileStream);
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers
                            .ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = file.fileName;
                        return response;
                    }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("api/Filesd/{id}")]
        public filesViewModel Files(int? id)
        {
            if(id==null)
            { return null; }
            filesViewModel f = new filesViewModel();
            f.files = new List<Models.File>();

            var files =db.Files.Where(x => x.ArticleId == id).ToList();
            foreach (var item in files)
            {
                Models.File fs = new Models.File();
                fs.fileName = item.fileName;
                fs.FileId = item.FileId;
                fs.fileExt = item.fileExt;
                fs.downloadLink = "";
                f.files.Add(fs);
            }
            return f;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FileExists(int id)
        {
            return db.Files.Count(e => e.FileId == id) > 0;
        }
    }
}