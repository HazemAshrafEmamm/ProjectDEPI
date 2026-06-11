using AutoMapper;
using BLL.Dtos.Consultion;
using BLL.Dtos.Medication;
using DAL.Models.Consultation;
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
            CreateMap<Medication, UpdateMedicationDto>().ReverseMap();
            CreateMap<Medication, AllMedicationDto>().ReverseMap();

            CreateMap<ConsultationReview, CreateConsultationReviewDto>().ReverseMap();
            CreateMap<ConsultationReview, ConsultationReviewDto>().ReverseMap();

            CreateMap<Consultation, ConsultationDto>().ReverseMap();

        }
    }
}
