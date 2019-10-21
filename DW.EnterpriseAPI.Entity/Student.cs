using System.ComponentModel.DataAnnotations;

namespace DW.EnterpriseAPI.Entity
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string StudentName { get; set; }
        [Required]
        [MaxLength(10)]
        public string StudentCellular { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
