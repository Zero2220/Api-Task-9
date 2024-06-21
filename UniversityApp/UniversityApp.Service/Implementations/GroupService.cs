using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.Core.Entites;
using UniversityApp.Data.Repositories.Interfaces;
using UniversityApp.Service.Dtos;
using UniversityApp.Service.Dtos.GroupDtos;
using UniversityApp.Service.Exceptions;
using UniversityApp.Service.Interfaces;

namespace UniversityApp.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }
        public int Create(GroupCreateDto createDto)
        {
            if (_groupRepository.Exists(x => x.No == createDto.No && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest,"No", "No already taken");

            Group entity = _mapper.Map<Group>(createDto);

            _groupRepository.Add(entity);
            _groupRepository.Save();

            return entity.Id;
        }

        public void Delete(int id)
        {
            Group entity = _groupRepository.Get(x => x.Id == id && !x.IsDeleted);

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound,"Group not found");

            //_groupRepository.Delete(entity);
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            _groupRepository.Save();
        }

        public PaginatedList<GroupGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10)
        {
            var query = _groupRepository.GetAll(x => !x.IsDeleted && (search == null || x.No.Contains(search)), "Students");
            var paginated =  PaginatedList<Group>.Create(query, page, size);
            return new PaginatedList<GroupGetDto>(_mapper.Map<List<GroupGetDto>>(paginated.Items), paginated.TotalPages,page,size);
        }

        public GroupGetDto GetById(int id)
        {
            Group entity = _groupRepository.Get(x => x.Id == id && !x.IsDeleted,includes:"Students");

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Group not found");
 
            return _mapper.Map<GroupGetDto>(entity);
        }

        public void Update(int id, GroupUpdateDto updateDto)
        {
            Group entity = _groupRepository.Get(x => x.Id == id && !x.IsDeleted,"Students");

            if(entity.No!=updateDto.No && _groupRepository.Exists(x=>x.No == updateDto.No && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "No", "No already taken");

            if (entity.Students.Count > updateDto.Limit)
                throw new RestException(StatusCodes.Status400BadRequest, "Limit", "There are more students than wanted limit");

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Group not found");

            entity.No = updateDto.No;
            entity.Limit = updateDto.Limit;
            entity.ModifiedAt = DateTime.Now;

            _groupRepository.Save();

        }
    }
}
