using AutoMapper;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Models.OrderModule;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.OrderSpecs;
using DomainLayer.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.MedicationModule
{
    public class OrderService(IUnitOfWork  _unitOfWork , IMapper _mapper) : IOrderService
    {

        public Task<OrderDto> CancelOrderAsync(int orderId, int patientId)
        {

            throw new NotImplementedException();
        }

        public async Task<OrderDto> CreateOrderAsync(int patientId, CreateOrderDto dto)
        {
            var Address = _mapper.Map<OrderAddress>(dto.Address);
         
            var Basket = (await _unitOfWork.GetRepository<CustomerBasket>()
                                            .GetAllAsync(new BasketById(dto.BasketId))).FirstOrDefault();
            if (Basket == null)
                throw new Exception("Basket not found");

            if (Basket.IsCheckedOut)
                throw new Exception("Order already created");

            List<OrderItem> orderItems = new List<OrderItem>();

            var medicationRepo = _unitOfWork.GetRepository<Medication>();

            foreach (var basketitem in Basket.BasketItems)
            {
                var OriginalProduct = await medicationRepo.GetByIdAsync(basketitem.MedicationId)
                    ?? throw new Exception($"Product with ID {basketitem.MedicationId} not found");
                if(OriginalProduct.Stock < basketitem.Quantity)
                     throw new Exception($"{OriginalProduct.Name}: available quantity is {OriginalProduct.Stock}");

                var OrderItem = new OrderItem()
                {

                    MedicationId = basketitem.MedicationId,
                    Name = OriginalProduct.Name,
                    PictureUrl = OriginalProduct.PictureUrl,
                    UnitPrice = OriginalProduct.Price,
                    Quantity = basketitem.Quantity,
                };

                orderItems.Add(OrderItem);
                OriginalProduct.Stock -= basketitem.Quantity;

            }

            var SubTotal = orderItems.Sum(i => i.UnitPrice * i.Quantity);
            var Order = new Order()
            {
                PatientId = patientId,
                SubTotal = SubTotal,
                Address = Address,
                Order_Item = orderItems,
                Status = OrderStatus.Pending,
            };

            await _unitOfWork.GetRepository<Order>().AddAsync(Order);
            
            Basket.IsCheckedOut = true;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(Order);

        }
        

        public async Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int patientId)
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync(new OrdersByPatientId(patientId));
            if(orders == null || !orders.Any())
                return Enumerable.Empty<OrderDto>();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
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
