using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTrip.Models
{
    public class Region
    {
        [Key]
        [Display(Name = "ID")]
        public required int Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Location")]
        public required string Location { get; set; }

        [Display(Name = "Description")]
        public required string Description { get; set; }

        [Required]
        [Display(Name = "Country Id")]
        public required int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public required Country Country { get; set; }
        
        public ICollection<City>? Cities { get; set; }
    }
}