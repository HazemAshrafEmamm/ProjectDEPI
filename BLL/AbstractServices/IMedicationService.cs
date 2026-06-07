using BLL.Dtos.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IMedicationService
    {
       Task<int> AddMedicationAsync(CreateMedicationDto medication);
       Task<int> UpdateMedicationAsync(UpdateMedicationDto medication);
       Task<int> DeleteMedicationAsync(string id);
       Task<MedicationDto> GetMedicationByIdAsync(string id);
       Task<IEnumerable<AllMedicationDto>> GetAllMedicationsAsync();
    }
}
