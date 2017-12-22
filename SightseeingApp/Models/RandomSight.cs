using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SightseeingApp.Models
{
    public class RandomSight
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Koszt")]
        public double Cost { get; set; }

        [Required]
        [Display(Name = "Czas")]
        public double Time { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longtitude { get; set; }

        public Category Category { get; set; } //navigation property

        [Display(Name = "Kategoria")]
        public byte CategoryId { get; set; }

        [Required]
        public int Attractivenes { get; set; }
    }
}
