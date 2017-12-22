using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service.AlgorithmService
{
    public class Generation
    {
        public List<Population> List { get; set; }
        public Chromosome FirstFittest { get; set; }

        public Chromosome LastFittest { get; set; }

       public Generation()
        {
            List = new List<Population>();
            //FirstFittest = List[0].Solutions.OrderByDescending(c => c.Fitness).First();
            //LastFittest = List[List.Count-1].Solutions.OrderByDescending(c => c.Fitness).First();

        }

    }
}