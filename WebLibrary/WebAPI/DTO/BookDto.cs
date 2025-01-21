using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class BookDto
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string Author { get; set; } 

        public string? Description { get; set; }

        public bool IsAvailable { get; set; }

        public string? Genre  { get; set; }

        public int GenreId  { get; set; }

    }
}
