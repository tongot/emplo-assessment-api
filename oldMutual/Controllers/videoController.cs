using oldMutual.Models;
using oldMutual.viewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace oldMutual.Controllers
{
    public class videoController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("api/videoPlayer/{id}")]
        public HttpResponseMessage getVideoContent(int id)
        {
            // Action<Stream, HttpContent, TransportContext, string> action = new Action<Stream, HttpContent, TransportContext, string>(WriteContentToSteam);
            Models.File f = db.Files.Find(id);

            string filename = f.fileName;
            var httpResponse = Request.CreateResponse();
            videoViewModel content = new videoViewModel(filename);

            httpResponse.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)content.WriteContentToSteam);
            return httpResponse;
        }

        [HttpPost]
        [Route("api/video/videoUpload")]
        public async Task<IHttpActionResult> videoUpload()
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
                string filename = "";

                int id = Convert.ToInt32(provider.Contents[1].Headers.ContentDisposition.Name.Trim(removeChar));
                filename = provider.Contents[0].Headers.ContentDisposition.FileName.Trim(removeChar);


                byte[] buffer = provider.Contents[0].ReadAsByteArrayAsync().Result;

                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/videos/" + filename);
                using (var sw = new FileStream(filePath, FileMode.Create))
                {
                    await sw.WriteAsync(buffer, 0, buffer.Length);
                }
                //create file model

                Models.File file = new Models.File();
                file.ArticleId = id;
                file.fileName = filename.Trim('"');
                file.filePath = filePath;
                file.fileExt = "video";

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
    }

}
