using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DW.EnterpriseAPI.Entity;
using DW.EnterpriseAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DW.EnterpriseAPI.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly EnterpriseContext _context;

        public StudentService(EnterpriseContext context)
        {
            _context = context;
        }

        public async Task<Student> CreateAsync(Student student)
        {
           var newStudent =  await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return newStudent.Entity;
        }

        public async Task<Student> DeleteAsync(int studentId)
        {
            var student = await GetStudentByIdAsync(studentId);
            _context.Remove(student);
           await  _context.SaveChangesAsync();
            return student;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<List<Student>> GetListStudent()
        {
            var students = await GetAllAsync();
            List<Student> lista = new List<Student>();
            foreach (var item in students)
            {
                lista.Add(new Student()
                {
                    Id = item.Id,
                    StudentName=item.StudentName
                });
            }
            return lista;
        }

        public async  Task<Student> GetStudentByCourse(
                                    int studentId, 
                                    int courseId)
        {
            var student = await GetAllAsync();
            return student.Where(s => s.Id == studentId &&
                            s.CourseId == courseId).FirstOrDefault();
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students.Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId)
        {
            return await _context.Students.Where(s => s.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }
    }
}
