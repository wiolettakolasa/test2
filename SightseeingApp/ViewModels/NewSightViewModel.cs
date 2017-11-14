using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.ViewModels
{
    public class NewSightViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Sight Sight { get; set; }
    }
}