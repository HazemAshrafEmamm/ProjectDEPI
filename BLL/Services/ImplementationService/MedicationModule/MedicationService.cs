using AutoMapper;
using BLL.Dtos.Medication;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Models.OrderModule;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.MedicationModule
{
    public class MedicationService : IMedicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenaricRepository<Medication> _repo;

        public MedicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Medication>();
        }

        public async Task<MedicationDto> CreateMedicationAsync(CreateMedicationDto medicationDto)
        {
            var medicationEntity = _mapper.Map<Medication>(medicationDto);
            await _repo.AddAsync(medicationEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MedicationDto>(medicationEntity);
        }

        public async Task DeleteMedicationAsync(int id)
        {
            var medicationEntity = await _repo.GetByIdAsync(id);
            if (medicationEntity == null)
            {
                throw new Exception("Medication not found");
            }
            _repo.Delete(medicationEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllMedicationDto>> GetAllMedicationsAsync()
        {
            var medications = await _repo.GetAllAsync();
            var medicationDtos = _mapper.Map<IEnumerable<AllMedicationDto>>(medications);
            return medicationDtos;
        }

        public async Task<MedicationDto> GetMedicationByIdAsync(int id)
        {
            var medicationEntity = await _repo.GetByIdAsync(id);
            if (medicationEntity == null)
            {
                throw new Exception("Medication not found");
            }
            var medicationDto = _mapper.Map<MedicationDto>(medicationEntity);
            return medicationDto;
        }
        public async Task UpdateMedicationAsync(MedicationDto medicationDto)
        {
            var medicationEntity = await _repo.GetByIdAsync(medicationDto.Id);
            if (medicationEntity == null)
            {
                throw new Exception("Medication not found");
            }
            medicationEntity.Name = medicationDto.Name;
            medicationEntity.Price = medicationDto.Price;
            medicationEntity.Stock = medicationDto.Stock;
            medicationEntity.IsAvailable = medicationDto.Is_available;
             _repo.Update(medicationEntity);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
