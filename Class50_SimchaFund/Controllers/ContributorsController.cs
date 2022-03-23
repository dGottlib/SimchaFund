using Class50_SimchaFund.Data;
using Class50_SimchaFund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class50_SimchaFund.Web.Controllers
{
    public class ContributorsController : Controller
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            var db = new SimchaFundDB(_connectionString);
            var contributions = db.GetContributors();
            var vm = new ContributorIndexViewModel
            {
                Contributors = contributions,
                TotalFunds = db.TotalFunds()
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Add(Contributor contributor, Deposit deposit)
        {
            var db = new SimchaFundDB(_connectionString);
            int id = db.AddContributor(contributor);
            db.AddDeposit(deposit, id);
            return Redirect("/Contributors");
        }
        [HttpPost]
        public IActionResult Deposit(Deposit deposit)
        {
            var db = new SimchaFundDB(_connectionString);
            db.AddDeposit(deposit);
            return Redirect("/Contributors");
        }
        public IActionResult History(int contributorId)
        {
            var db = new SimchaFundDB(_connectionString);
            var vm = new ContributorsHistoryViewModel
            {
                Contributor = db.GetContributors().FirstOrDefault(c => c.Id == contributorId),
                Transactions = db.GetHistory(contributorId)
            };
            return View(vm);
        }
        public IActionResult Update(Contributor contributor)
        {
            var db = new SimchaFundDB(_connectionString);
            db.UpdateContributor(contributor);
            return Redirect("/Contributors");
        }
    }
}
