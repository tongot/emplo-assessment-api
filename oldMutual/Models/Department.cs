
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace oldMutual.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        public string departmentName { get; set; }
        [Required]
        public string location { get; set; }

        public ICollection<Employee> employees { get; set; }
        public ICollection<Test> tests { get; set; }

        [NotMapped]
        public int pageNumber { get; set; }
       
    }
}