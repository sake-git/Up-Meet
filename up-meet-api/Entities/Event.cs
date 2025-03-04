using System;
using System.Collections.Generic;

namespace up_meet_api.Entities;

public partial class Event
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

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<FavouriteEvent> FavouriteEvents { get; set; } = new List<FavouriteEvent>();
}
