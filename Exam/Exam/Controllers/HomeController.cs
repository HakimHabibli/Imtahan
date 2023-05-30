
using Exam.DAL;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {

        private readonly MyDbcontext _dbcontext;

        public HomeController(MyDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index(MyDbcontext myDbcontext)
        {

            List<Team> teams = await _dbcontext.Teams.OrderByDescending(s => s.Id).ToListAsync();
            return View(teams);
        }




    }
}