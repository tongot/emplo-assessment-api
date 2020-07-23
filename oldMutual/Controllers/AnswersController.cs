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
using oldMutual.viewModels;

namespace oldMutual.Controllers
{
    public class AnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Answers
        public IQueryable<Answer> GetAnswers()
        {
            return db.Answers;
        }

        // GET: api/Answers/5
        [ResponseType(typeof(Answer))]
        public async Task<IHttpActionResult> GetAnswer(int id)
        {
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }
        [HttpGet]
        [Route("api/answerFor/{id}")]
        public async Task<IHttpActionResult> getAnswer(int id)
        {
            List<Answer> answers = new List<Answer>();
            var ans =  db.Answers.Where(x=>x.warmUpQuestionsId==id).ToList();
            if (answers == null)
            {
                return NotFound();
            }
            foreach (var item in ans)
            {
                Answer answ = new Answer();
                answ.correct = item.correct;
                answ.answer = item.answer;
                answ.warmUpQuestionsId = item.warmUpQuestionsId;
                answ.score = item.score;

                answers.Add(answ);
            }

            return Ok(answers);
        }

        // PUT: api/Answers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnswer(int id, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != answer.AnswerId)
            {
                return BadRequest();
            }

            db.Entry(answer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answers
        [ResponseType(typeof(Answer))]
        public async Task<IHttpActionResult> PostAnswer(answersViewModel answers)
        {
            var AnswerIndatabase= db.Answers.Where(x=>x.warmUpQuestionsId==answers.questionId);
            //check if the model has data
            if(AnswerIndatabase!=null)
            {
                foreach (var item in AnswerIndatabase)
                {
                   db.Answers.Remove(item);
                }
            }
            //save available items
            try
            {

                foreach (var item in answers.answers)
                {
                    db.Answers.Add(item);
                 }

                 await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi",null,new { id=answers.questionId});
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        // DELETE: api/Answers/5
        [ResponseType(typeof(Answer))]
        public async Task<IHttpActionResult> DeleteAnswer(int id)
        {
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            db.Answers.Remove(answer);
            await db.SaveChangesAsync();

            return Ok(answer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(int id)
        {
            return db.Answers.Count(e => e.AnswerId == id) > 0;
        }
    }
}