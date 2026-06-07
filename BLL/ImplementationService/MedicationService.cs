using AutoMapper;
using BLL.AbstractServices;
using BLL.Dtos.Medication;
using DAL.Models.OrderModule;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService
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
        public async Task<int> AddMedicationAsync(CreateMedicationDto medication)
        {
            var medicationEntity = _mapper.Map<Medication>(medication);
            await _repo.AddAsync(medicationEntity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteMedicationAsync(string id)
        {
            var medicationEntity = await _repo.GetByIdAsync(id);
            if (medicationEntity == null)
            {
                throw new Exception("Medication not found");
            }
            _repo.Delete(medicationEntity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllMedicationDto>> GetAllMedicationsAsync()
        {
            var medications = await _repo.GetAllAsync();
            var medicationDtos = _mapper.Map<IEnumerable<AllMedicationDto>>(medications);
            return medicationDtos;
        }

        public async Task<MedicationDto> GetMedicationByIdAsync(string id)
        {
            var medicationEntity = await _repo.GetByIdAsync(id);
            if (medicationEntity == null)
            {
                throw new Exception("Medication not found");
            }
            var medicationDto = _mapper.Map<MedicationDto>(medicationEntity);
            return medicationDto;
        }

        public async Task<int> UpdateMedicationAsync(UpdateMedicationDto medication)
        {
            var existingMedication = await _repo.GetByIdAsync(medication.Id);

            if (existingMedication == null)
                throw new KeyNotFoundException("Medication not found");

            _mapper.Map(medication, existingMedication);

            _repo.Update(existingMedication);

            return await _unitOfWork.SaveChangesAsync();
        }

    }
}
