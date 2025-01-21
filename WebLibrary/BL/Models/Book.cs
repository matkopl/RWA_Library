using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BL.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsAvailable { get; set; }

    public int GenreId { get; set; }

    public virtual ICollection<BookLocation> BookLocations { get; } = new List<BookLocation>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; } = new List<Reservation>();
}
