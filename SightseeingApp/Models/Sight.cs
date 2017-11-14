using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SightseeingApp.Models
{
    public class Sight
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public double Time { get; set; }

        [Required]
        public double CoordinateX { get; set; }

        [Required]
        public double CoordinateY { get; set; }


        public Category Category { get; set; } //navigation property

        public byte CategoryId { get; set; }

        [Required]
        public int Attractivenes { get; set; }
    }
}