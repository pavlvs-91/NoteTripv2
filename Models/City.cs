using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTrip.Models
{
    public class City
    {
        [Key]
        [Display(Name = "ID")]
        public required int Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Region Id")]
        public int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region? Region { get; set; }
        
        public ICollection<TouristAttraction>? TouristAttractions { get; set; }
    }
}