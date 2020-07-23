
namespace oldMutual.Models
{
    public class testToQuestions
    {
        public int testToQuestionsId { get; set; }
        public int warmUpQuestionsId { get; set; }
        public int TestId { get; set; }

        public virtual warmUpQuestions question { get; set; }
        public virtual Test test { get; set; }
    }
}