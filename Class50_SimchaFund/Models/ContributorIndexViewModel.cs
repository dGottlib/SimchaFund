using Class50_SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Web.Models
{
    public class ContributorIndexViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public decimal TotalFunds { get; set; }
    }
}
