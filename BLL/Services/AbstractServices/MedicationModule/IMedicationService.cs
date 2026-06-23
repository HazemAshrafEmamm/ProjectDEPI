using BLL.Dtos.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.MedicationModule
{
    public interface IMedicationService
    {
       Task<MedicationDto> GetMedicationByIdAsync(int id);
       Task<IEnumerable<AllMedicationDto>> GetAllMedicationsAsync();
       Task UpdateMedicationAsync(int PharmacistId,MedicationDto medicationDto);
       Task<MedicationDto> CreateMedicationAsync(int PharmacistId,CreateMedicationDto medicationDto);
       Task DeleteMedicationAsync(int PharmacistId,int id);


    }
}
