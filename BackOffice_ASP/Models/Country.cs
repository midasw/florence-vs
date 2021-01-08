using System.ComponentModel.DataAnnotations;

namespace BackOffice_ASP.Models
{
    public class Country
    {
        private string code;

        [Key]
        public string Code
        {
            get
            {
                return code.ToLower();
            }
            set
            {
                code = value;
            }
        }
        public string Name { get; set; }
    }
}
