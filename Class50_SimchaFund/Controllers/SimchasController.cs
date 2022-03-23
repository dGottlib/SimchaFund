using Class50_SimchaFund.Data;
using Class50_SimchaFund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Web.Controllers
{
    public class SimchasController : Controller
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Simcha simcha)
        {
            var db = new SimchaFundDB(_connectionString);
            int newId = db.AddSimcha(simcha);
            return Redirect("/");
        }
        public IActionResult Contributions(int simchaId)
        {
            var db = new SimchaFundDB(_connectionString);
            var vm = new SimchaContributionsViewModel
            {
                Simcha = db.GetSimchas().FirstOrDefault(s => s.Id == simchaId),
                Contributors = db.GetContributions(simchaId),                
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateContributions(List<Contributor> contributions, int simchaId)
        {
            var db = new SimchaFundDB(_connectionString);
            var updateContributors = contributions.Where(c => c.Include == true).ToList();
            db.UpdateContributions(updateContributors, simchaId);
            return Redirect("/");
        }
    }
}
