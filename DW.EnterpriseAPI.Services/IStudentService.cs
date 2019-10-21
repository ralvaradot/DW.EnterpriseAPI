using DW.EnterpriseAPI.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DW.EnterpriseAPI.Services
{
    public interface IStudentService
    {
        // Lista general de Estudiantes
        Task<IEnumerable<Student>> GetAllAsync();
        // Lista de estudiantes por curso
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<Student> DeleteAsync(int studentId);
        Task<List<Student>> GetListStudent();
        Task<Student> GetStudentByCourse(int studentId, int courseId);
    }
}
