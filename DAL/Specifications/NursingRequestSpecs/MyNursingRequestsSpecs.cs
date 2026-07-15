using DAL.Models.NursingModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications.NursingRequestSpecs
{
    public class MyNursingRequestsSpecs : BaseSpecification<NursingRequest>
    {
        public MyNursingRequestsSpecs(int requesterId) : base(r => r.PatientId == requesterId || r.NurseId == requesterId) 
        {
        }
    }
}
