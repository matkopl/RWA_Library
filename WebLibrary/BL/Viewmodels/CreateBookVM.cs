using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Viewmodels
{
    public class CreateBookVM
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable => Locations?.Any(l => l.IsChecked) == true;

        [Required(ErrorMessage = "Genre is required")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Display(Name = "Available Locations")]
        public List<LocationVM> Locations { get; set; } = new List<LocationVM>();
    }
}
