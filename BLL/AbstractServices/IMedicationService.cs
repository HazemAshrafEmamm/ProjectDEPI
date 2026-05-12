using BLL.Dtos.Medication;
using DAL.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IMedicationService
    {
        bool CreateMedication(CreateMedicationVM model);
        bool Edit(EditMedicationDTO Model);
        bool Delete(int id);

        //    //Query

        Medication GetById(int id);
       List<GetAllMedicationDTO> GetAll();
    }
}
