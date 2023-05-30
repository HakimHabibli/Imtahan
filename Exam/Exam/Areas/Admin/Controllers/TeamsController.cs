using Exam.Areas.Admin.ViewModels;
using Exam.DAL;
using Exam.Models;
using Exam.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {

        private readonly MyDbcontext _myDbcontext;
        private readonly IWebHostEnvironment _webHost;

        public TeamsController(MyDbcontext myDbcontext, IWebHostEnvironment webHost)
        {
            _myDbcontext = myDbcontext;
            _webHost = webHost;
        }

        // GET: TeamControllers
        public async Task<ActionResult> Index(MyDbcontext myDbcontext)
        {
            List<Team> teams = await _myDbcontext.Teams.OrderByDescending(s => s.Id).ToListAsync();
            return View(teams);
        }

        // GET: TeamControllers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamControllers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM createTeam)
        {
            if (!ModelState.IsValid) { return View(createTeam); }
            if (!createTeam.Image.ContentType.Contains("image/")) 
            {
                ModelState.AddModelError("Image", ErrorMessages.FileMustBeImageType);
            }
            if (createTeam.Image.Length / 1024 > 200)
            {
                ModelState.AddModelError("Image", ErrorMessages.FileSizeMustBe200kb);
            }
            string rootpath = Path.Combine(_webHost.WebRootPath, "assets", "img");
            string fileName = Guid.NewGuid().ToString()+createTeam.Image.FileName;
            using (FileStream fileStream = new FileStream(Path.Combine(rootpath,fileName),FileMode.Create))
            {
                await createTeam.Image.CopyToAsync(fileStream);
            }
            Team team = new Team
            {
                Name= createTeam.Name,
                Surname = createTeam.Surname,
                Position = createTeam.Position,
                FacebookLink = createTeam.FacebookLink,
                InstagramLink = createTeam.InstagramLink,
                TwitterLink = createTeam.TwitterLink,
                ImagePath = fileName,
            };
            await _myDbcontext.AddAsync(team);
            await _myDbcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
      
        public async Task<IActionResult> Update(int id)
        {
            Team team = await _myDbcontext.Teams.FindAsync(id);
            if (team == null) return NotFound();
            UpdateTeamVM updateTeamVM = new UpdateTeamVM()
            {
                Id = id,
                Name = team.Name,
                Surname = team.Surname,
                Position = team.Position,
                FacebookLink = team.FacebookLink,
                InstagramLink = team.InstagramLink,
                TwitterLink = team.TwitterLink
            };
            return View(updateTeamVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTeamVM teamVM)
        {
            if (!ModelState.IsValid) { return View(teamVM); }
            if (!teamVM.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", ErrorMessages.FileMustBeImageType);
            }
            if (teamVM.Image.Length / 1024 > 200)
            {
                ModelState.AddModelError("Image", ErrorMessages.FileSizeMustBe200kb);
            }
            string rootpath = Path.Combine(_webHost.WebRootPath, "assets", "img");
            Team team = await _myDbcontext.Teams.FindAsync(teamVM.Id);
            string filepath = Path.Combine(rootpath, team.ImagePath);

            if (System.IO.File.Exists(filepath)) { System.IO.File.Delete(filepath); }

            string fileName = Guid.NewGuid().ToString() + teamVM.Image.FileName;
            using (FileStream fileStream = new FileStream(Path.Combine(rootpath, fileName), FileMode.Create))
            {
                await teamVM.Image.CopyToAsync(fileStream);
            }
            team.Name = teamVM.Name;
            team.Surname = teamVM.Surname;
            team.Position = teamVM.Position;
            team.FacebookLink = teamVM.FacebookLink;
            team.InstagramLink = teamVM.InstagramLink;
            team.TwitterLink = teamVM.TwitterLink;
            
            await _myDbcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }





        // GET: TeamControllers/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Team team = await _myDbcontext.Teams.FindAsync(id);
            if(team == null) return NotFound();
            string filePath = Path.Combine(_webHost.WebRootPath, "assets", "img", team.ImagePath);
            if (System.IO.File.Exists(filePath)) { System.IO.File.Delete(filePath); }
            _myDbcontext.Teams.Remove(team);
            await _myDbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));   
        }

     
    }
}
