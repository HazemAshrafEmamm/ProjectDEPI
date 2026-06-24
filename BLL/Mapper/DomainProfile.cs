using AutoMapper;
using BLL.Dtos.Appointment;
using BLL.Dtos.Consultion;
using BLL.Dtos.Medication;
using BLL.Dtos.Order;
using BLL.Dtos.Schedule;
using DAL.Models.AppointmentModule;
using DAL.Models.Consultation;
using DAL.Models.OrderModule;
using DomainLayer.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // Appointment Mappings
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();
            CreateMap<Appointment, UpdateAppointmentDto>().ReverseMap();

            // Schedule Mappings
            CreateMap<DoctorSchedule, DoctorScheduleDto>().ReverseMap();
            CreateMap<DoctorSchedule, CreateDoctorScheduleDto>().ReverseMap();
            CreateMap<DoctorSchedule, UpdateDoctorScheduleDto>().ReverseMap();

            // Medication Mappings
            CreateMap<Medication, MedicationDto>().ReverseMap();
            CreateMap<Medication, CreateMedicationDto>()
                        .ForMember(dest => dest.Image, opt => opt.Ignore())
                        .ReverseMap();
            CreateMap<Medication, UpdateMedicationDto>()
            .ForMember(dest => dest.Image, opt => opt.Ignore())
            .ReverseMap();
            CreateMap<Medication, AllMedicationDto>().ReverseMap();

            // Consultation Mappings
            CreateMap<Consultation, ConsultationDto>().ReverseMap();
            CreateMap<Consultation, CreateConsultationDto>().ReverseMap();

            CreateMap<ConsultationMessage, ConsultationMessageDto>().ReverseMap();
            CreateMap<ConsultationMessage, SendMessageDto>().ReverseMap();

            CreateMap<ConsultationReview, ConsultationReviewDto>().ReverseMap();
            CreateMap<ConsultationReview, CreateConsultationReviewDto>().ReverseMap();

            // Order Mappings
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemDto>().ReverseMap();


            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Medication.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Medication.PictureUrl))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Price * src.Quantity));

            CreateMap<CustomerBasket, BasketDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BasketItems))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src =>
                    src.BasketItems.Sum(i => i.Price * i.Quantity)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src =>
                    src.BasketItems.Sum(i => i.Price * i.Quantity) + src.ShippingPrice));


        }
    }
}