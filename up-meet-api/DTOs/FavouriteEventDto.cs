namespace up_meet_api.DTOs
{
    public class FavouriteEventDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int EventId { get; set; }
        public EventDto? Event { get; set; }
    }
}
