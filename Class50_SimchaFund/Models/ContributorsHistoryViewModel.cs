using Class50_SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Web.Models
{
    public class ContributorsHistoryViewModel
    {
        public Contributor Contributor { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
