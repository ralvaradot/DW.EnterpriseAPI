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
    public class CourseService : ICourseService
    {
        private readonly EnterpriseContext _context;

        public CourseService(EnterpriseContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsListByCourse(int courseId)
        {
            return await  _context.Students
                .Where(s => s.CourseId == courseId).ToListAsync();
        }
    }
}
