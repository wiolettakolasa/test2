using SightseeingApp.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Models
{
    public class Result
    {
        public AlghService _alghService { get; set; }
        public IEnumerable<Sight> SightList { get; set; }
        public double totalTime { get; set; }
        public double totalCost { get; set; }
        public double AvarageAttractivenes { get; set; }

        public Result()
        {
            _alghService = new AlghService();

        }

        public double CalculateAvarageAttractivenes()
        {

            double avarage = 0;

            foreach (Sight sight in SightList)
            {
                avarage += sight.Attractivenes;
            }

            avarage = avarage / SightList.Count();

            return avarage;
        }

        public async System.Threading.Tasks.Task GetResultAsync(UserData userData)
        {
            //Get Random Population
            var initialPopulation = _alghService.CreateInitialPopulation();

            //Evaluate Solutions
            await _alghService.EvaluateFirstSolutionsAsync(initialPopulation, userData);

            var population = _alghService.p;

            //Genetic Algorithm
            var ga = _alghService.CreateGeneticAlgorithm(population);

            var best = ga.Population.GetTop(1)[0];

            SightList = new List<Sight>();

            foreach (var gene in best.Genes)
            {
                var sight = (Sight)gene.ObjectValue;

                if (sight != null)
                {
                    SightList.ToList().Add(sight);
                    //Add transport time&cost
                    totalTime += sight.Time;
                    totalCost += sight.Cost;
                }

            }


        }
    }
}