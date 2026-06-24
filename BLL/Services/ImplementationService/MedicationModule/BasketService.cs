using AutoMapper;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Exceptions;
using DAL.Exceptions.OrderModule;
using DAL.Models.OrderModule;
using DAL.Repository;
using DAL.Specifications.OrderSpecs;
using DomainLayer.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.MedicationModule
{
    public class BasketService(IUnitOfWork _unitOfWork, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> GetBasketAsync(int patientId)
        {
            var spec = new BasketByUserIdSpecs(patientId);
            var basket = (await _unitOfWork.GetRepository<CustomerBasket>().GetAllAsync(spec)).FirstOrDefault();

            if (basket is null)
                throw new BasketNotFoundException(patientId);

            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> AddItemAsync(int patientId, BasketItemInputDto dto)
        {
            var medication = await _unitOfWork.GetRepository<Medication>().GetByIdAsync(dto.MedicationId);

            if (medication is null)
                throw new MedicationNotFoundException(dto.MedicationId);

            if (!medication.IsAvailable || medication.Stock < dto.Quantity)
                throw new InsufficientStockException(medication.Name, medication.Stock);

            var spec = new BasketByUserIdSpecs(patientId);
            var basket = (await _unitOfWork.GetRepository<CustomerBasket>().GetAllAsync(spec)).FirstOrDefault();

            if (basket is null)
            {
                basket = new CustomerBasket { PatientId = patientId };
                await _unitOfWork.GetRepository<CustomerBasket>().AddAsync(basket);
            }

            var existingItem = basket.BasketItems
                .FirstOrDefault(i => i.MedicationId == dto.MedicationId);

            if (existingItem is not null)
                existingItem.Quantity += dto.Quantity;
            else
                basket.BasketItems.Add(new BasketItem
                {
                    MedicationId = dto.MedicationId,
                    Price = medication.Price,
                    PictureUrl = medication.PictureUrl,
                    Quantity = dto.Quantity
                });

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateItemQuantityAsync(int patientId, int medicationId, int quantity)
        {
            var spec = new BasketByUserIdSpecs(patientId);
            var basket = (await _unitOfWork.GetRepository<CustomerBasket>().GetAllAsync(spec)).FirstOrDefault();

            if (basket is null)
                throw new BasketNotFoundException(patientId);

            var item = basket.BasketItems
                .FirstOrDefault(i => i.MedicationId == medicationId)
                ?? throw new MedicationNotFoundException(medicationId);

            item.Quantity = quantity;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task RemoveItemAsync(int patientId, int medicationId)
        {
            var spec = new BasketByUserIdSpecs(patientId);
            var basket = (await _unitOfWork.GetRepository<CustomerBasket>().GetAllAsync(spec)).FirstOrDefault();

            if (basket is null)
                throw new BasketNotFoundException(patientId);

            var item = basket.BasketItems
                .FirstOrDefault(i => i.MedicationId == medicationId)
                ?? throw new MedicationNotFoundException(medicationId);

            _unitOfWork.GetRepository<BasketItem>().Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClearBasketAsync(int patientId)
        {
            var spec = new BasketByUserIdSpecs(patientId);
            var basket = (await _unitOfWork.GetRepository<CustomerBasket>().GetAllAsync(spec)).FirstOrDefault();

            if (basket is null)
                throw new BasketNotFoundException(patientId);

            var itemRepo = _unitOfWork.GetRepository<BasketItem>();
            foreach(var item in basket.BasketItems.ToList())
            {
                itemRepo.Delete(item);
            }
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}