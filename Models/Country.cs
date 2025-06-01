using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTrip.Models
{
    public class Country
    {
        [Key]
        [Display(Name = "ID")]
        public required int Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Continent")]
        public required string Continent { get; set; }

        [Display(Name = "Language")]
        public required string Language { get; set; }

        [Display(Name = "Currency")]
        public required string Currency { get; set; }

        [Display(Name = "Capital City")]
        public required string Capital { get; set; }

        [Display(Name = "User Login")]
        public string? UserLogin { get; set; }

        [ForeignKey("UserLogin")]
        public User? User { get; set; }

        public ICollection<Region>? Regions { get; set; }
    }
}