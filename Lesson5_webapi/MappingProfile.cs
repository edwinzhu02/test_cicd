using AutoMapper;
using Lesson5_webapi.Models; // 替换为你的命名空间
using Lesson5_webapi.Dtos; // 替换为你的命名空间

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddStudentDto, Student>();
    }
}
