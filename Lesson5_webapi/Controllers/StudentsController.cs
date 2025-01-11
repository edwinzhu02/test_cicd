using Microsoft.AspNetCore.Mvc;
using Lesson5_webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Lesson5_webapi.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lesson5_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentsController : ControllerBase
    {
        // GET: api/<StudentsController>
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController( IMapper mapper, ILogger<StudentsController> logger)
        {
             _mapper = mapper;
            _logger = logger;

        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StudentsController>/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            _logger.LogInformation("Received request to get student with ID {Id}", id);
            //int result = 10 / int.Parse("0");
            using (var context = new TestdbContext())
            {
                // 查询学生
                var student = context.Students.Find(id);
                if (student != null)
                {
                    // 返回学生信息
                    var studentInfo = new
                    {
                        Id = student.Id,
                        Name = student.Name,
                        BirthDate = student.BirthDate,
                        Gender = student.Gender
                    };
                    _logger.LogDebug("Student data retrieved: {@Student}", studentInfo);

                    return Ok(studentInfo); // 返回 200 OK 响应并附带学生信息
                }
                else
                {
                    // 如果未找到学生，返回 404 Not Found
                    _logger.LogWarning("Invalid ID {Id} received", id);
                    return NotFound($"Invalid Student ID {id} received！");
                }
            }
        }


        // POST api/<StudentsController>
        [HttpPost]


        public IActionResult Post([FromBody] AddStudentDto studentDto)
        //public IActionResult Post([FromBody] Student student)
        {
            //if (!ModelState.IsValid)
            //{
            //    // 返回 400 Bad Request 和验证错误信息
            //    //return BadRequest(ModelState);
            //}
            Student student = new Student();
            student.Gender = studentDto.Gender;
            student.Name = studentDto.Name;
            student.BirthDate = studentDto.BirthDate;
            //var student = _mapper.Map<Student>(studentDto);

            using (var context = new TestdbContext())
            {

                // 添加新学生到数据库
                context.Students.Add(student);
                context.SaveChanges();

                // 返回 201 Created 响应，并附带新创建的学生信息
                return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
