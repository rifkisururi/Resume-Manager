using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeManager.Models
{
    // deklaresi kolom kolom dalam table Applicant
    public class Applicant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [Range(25, 55, ErrorMessage = " Valid dong bos")]
        [DisplayName("Age In Years")]
        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Qualification { get; set; }

        [Required]
        [Range(1, 25, ErrorMessage = " Valid dong bos input nya")]
        [DisplayName("Total Experience")]
        public int TotalExperience { get; set; }

        public virtual List<Experience> Experiences { get; set; } = new List<Experience>();

        public string PhotoUrl { get; set; }
        [DisplayName("Profile Photo")]
        [Required(ErrorMessage = "Tambahin foto dong bos")]
        [NotMapped]

        public IFormFile ProfilePhoto { get; set; }



    }
}
