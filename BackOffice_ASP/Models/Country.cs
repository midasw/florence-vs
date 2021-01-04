using System.ComponentModel.DataAnnotations;

namespace BackOffice_ASP.Models
{
    public class Country
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
