using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service.AlgorithmService
{
    public class Chromosome
    {
        public List<RandomSight> List {get; set;}
        public int ChromosomeLength { get; set; }
        public double Fitness { get; set; }

        public bool IsElite { get; set; }

        public Chromosome()
        {
            List<RandomSight> List = new List<RandomSight>();
            ChromosomeLength = 0;
            Fitness = 0;
            IsElite = false;
        }

       

    }
}