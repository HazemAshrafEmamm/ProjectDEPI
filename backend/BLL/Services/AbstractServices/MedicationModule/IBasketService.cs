using BLL.Dtos.Order;
using DomainLayer.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.MedicationModule
{
    public interface IBasketService
    {
        Task<BasketDto> GetBasketAsync(int patientId);
        Task<BasketDto> AddItemAsync(int patientId, BasketItemInputDto dto);
        Task<BasketDto> UpdateItemQuantityAsync(int patientId, int medicationId, int quantity);
        Task RemoveItemAsync(int patientId, int medicationId);
        Task ClearBasketAsync(int patientId);
    }
}
