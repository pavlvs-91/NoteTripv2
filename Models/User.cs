using System.ComponentModel.DataAnnotations;

namespace NoteTrip.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Login")]
        public required string Login { get; set; }

        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public required string Email { get; set; }

        public ICollection<Country>? Countries { get; set; }
    }
}
