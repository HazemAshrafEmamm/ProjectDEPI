using AutoMapper;
using BLL.Dtos.Medication;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Exceptions;
using DAL.Exceptions.OrderModule;
using DAL.Models.OrderModule;
using DAL.Models.Users;
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
        private readonly IAttachmentService _attach;
        private readonly IUserRepository _userRepository;

        public MedicationService(IUnitOfWork unitOfWork, IMapper mapper , IAttachmentService attach, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Medication>();
            _attach = attach;
            _userRepository = userRepository;
        }

        public async Task<MedicationDto> CreateMedicationAsync(int PharmacistId,CreateMedicationDto medicationDto)
        {
            var isPharmacist = await ValidatePharmacist(PharmacistId);
            if(!isPharmacist)
                throw new UnauthorizedAccessException("Only pharmacists can create medications.");
            var medicationEntity = _mapper.Map<Medication>(medicationDto);
            if(medicationDto.Image != null)
            {
                var imageUrl = await _attach.Upload(medicationDto.Image, "medications");
                medicationEntity.PictureUrl = imageUrl;
            }
            await _repo.AddAsync(medicationEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MedicationDto>(medicationEntity);
        }

        public async Task DeleteMedicationAsync(int PharmacistId, int id)
        {
            var isPharmacist = await ValidatePharmacist(PharmacistId);
            if (!isPharmacist)
                throw new UnauthorizedAccessException("Only pharmacists can create medications.");

            var medicationEntity = await _repo.GetByIdAsync(id)
                ?? throw new MedicationNotFoundException(id);

            _repo.Delete(medicationEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllMedicationDto>> GetAllMedicationsAsync()
        {
            var medications = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<AllMedicationDto>>(medications);
        }

        public async Task<MedicationDto> GetMedicationByIdAsync(int id)
        {
            var medicationEntity = await _repo.GetByIdAsync(id)
                ?? throw new MedicationNotFoundException(id);

            var medicationDto = _mapper.Map<MedicationDto>(medicationEntity);
            return medicationDto;
        }
        public async Task UpdateMedicationAsync(int PharmacistId,MedicationDto medicationDto)
        {
            var isPharmacist = await ValidatePharmacist(PharmacistId);
            if (!isPharmacist)
                throw new UnauthorizedAccessException("Only pharmacists can create medications.");

            var medicationEntity = await _repo.GetByIdAsync(medicationDto.Id)
                ?? throw new MedicationNotFoundException(medicationDto.Id);

            medicationEntity.Name = medicationDto.Name;
            medicationEntity.Price = medicationDto.Price;
            medicationEntity.Stock = medicationDto.Stock;
            medicationEntity.IsAvailable = medicationDto.Is_available;
             _repo.Update(medicationEntity);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task<bool> ValidatePharmacist(int Id)
        {
            var pharmacist = await _userRepository.GetPharmacistWithMedicationsAsync(Id)
                ?? throw new PharmacistNotFoundException(Id);
            if (pharmacist.UserType == "Pharmacist")
                return true;
            return false;
        }


    }
}
