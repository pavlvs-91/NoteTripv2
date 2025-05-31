using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTrip.Models
{
    public class TouristAttraction
    {
        [Key]
        [Display(Name = "ID")]
        public required int Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Category")]
        public required string Category { get; set; }

        [Display(Name = "Description")]
        public required string Description { get; set; }

        [Display(Name = "Price")]
        public required decimal Price { get; set; }

        [Display(Name = "Visited")]
        public required bool Visited { get; set; }

        [Display(Name = "Rate /10")]
        public required int Rate { get; set; }

        [Required]
        [Display(Name = "City Id")]
        public required int CityId { get; set; }

        [ForeignKey("CityId")]
        public required City City { get; set; }
    }
}