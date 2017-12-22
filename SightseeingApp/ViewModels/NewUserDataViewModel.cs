using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SightseeingApp.ViewModels
{
    public class NewUserDataViewModel
    {
      public UserData userData { get; set; }
    
       public IEnumerable<Category> GetInterests { get; set; }
    
       public Category[] CategoriesList { get; set; }

       public IEnumerable<TransportMode> GetTransportMode { get; set; }

        public TransportMode[] TransportModeList { get; set; }


    }
}