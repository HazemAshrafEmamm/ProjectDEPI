using BLL.AbstractServices;
using BLL.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService
{
    public class OrderService : IOrderService
    {
        public Task<OrderDto> AddItemToOrderAsync(string orderId, OrderItemDto orderItemDto, string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> CancelOrderAsync(string orderId, string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> CreateOrderAsync(string patientId, CreateOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDto>> GetMyOrdersAsync(string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetOrderAsync(string orderId, string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> RemoveItemAsync(string orderId, string itemId, string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> UpdateItemAsync(string orderId, OrderItemDto orderItemDto, string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
