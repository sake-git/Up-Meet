using up_meet_api.Entities;

namespace up_meet_api.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Location { get; set; }

        public DateTime? EventDateTime { get; set; }

        public string? ImgUrl { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public bool? KidsAllowed { get; set; }

        public int? Duration { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedByUser { get; set; }

        public bool isFavourite { get; set; }

    }
}
