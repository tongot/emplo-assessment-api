

using System.ComponentModel.DataAnnotations.Schema;

namespace oldMutual.Models
{
    public class File
    {
        public int FileId { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }

        public string fileExt { get; set; }
        public int ArticleId { get; set; }
        /// <summary>
        /// the article associated with this file
        /// </summary>
        public virtual Article article { get; set; }

        [NotMapped]

        public float size { get; set; }
        [NotMapped]
        public string name { get; set; }
        [NotMapped]
        public string type { get; set; }
        [NotMapped]
        public string downloadLink { get; set; }

    }
}