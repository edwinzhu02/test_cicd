using Lesson5_webapi.Models;
using System.ComponentModel.DataAnnotations;

namespace Lesson5_webapi.Dtos
{
    public class AddStudentDto
    {

        public string Name { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Gender { get; set; }

    }


    //public class AddStudentDto
    //{
    //    [Required(ErrorMessage = "姓名是必填项")] // 必填字段
    //    [StringLength(50, ErrorMessage = "姓名长度不能超过50个字符")] // 最大长度限制
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "出生日期是必填项")] // 必填字段
    //    [DataType(DataType.Date, ErrorMessage = "出生日期格式不正确")] // 确保是日期类型
    //    public DateOnly BirthDate { get; set; }

    //    [Required(ErrorMessage = "性别是必填项，是必须的")] // 必填字段
    //    [RegularExpression("^(男|女)$", ErrorMessage = "性别只能是'男'或'女'")] // 正则表达式验证
    //    public string Gender { get; set; }
    //}
}
