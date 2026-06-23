using AutoMapper;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Exceptions.OrderModule;
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
    public class BasketService(IUnitOfWork _unitOfWork , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> GetBasketAsync(int basketId)
        {
            var basket =await _unitOfWork.GetRepository<CustomerBasket>().GetByIdAsync(basketId)
                ?? throw new BasketNotFoundException(basketId.ToString());

            var basketDto = _mapper.Map<BasketDto>(basket);
            return basketDto;
        }

        public async Task<BasketDto> CreateBasketAsync(BasketDto basket)
        {
            var basketEntity = _mapper.Map<CustomerBasket>(basket);
            await _unitOfWork.GetRepository<CustomerBasket>().AddAsync(basketEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BasketDto>(basketEntity); ;
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            var existingBasket = await _unitOfWork.GetRepository<CustomerBasket>()
                .GetByIdAsync(basket.Id) ?? throw new BasketNotFoundException(basket.Id.ToString());


            _mapper.Map(basket, existingBasket);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BasketDto>(existingBasket);
        }

        public async Task<bool> DeleteBasketAsync(int basketId)
        {
            var existingBasket = await _unitOfWork.GetRepository<CustomerBasket>().GetByIdAsync(basketId)
                                ?? throw new BasketNotFoundException(basketId.ToString());

            _unitOfWork.GetRepository<CustomerBasket>().Delete(existingBasket);
            var res = await _unitOfWork.SaveChangesAsync();
            return res > 0;
        }
    }
}