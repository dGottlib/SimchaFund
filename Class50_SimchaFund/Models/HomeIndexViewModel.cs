using Class50_SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Web.Models
{
    public class HomeIndexViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public int TotalContributors {get; set;}    
    }
}
