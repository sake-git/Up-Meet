using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpMeet.Api.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime EventDateTime { get; set; }
        public decimal? Price { get; set; }

        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public ICollection<FavoriteEvent>? FavoriteEvents { get; set; }
    }
}
