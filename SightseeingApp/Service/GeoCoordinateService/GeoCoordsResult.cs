﻿namespace SightseeingApp.Service
{
    public class GeoCoordsResult
    {
        public bool Success { get; set; }
        public string Message {get; set;}

        public double Longtitude { get; set; }

        public double Latitude { get; set; }
    }
}