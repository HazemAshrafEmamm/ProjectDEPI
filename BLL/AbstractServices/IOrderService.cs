using BLL.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderAsync(int orderId, int patientId);
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int patientId);

        Task<OrderDto> CreateOrderAsync(int patientId, CreateOrderDto dto);
        Task<OrderDto> CancelOrderAsync(int orderId, int patientId);

        Task<OrderDto> AddItemToOrderAsync(int orderId, OrderItemDto orderItemDto, int patientId);

        Task<OrderDto> UpdateItemAsync(int orderId,OrderItemDto orderItemDto, int patientId);

        Task<OrderDto> RemoveItemAsync(int orderId, int itemId, int patientId);
    }
}
