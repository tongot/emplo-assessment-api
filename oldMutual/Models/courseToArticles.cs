

namespace oldMutual.Models
{
    public class courseToArticles
    {
        public int courseToArticlesId { get; set; }
        public int ArticleId { get; set; }
        public int CourseId { get; set; }

        public virtual Article article { get; set; }
        public virtual Course course { get; set; }
    }
}