using AutoMapper;
using BLL.AbstractServices.MedicationModule;
using BLL.Dtos.Medication;
using DAL.Models.OrderModule;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService.MedicationModule
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

        

    }
}
