using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Viewmodels
{
    public class LocationVM
    {
        public int Id { get; set; }

        [ValidateNever]
        public string Name { get; set; }

        public bool IsChecked { get; set; }

        public bool IsAvailable { get; set; }
    }
}
