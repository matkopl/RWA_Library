using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Viewmodels
{
    public class ReservationVM
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Book { get; set; }

        [ValidateNever]
        [Display(Name = "Genre")]
        public string Genre { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Reservation Date")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDate { get; set; }
    }
}
