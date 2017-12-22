using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SightseeingApp.Models
{
    public class UserData
    {
        public int Id { get; set; }

        [Display(Name = "Koszt")]
        public double Cost { get; set; }

        [Display(Name = "Czas")]
        public double Time { get; set; }

        public Category Category { get; set; } //navigation property

        [Display(Name = "Zainteresowania")]
        public byte CategoryId { get; set; }

        public List<byte> Interests { get; set; }

        
        public TransportMode TransportMode { get; set; }

        [Display(Name = "Środek transportu")]
        public byte TrasportModeId { get; set; }
        

    }
}