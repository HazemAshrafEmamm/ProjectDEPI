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
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderAsync(int orderId, int patientId);
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int patientId);
        Task<OrderDto> GetOrderForMerchantAsync(int orderId);
        Task DeleteOrderAsync(int OrderId, int PatientId);

        Task<OrderDto> CreateOrderAsync(int patientId, CreateOrderDto dto);
        Task<OrderDto> CancelOrderAsync(int orderId, int patientId);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatus dto);


    }
}
