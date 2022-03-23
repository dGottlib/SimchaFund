using Class50_SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Web.Models
{
    public class SimchaContributionsViewModel
    {
        public Simcha Simcha { get; set; }
        public List<Contributor> Contributors { get; set; } 
    }
}
