using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DW.EnterpriseAPI.Entity;
using DW.EnterpriseAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DW.EnterpriseAPI.Controllers
{
    /// <summary>
    /// API de los Cursos de la API de Digital Ware
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly ILogger _logger;
        public CoursesController(ICourseService service,
            ILogger<CoursesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // 
        /// <summary>
        /// GET: api/Courses
        /// Lista todos los cursos
        /// </summary>
        /// <returns>Devuelve una lista de objetos de cursos</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogWarning("Warning COURSES");
            return new string[] { "value1", "value2" };
        }

        // 
        /// <summary>
        /// GET: api/Courses/5
        /// Buscar un curso por su Id
        /// </summary>
        /// <param name="id">Id del curso a buscar</param>
        /// <returns>EL objeto del curso encontrado</returns>
        [HttpGet("{id}", Name = "GetCourseById")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        [Route("[action]/{courseId}")]
        public async Task<List<Student>> StudentsByCourse(int courseId)
        {
            return await _service.GetStudentsListByCourse(courseId);
        }

        // POST: api/Courses
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
