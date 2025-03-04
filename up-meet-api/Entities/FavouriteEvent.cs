using System;
using System.Collections.Generic;

namespace up_meet_api.Entities;

public partial class FavouriteEvent
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
