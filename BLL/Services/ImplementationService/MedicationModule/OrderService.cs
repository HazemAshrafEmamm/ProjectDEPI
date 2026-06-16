using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.MedicationModule
{
    public class OrderService : IOrderService
    {
        public Task<OrderDto> AddItemToOrderAsync(int orderId, OrderItemDto orderItemDto, int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> CancelOrderAsync(int orderId, int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> CreateOrderAsync(int patientId, CreateOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetOrderAsync(int orderId, int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> RemoveItemAsync(int orderId, int itemId, int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> UpdateItemAsync(int orderId, OrderItemDto orderItemDto, int patientId)
        {
            throw new NotImplementedException();
        }
    }
}
