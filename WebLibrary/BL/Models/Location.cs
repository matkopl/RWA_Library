using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookLocation> BookLocations { get; } = new List<BookLocation>();
}
