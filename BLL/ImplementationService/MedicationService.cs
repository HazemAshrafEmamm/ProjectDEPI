using AutoMapper;
using Azure;
using BLL.AbstractServices;
using BLL.Dtos.Medication;
using DAL.Models.OrderModule;
using DAL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository medicationRepository;
        private readonly IMapper mapper;

        public MedicationService(IMedicationRepository medicationRepository, IMapper mapper)
        {
            this.medicationRepository = medicationRepository;
            this.mapper = mapper;
        }

        public bool CreateMedication(CreateMedicationVM model)
        {
            try {
                var mapp = mapper.Map<Medication>(model);


                if (mapp!=null)
                {
                    var result = medicationRepository.AddMedication(mapp);

                    return ( true);
                }
                else
                {
                    return ( false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var result = medicationRepository.Delete(id);
                if (result == null)
                {
                    return( false);
                }
                else
                {
                return ( true);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Edit(EditMedicationDTO Model)
        {
            try
            {
                var mapp =  mapper.Map<Medication>(Model);


                if (mapp!=null)
                {
                    var result = medicationRepository.Edit(mapp);

                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<GetAllMedicationDTO> GetAll()
        {
            try
            {
                var result = medicationRepository.GetAll();
             
                var mapp = mapper.Map<List<GetAllMedicationDTO>>(result);
                return mapp;
            }
            catch (Exception ex)
            {
                throw;            }
        }

        public Medication GetById(int id)
        {
            try
            {
                var result =medicationRepository.GetById( id);

                return result;



            }
            catch (Exception)
            {
                throw;
            }
        }
    }
        
}
