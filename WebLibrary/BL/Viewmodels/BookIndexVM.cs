using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Viewmodels
{
    public class BookIndexVM
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Author { get; set; } 
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public string Genre { get; set; }
    }
}
