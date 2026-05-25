using DAL.Data;
using DAL.Models.OrderModule;
using DAL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DAL.RepositoryImplementations
{
    
    public class MedicationRepository : IMedicationRepository
    {
        private readonly TabibyDbContext context;

        public MedicationRepository(TabibyDbContext context)
        {
            this.context = context;
        }

        public bool AddMedication(Medication medication)
        {
            try
            {
                var result = context.Medications.Add(medication);
                context.SaveChanges();
                if (result.Entity.Id >0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var result = GetById( id);
                if (result!=null)
                {
                    context.Medications.Remove(result);
                    context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Edit(Medication newMedication)
        {
            try
            {
                var oldMedication = GetById(newMedication.Id);

                if (oldMedication == null)
                    return false;

                oldMedication.Is_available = newMedication.Is_available;
                oldMedication.Price = newMedication.Price;
                oldMedication.Stock = newMedication.Stock;
                // كمل باقي الخصائص

                context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Medication> GetAll()
        {
            try
            {
               
                    var result =  context.Medications.ToList();
                return  result;

            
            }
            catch (Exception)
            {
                throw;
            
        }   }

        public Medication GetById(int id)
        {

            try
            {
                var result = context.Medications.FirstOrDefault(a => a.Id == id);

                return result;



            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
