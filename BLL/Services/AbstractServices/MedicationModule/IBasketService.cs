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
        Task<BasketDto> GetBasketAsync(int basketId);
        Task<BasketDto> CreateBasketAsync(BasketDto basket);
        Task<BasketDto> UpdateBasketAsync(BasketDto basket);
        Task<bool> DeleteBasketAsync(int basketId);
    }
}
