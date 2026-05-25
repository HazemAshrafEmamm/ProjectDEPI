using DAL.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RepositoryInterfaces
{
    public interface IMedicationRepository
    {
        bool AddMedication(Medication medication);
        bool Edit(Medication medication);
        bool Delete(int id);

        

        Medication GetById(int id);
        List<Medication> GetAll();
    }
}
