using AutoMapper;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Exceptions;
using DAL.Exceptions.OrderModule;
using DAL.Models.OrderModule;
using DAL.Models.Users;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.OrderSpecs;
using DomainLayer.Models.BasketModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.MedicationModule
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IUserRepository _userRepository) : IOrderService
    {
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync(new DAL.Specifications.OrderSpecs.AllOrdersSpecs());
            if (orders == null)
                return Enumerable.Empty<OrderDto>();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CancelOrderAsync(int orderId, int patientId)
        {
            var order = (await _unitOfWork.GetRepository<Order>()
                                .GetAllAsync(new OrderByOrderIdAndPatientIdSpecs(orderId, patientId))).FirstOrDefault()
                                    ?? throw new OrderNotFoundException(orderId);

            if (order.Status != OrderStatus.Pending)
                throw new OrderNotCancelableException(orderId);

            var patient = await _userRepository.GetPatientWithBasketAsync(patientId)
                ?? throw new PatientNotFoundException(patientId);


            #region Restore stock
            
            
            
            
            
            
            
            
            
            await RestoreStock(order);
            #endregion

            var Basket = patient.Basket;

            if(Basket != null)
            {
                Basket.IsCheckedOut = false;
            }


            order.Status = OrderStatus.Cancelled;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);
        }
        public async Task DeleteOrderAsync(int OrderId , int PatientId)
        {
            var Order = (await _unitOfWork.GetRepository<Order>()
                                   .GetAllAsync(new OrderByOrderIdAndPatientIdSpecs(OrderId, PatientId))).FirstOrDefault()
                                   ?? throw new OrderNotFoundException(OrderId);
            if(Order.Status != OrderStatus.Cancelled 
                    || Order.Status != OrderStatus.Rejected || Order.Status != OrderStatus.Delivered)
                throw new OrderCantBeDeletedException(OrderId);
             _unitOfWork.GetRepository<Order>().Delete(Order);
             await _unitOfWork.SaveChangesAsync();

        }
        public async Task<OrderDto> CreateOrderAsync(int patientId, CreateOrderDto dto)
        {
            var Address = _mapper.Map<OrderAddress>(dto.Address);
         
            var patient = await _userRepository.GetPatientWithBasketAsync(patientId)
                ?? throw new PatientNotFoundException(patientId);

            var Basket = (await _unitOfWork.GetRepository<CustomerBasket>()
                                            .GetAllAsync(new BasketByIdSpecs(patient!.Basket!.Id))).FirstOrDefault()
                                            ?? throw new BasketNotFoundException(patient!.Basket!.Id);

            if (Basket.IsCheckedOut)
                throw new BasketAlreadyCheckedOutException(patient!.Basket!.Id);

            List<OrderItem> orderItems = new List<OrderItem>();

            var medicationRepo = _unitOfWork.GetRepository<Medication>();

            foreach (var basketItem in Basket.BasketItems)
            {
                var OriginalProduct = await medicationRepo.GetByIdAsync(basketItem.MedicationId)
                                 ?? throw new MedicationNotFoundException(basketItem.MedicationId);
                if (OriginalProduct.Stock < basketItem.Quantity)
                                    throw new InsufficientStockException(OriginalProduct.Name, OriginalProduct.Stock);

                var OrderItem = new OrderItem()
                {

                    MedicationId = basketItem.MedicationId,
                    Name = OriginalProduct.Name,
                    PictureUrl = OriginalProduct.PictureUrl ?? "",
                    UnitPrice = OriginalProduct.Price,
                    Quantity = basketItem.Quantity,
                };

                orderItems.Add(OrderItem);
                OriginalProduct.Stock -= basketItem.Quantity;

            }

            var SubTotal = orderItems.Sum(i => i.UnitPrice * i.Quantity);
            var Order = new Order()
            {
                PatientId = patientId,
                SubTotal = SubTotal,
                Address = Address,
                OrderItem = orderItems,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
            };

            await _unitOfWork.GetRepository<Order>().AddAsync(Order);
            
            var itemRepo = _unitOfWork.GetRepository<BasketItem>();
            foreach (var item in Basket.BasketItems.ToList())
            {
                itemRepo.Delete(item);
            }
            Basket.IsCheckedOut = false; 

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(Order);

        }

        public async Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int patientId)
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync(new OrdersByPatientIdSpecs(patientId));
            if (orders == null || !orders.Any())
                throw new OrdersNotFoundByPatientIdException(patientId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderAsync(int orderId, int patientId)
        {
            var order = (await _unitOfWork.GetRepository<Order>()
                        .GetAllAsync(new OrderByOrderIdAndPatientIdSpecs(orderId, patientId))).FirstOrDefault() 
                        ?? throw new OrderNotFoundException(orderId);

            return _mapper.Map<OrderDto>(order);
        }
        public async Task<OrderDto> GetOrderForMerchantAsync(int orderId)
        {
            var order = (await _unitOfWork.GetRepository<Order>()
                .GetAllAsync(new OrderByIdSpec(orderId)))
                .FirstOrDefault()
                ?? throw new OrderNotFoundException(orderId);

            return _mapper.Map<OrderDto>(order);
        }
        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatus dto)
        {
            var order = (await _unitOfWork.GetRepository<Order>()
                .GetAllAsync(new OrderByIdSpec(orderId)))
                .FirstOrDefault()   
                ?? throw new OrderNotFoundException(orderId);

            if (order.Status == OrderStatus.Pending && dto.Status == OrderStatus.Rejected)
            {
                await RestoreStock(order);
            }

            order.Status = dto.Status;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }
        private async Task RestoreStock(Order order)
        {
            var medicationRepo = _unitOfWork.GetRepository<Medication>();
            var ids = order.OrderItem.Select(x => x.MedicationId).ToList();

            var meds = await medicationRepo.GetAllAsync(new MedicationsByIdsSpec(ids));

            var dict = meds.ToDictionary(x => x.Id);

            foreach (var item in order.OrderItem)
            {
                if (dict.TryGetValue(item.MedicationId, out var medication))
                {
                    medication.Stock += item.Quantity;
                }
            }
        }

    }
}
