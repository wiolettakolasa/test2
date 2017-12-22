using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service.AlgorithmService
{
    public class Result
    {
        public Chromosome BestOrder { get; set; }
        public double TotalTime { get; set; }

        public double TotalCost { get; set; }

        public double TotalFitness { get; set; }

    }
}