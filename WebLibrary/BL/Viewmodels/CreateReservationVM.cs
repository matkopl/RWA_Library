using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Viewmodels
{
    public class CreateReservationVM
    {
        public int BookId { get; set; }

        [Display(Name = "Book")]
        public string Book { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; } 

        [Required(ErrorMessage = "Please select a location.")]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [ValidateNever]
        public List<LocationVM> Locations { get; set; } = new();
    }
}
