﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Data
{
   public class Contributor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public bool AlwaysInclude { get; set; }  
        public decimal Balance { get; set; }  
        public bool Include { get; set; }     
        public decimal Amount { get; set; }
    }
}
