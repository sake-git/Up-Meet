using System;
using System.Collections.Generic;

namespace up_meet_api.Entities;

public partial class User
{
    public int Id { get; set; }

    public string LoginId { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<FavouriteEvent> FavouriteEvents { get; set; } = new List<FavouriteEvent>();
}
