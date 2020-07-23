

namespace oldMutual.Models
{
    public class courseToTest
    {
        public int courseToTestId { get; set; }
        public int TestId { get; set; }
        public int CourseId { get; set; }

        public virtual Course course { get; set; }
        public virtual Test test { get; set; }

    }
}