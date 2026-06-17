using BLL.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.MedicationModule
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderAsync(int orderId, int patientId);
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int patientId);
        Task<OrderDto> GetOrderForMerchantAsync(int orderId);
        Task<OrderDto> CreateOrderAsync(int patientId, CreateOrderDto dto);
        Task<OrderDto> CancelOrderAsync(int orderId, int patientId);
        Task<OrderDto> UpdateOrderStatus(int orderId, UpdateOrderStatus dto);


    }
}
