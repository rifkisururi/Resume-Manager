using Microsoft.AspNetCore.Mvc;
using ResumeManager.Models;
using ResumeManager03.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace ResumeManager03.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeDbContext _context;
        private readonly IWebHostEnvironment _webHost; // upload file

        public ResumeController(ResumeDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            List<Applicant> applicants;
            applicants = _context.Applicants.ToList();

            return View(applicants);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experiences.Add(new Experience() { ExperienceId = 1 });
            return View(applicant);
        }

        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {
            string uniqueFileName = GetUploadedFileName(applicant);
            applicant.PhotoUrl = uniqueFileName;
            foreach (Experience experience in applicant.Experiences)
            {
                if (experience.CompanyName == null || experience.CompanyName.Length == 0)
                    applicant.Experiences.Remove(experience);
            }
            _context.Add(applicant);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        private string GetUploadedFileName(Applicant applicant) {
            string uniqueFileName = null;
            if (applicant.ProfilePhoto != null) {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using ( var fileStream = new FileStream(filePath, FileMode.Create)) {
                    applicant.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Applicants
                .FirstOrDefaultAsync(m => m.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        public ActionResult Edit(int id)
        {
            //here, get the student from the database in the real application

            //getting a student from collection for demo purpose
            var data = _context.Applicants.Where( a=> a.Id == id).FirstOrDefault();

            return View(data);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Applicants
                .Include(c => c.Experiences)
                .FirstOrDefaultAsync(m => m.Id == id);
            _context.Remove(data);
            _context.SaveChanges();

            if (data == null)
            {
                return NotFound();
            }

            return RedirectToAction("index");
        }
    }
}
