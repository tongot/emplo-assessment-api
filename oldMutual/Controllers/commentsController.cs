using oldMutual.Models;
using oldMutual.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace oldMutual.Controllers
{
    public class commentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Answers
        /// <summary>
        /// get comment of an article  
        /// </summary>
        /// <param name="ArtcleId">the id of the article to pull comments for</param>
        /// <returns></returns>
        // GET: api/Comments/5
        [Route("api/comments/{ArtcleId}")]
        [ResponseType(typeof(Comment))]
        public List<Comment> GetComment(int ArtcleId)
        {
            List<Comment> comments = db.comment.Where(x => x.ArticleId == ArtcleId).ToList();
            return comments;
        }

        /// <summary>
        /// post a commet for an article 
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> PostComment(Comment comment)
        {
            comment.commentedBy = User.Identity.Name;
            comment.dateCommented = DateTime.Now;

            db.comment.Add(comment);

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = comment.CommentId }, comment);
        }

        /// <summary>
        /// post a reply to a comment on an article
        /// </summary>
        /// <param name="reply">the content to save to the database</param>
        /// <returns></returns>
        [Route("api/comment/reply")]
        public async Task<IHttpActionResult> CommentReply(commentReplyViewmodel reply)
        {
            Comment comment = await db.comment.FindAsync(reply.commentId);
            commentReply rep = new commentReply();
                rep.replier = User.Identity.Name;
            rep.commentId = reply.commentId;
            rep.reply = reply.reply;

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rep.commentReplyId },rep);
        }
    }
}
