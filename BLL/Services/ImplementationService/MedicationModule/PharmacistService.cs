using AutoMapper;
using BLL.Dtos.Medication;
using DAL.Models.OrderModule;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.MedicationModule
{
    public class PharmacistService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenaricRepository<Medication> _repo;

        public PharmacistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Medication>();
        }

        public async Task<int> CreateMedicationAsync(CreateMedicationDto medication)
        {
            var medicationEntity = _mapper.Map<Medication>(medication);
            await _repo.AddAsync(medicationEntity);
            return await _unitOfWork.SaveChangesAsync();
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

        public async Task<int> DeleteMedicationAsync(int id)
        {
            var medicationEntity = await _repo.GetByIdAsync(id);
            if (medicationEntity == null)
            {
                throw new Exception("Medication not found");
            }
            _repo.Delete(medicationEntity);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
