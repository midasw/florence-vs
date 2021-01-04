using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackOffice_ASP.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public bool Published { get; set; } = false;

        public ApplicationUser Author { get; set; }
        public ApplicationUser Editor { get; set; }
    }
}
