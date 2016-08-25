using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OldNewTeamProject.Models
{
    public class Evaluation
    {
        public Evaluation()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Language { get; set; }

        public DateTime Date { get; set; }
        public ApplicationUser Author { get; set; }

        [Required]
        public int LanguageId { get; set; }
    }
}