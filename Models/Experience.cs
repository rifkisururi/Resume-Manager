﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeManager.Models
{
    public class Experience
    {
        public Experience(){}
        
        [Key]
        public int ExperienceId { get; set; }

        [ForeignKey("Applicant")] // very important
        public int ApplicationId { get; set; }
        public virtual Applicant Applicant { get; private set; } // very important
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        [Required]
        public int YearsWorked { get; set; }


    }
}
