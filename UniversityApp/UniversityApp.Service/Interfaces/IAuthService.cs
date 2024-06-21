using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.Service.Dtos.UserDtos;

namespace UniversityApp.Service.Interfaces
{
    public interface IAuthService
    {
        string Login(UserLoginDto loginDto);
    }
}
