using BLL.Dtos.Medication;


namespace BLL.AbstractServices.MedicationModule
{
    public interface IPharmacistService
    {
        Task<int> CreateMedicationAsync(CreateMedicationDto medication);
        Task<int> UpdateMedicationAsync(UpdateMedicationDto medication);
        Task<int> DeleteMedicationAsync(string id);
    }
}
