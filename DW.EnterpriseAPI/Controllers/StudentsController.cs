using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DW.EnterpriseAPI.Entity;
using DW.EnterpriseAPI.Models;
using DW.EnterpriseAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DW.EnterpriseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        private IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Serilog.ILogger _loggerDb;

        public StudentsController(IStudentService service, IMapper mapper,
                                ILogger<StudentsController> logger, 
                                Serilog.ILogger logDb)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _loggerDb = logDb;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IEnumerable<Student>> Get()
        {
            _logger.LogDebug(
                "Este es un mnesaje de tipo DEBUG desde el GetAll");
            _logger.LogWarning("Log WARNING desde GetAll");
            _logger.LogError("Este es un error generado por el desarrollador");
            // Log DataBase
            _loggerDb.Warning("Log WARNING a la BD");
            _loggerDb.Error("Log de ERROR a la BD");
            return await _service.GetAllAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Student> Get(int id)
        {
            _loggerDb.Information("GetById " + id.ToString());

            return await _service.GetStudentByIdAsync(id);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<List<StudentListItemViewModel>> ListStudents()
        {
            var students = await _service.GetListStudent();
            List<StudentListItemViewModel> lista =
                        new List<StudentListItemViewModel>();
            //  var config = new MapperConfiguration(cfg =>
            //cfg.CreateMap<Origen, Destino>());

            //var config = new MapperConfiguration(cfg =>
            //          cfg.CreateMap<Student, StudentListItemViewModel>());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentListItemViewModel>()
                .ForMember(destination => destination.Estudiante,
                    opts => opts.MapFrom(source => source.StudentName))
                .ForMember(destination => destination.Codigo,
                    opts => opts.MapFrom(source => source.Id));
            });
            try
            {
                _mapper = config.CreateMapper();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error " + ex.Message, ex.StackTrace);
                //throw;
            }


            lista = _mapper.Map<List<StudentListItemViewModel>>(students.ToList());
            //foreach (var item in students)
            //{
            //    var studentVM = _mapper.Map<StudentListItemViewModel>
            //        (item);
            //    lista.Add(studentVM);
            //    //lista.Add(new StudentListItemViewModel
            //    //{
            //    //    Id = item.Id,
            //    //    StudentName = item.StudentName
            //    //});
            //}
            return lista.OrderBy(s => s.Estudiante).ToList();
        }

        [HttpGet("{studentId}/{courseId}")]
        public async Task<Student> StudentByCourse(int studentId, int courseId)
        {
            return await _service.GetStudentByCourse(studentId, courseId);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<Student> Post([FromBody] Student value)
        {
            return await _service.CreateAsync(value);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<Student> Put(int id, [FromBody] Student value)
        {
            return await _service.UpdateAsync(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Student> Delete(int id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}
