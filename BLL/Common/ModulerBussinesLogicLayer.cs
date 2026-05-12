using BLL.AbstractServices;
using BLL.ImplementationService;
using BLL.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public static class ModulerBussinesLogicLayer
    {
        public static IServiceCollection AddBuissinesInBLL(this IServiceCollection Services)
        {
            Services.AddScoped<IUserService, UserService>();

            Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));



            return Services;
        }
        }
    }
