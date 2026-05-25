using DAL.RepositoryImplementations;
using DAL.RepositoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public static class ModulerDataAccessLayer
    {
        public static IServiceCollection AddBuissinesInDAL(this IServiceCollection Services)
        {
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddScoped<IMedicationRepository, MedicationRepository>();



            return Services;

        }
    }
}
