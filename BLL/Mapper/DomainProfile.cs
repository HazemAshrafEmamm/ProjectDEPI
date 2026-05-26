using AutoMapper;
using BLL.Dtos.Medication;
using DAL.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<Medication,CreateMedicationDto>().ReverseMap();
            CreateMap<Medication, EditMedicationDTO>().ReverseMap();
            CreateMap<Medication, AllMedicationDTO>().ReverseMap();
        }
    }
}
