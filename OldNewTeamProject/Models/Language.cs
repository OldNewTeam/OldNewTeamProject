using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OldNewTeamProject.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public ApplicationUser Uploader { get; set; }

        public List<Evaluation> Posts { get; set; }

        public double ratio { get; set; }

        public int positives { get; set; }

        public int negatives { get; set; }
    }
}