using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.Service.Dtos;
using UniversityApp.Service.Dtos.GroupDtos;

namespace UniversityApp.Service.Interfaces
{
    public interface IGroupService
    {
        int Create(GroupCreateDto createDto);
        void Update(int id,GroupUpdateDto updateDto);
        GroupGetDto GetById(int id);
        PaginatedList<GroupGetDto> GetAllByPage(string? search=null,int page=1,int size=10);
        void Delete(int id);
    }
}
