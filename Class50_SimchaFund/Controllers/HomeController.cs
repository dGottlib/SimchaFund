using Class50_SimchaFund.Data;
using Class50_SimchaFund.Models;
using Class50_SimchaFund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            var db = new SimchaFundDB(_connectionString);           
            var vm = new HomeIndexViewModel
            {
                Simchas = db.GetSimchas(),
                TotalContributors = db.GetContributors().Count()
            };
            return View(vm);
        }
    }
}
