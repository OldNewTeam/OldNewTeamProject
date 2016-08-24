using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OldNewTeamProject.Models
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Language { get; set; }

        
        public string AuthorId { get; set; }

        [Required]
        public int LanguageId { get; set; }
    }
}