
namespace oldMutual.Models
{
    public class commentReply
    {
        public int commentReplyId { get; set; }
        public string  reply { get; set; }
        public string replier { get; set; }
        public int likes { get; set; }
        public int dislikes { get; set; }

        public int commentId { get; set; }
        public virtual Comment comment { get; set; }
    }
}