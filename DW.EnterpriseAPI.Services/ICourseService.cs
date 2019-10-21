using DW.EnterpriseAPI.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DW.EnterpriseAPI.Services
{
    public interface ICourseService
    {
        Task<List<Student>> GetStudentsListByCourse(int courseId);
    }
}
