using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Models
{
    public class UserData
    {
        public int Id { get; set; }

        public double Cost { get; set; }

        public double Time { get; set; }

        public Category Category { get; set; }

        public byte CategoryId { get; set; }
    }
}