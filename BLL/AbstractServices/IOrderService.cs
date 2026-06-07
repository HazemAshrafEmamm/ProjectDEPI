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
        Task<OrderDto> GetOrderAsync(string orderId, string patientId);
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(string patientId);

        Task<OrderDto> CreateOrderAsync(string patientId, CreateOrderDto dto);
        Task<OrderDto> CancelOrderAsync(string orderId, string patientId);


        Task<OrderDto> AddItemToOrderAsync(string orderId, OrderItemDto orderItemDto, string patientId);

        Task<OrderDto> UpdateItemAsync(string orderId,OrderItemDto orderItemDto, string patientId);

        Task<OrderDto> RemoveItemAsync(string orderId, string itemId, string patientId);

    }
}
