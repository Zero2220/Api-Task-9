using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.Service.Dtos;
using UniversityApp.Service.Dtos.StudentDtos;

namespace UniversityApp.Service.Interfaces
{
    public interface IStudentService
    {
        int Create(StudentCreateDto createDto);
        StudentGetDto GetById(int id);
        PaginatedList<StudentGetDto> GetAllPaginated(int page=1,int size=10);
    }
}
