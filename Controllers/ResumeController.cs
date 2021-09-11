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
    }
}
