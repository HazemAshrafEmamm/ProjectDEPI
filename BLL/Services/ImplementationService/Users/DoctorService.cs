using BLL.Dtos.Appointment;
using BLL.Dtos.Doctor;


namespace BLL.Services.AbstractServices.Users
{
    public class DoctorService : IDoctorService
    {
        public Task<IEnumerable<AvailableDoctorSlotDto>> GetAvailableSlotsAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<DoctorInfoDto> GetDoctorInfoAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoctorInfoDto>> SearchDoctorsAsync(SearchDoctorDto searchDto)
        {
            throw new NotImplementedException();
        }
    }
}
