using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DW.EnterpriseAPI.Entity
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; }
       
        public List<Student> Students { get; set; }

        public Course()
        {
            Students = new List<Student>();
        }
    }
}
