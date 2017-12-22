using GAF;
using SightseeingApp.Models;
using SightseeingApp.Service;
using SightseeingApp.Service.AlgorithmService;
using SightseeingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SightseeingApp.Controllers
{
    public class ResultController : Controller
    {
        private IAppRepository _sightRepository;
        private AlghService _alghService;
        private AlgorithmService _test;
        private ComputingDistService _distService;
        

        public ResultController()
        {
            _sightRepository = new AppRepository();
            _alghService = new AlghService();
            _distService = new ComputingDistService();
             _test = new AlgorithmService();

        }

        public async System.Threading.Tasks.Task<ActionResult> RandomDbAsync()
        {
          /*int number = 500;
            var randomDb = new RandomDataBase();
            randomDb.GetRandomDataBase(number); */

            var ud = new UserData();
            ud.Cost = 500;
            ud.Time = 20;
            ud.Interests = new List<byte>();
            ud.Interests.Add(1);
            ud.Interests.Add(2);
            ud.Interests.Add(3);
            ud.TrasportModeId = 1;

            _test.userData = ud;

         var population = _test.CreatePopulationAsync(ud);

          var ga = _test.CreateGeneticAlgorithm(population);

            var chromosome1 = _test.CreateChromosomeAsync(ud);

            double cost = 0;
            double time = 0;

            foreach (var gene in chromosome1.List)
            {

                var sight = (RandomSight)gene;

                cost += sight.Cost;
                time += sight.Time;

                System.Diagnostics.Debug.WriteLine("Chromosom1: Zabytek: {0}, Atrakcyjność: {1}, Koszt: {2}, Czas: {3}", sight.Id, sight.Attractivenes, sight.Cost, sight.Time);

            }
            System.Diagnostics.Debug.WriteLine("Koszt: {0}, Czas: {1}", cost, time);

            var chromosome2 = _test.CreateChromosomeAsync(ud);

            cost = 0;
            time = 0;

            foreach (var gene in chromosome2.List)
            {

                var sight = (RandomSight)gene;

                cost += sight.Cost;
                time += sight.Time;

                System.Diagnostics.Debug.WriteLine("Chromosom2: Zabytek: {0}, Atrakcyjność: {1}, Koszt: {2}, Czas: {3}", sight.Id, sight.Attractivenes, sight.Cost, sight.Time);

            }
            System.Diagnostics.Debug.WriteLine("Koszt: {0}, Czas: {1}", cost, time);

            var child = _test.CrossoverOperatorDoublePoint(chromosome1, chromosome2);

            cost = 0;
            time = 0;

            foreach (var gene in child.List)
            {

                var sight = (RandomSight)gene;

                cost += sight.Cost;
                time += sight.Time;

                System.Diagnostics.Debug.WriteLine("Child: Zabytek: {0}, Atrakcyjność: {1}, Koszt: {2}, Czas: {3}", sight.Id, sight.Attractivenes, sight.Cost, sight.Time);

            }
            System.Diagnostics.Debug.WriteLine("Koszt: {0}, Czas: {1}", cost, time);



           


            //var fitness = await _test.CalculateFitnessAsync(chromosome, ud);
          
           
            /*

             // var population = _test.CreatePopulationAsync(ud);
            int i = 0;

            foreach(var sol in population.Solutions)
            {
                
                    i++;
                    System.Diagnostics.Debug.WriteLine("Chromosom: {0}, Przed: {1}", i, sol.Genes.Count());
                    _test.CheckChromosomeLength(sol, 10);
                    System.Diagnostics.Debug.WriteLine("Chromosom: {0}, Po: {1}", i, sol.Genes.Count());
    
            }

            

            var ga = _test.CreateGeneticAlgorithm(population);

              var fittest1 = ga.Population.GetTop(1)[0];


              System.Diagnostics.Debug.WriteLine("Wartość funkcji celu: {0} ", fittest1.Fitness);

              double cost2 = 0;
              double time2 = 0;

              foreach (var gene in fittest1.Genes)
              {

                  var sight = (RandomSight)gene.ObjectValue;

                  cost2 += sight.Cost;
                  time2 += sight.Time;

                  System.Diagnostics.Debug.WriteLine("Zabytek: {0}, Atrakcyjność: {1}, Koszt: {2}, Czas: {3}",  sight.Id, sight.Attractivenes, sight.Cost, sight.Time);

              }
              System.Diagnostics.Debug.WriteLine("Koszt: {0}, Czas: {1}", cost2, time2); */


            return View();
        }

        // GET: Result
        public ActionResult ShowResult(NewUserDataViewModel viewModel)
        {
            return View(viewModel);
        }

        public async System.Threading.Tasks.Task<ActionResult> TestAsync()
        {
            /* Sight sight1 = new Sight()
             {
                 Name = "Kraków",
                 Latitude = 50.0592384338379,
                 Longtitude = 19.9399833679199
            };


             Sight sight2 = new Sight()
             {
                 Name = "Kraków2",
                 Latitude = 50.08455592384338379,
                 Longtitude = 19.8399833679199
             };

             var result = await _distService.GetDistanceWalkingResult(sight1, sight2);

             var dist = result.distance;
             var time = result.time;


             var ud = new UserData();
             ud.Cost = 75;
             ud.Time = 3;
             ud.CategoryId = 1;
             ud.TrasportModeId= 1;
             _alghService.userData = ud;

             var chromosome = await _alghService.CreateChromosomeAsync(ud);

             var population = await _alghService.CreatePopulationAsync(ud);
             var fitness = _alghService.CalculateFitness(chromosome);
             var ag = _alghService.CreateGeneticAlgorithm(population);
             var fittest = ag.Population.GetTop(1)[0];
             var ag2 = _alghService.CreateGeneticAlgorithm(ag.Population); */

            var ud = new UserData();
            ud.Cost = 300;
            ud.Time = 10;
             ud.TrasportModeId = 1;
            _alghService.userData = ud;
            
            var chromosome = _alghService.CreateChromosome();
            var initialPopulation = _alghService.CreateInitialPopulation();

          
            await _alghService.EvaluateFirstSolutionsAsync(initialPopulation, ud);

            var population = _alghService.p;

            var ga = _alghService.CreateGeneticAlgorithm(population);

            var fittest1 = ga.Population.GetTop(1)[0];

            System.Diagnostics.Debug.WriteLine("Wartość funkcji celu: {0} ", fittest1.Fitness);

            foreach (var gene in fittest1.Genes)
            {
                var sight = (Sight)gene.ObjectValue;
                System.Diagnostics.Debug.WriteLine("Zabytek: {0}, Atrakcyjność: {1}", sight.Name, sight.Attractivenes);
            }

            var fittest2 = ga.Population.GetBottom(1)[0];

            System.Diagnostics.Debug.WriteLine("Wartość funkcji celu: {0} ", fittest2.Fitness);

            foreach (var gene in fittest2.Genes)
            {
                var sight = (Sight)gene.ObjectValue;
                System.Diagnostics.Debug.WriteLine("Zabytek: {0}, Atrakcyjność: {1}", sight.Name, sight.Attractivenes);
            }

            //await result.GetResultAsync(ud); nie działa lista
           // result.AvarageAttractivenes = result.CalculateAvarageAttractivenes();

            return View();
        }

        public ActionResult ShowRoute()
        {
            return View();
        }

        public ActionResult ModeTravel()
        {
            return View();
        }
         
        public ActionResult ShowMap()
        {
            return View();
        }

        public ActionResult CountDistance()
        {
            return View();
        }

    }
}