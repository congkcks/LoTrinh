using AutoMapper;
using LoTriinhHoc.DTOs;
using LoTriinhHoc.Models;
using Lotrinhhoc.DTOs;

namespace LoTriinhHoc.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Lesson, LessonDto>(); 
        CreateMap<ExerciseType, ExerciseTypeDto>();
        CreateMap<Exercise, ExerciseDto>();
    }
}
