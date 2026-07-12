using BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices
{
    public interface IProfileUserService
    {
        Task<ProfileUser> GetProfileUserByIdAsync(int id);
        Task<ProfileUser> UpdateMyProfile(int id, ProfileUser profile);
    }
}
