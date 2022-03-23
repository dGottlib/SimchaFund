using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Data
{
    public class Simcha
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date {get; set;}
        public int TotalContributorsForSimcha { get; set; }
        public decimal TotalContributed { get; set; }

    }
}
