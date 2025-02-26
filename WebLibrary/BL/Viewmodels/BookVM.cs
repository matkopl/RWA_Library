﻿using BL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Viewmodels
{
    public class BookVM
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Genre Id")]
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; }

        [Display(Name = "Location")]
        public List<int> SelectedLocationIds { get; set; } = new(); 

        [Display(Name = "Available Locations")]
        public List<LocationVM> AvailableLocations { get; set; } = new(); 
    }

}
