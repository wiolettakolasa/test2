using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service
{
    public class DistanceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public double distance { get; set; }
        public double time { get; set; }
    }
}