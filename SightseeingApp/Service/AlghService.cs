using GAF;
using GAF.Extensions;
using GAF.Operators;
using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service
{
    //Funkcja kary
    //Operatory mutacji
    //Baza danych
    //Koszt dojazdu

    public class AlghService
    {
        private IAppRepository _sightRepository;
        private ComputingDistService _distanceService;
        public UserData userData;
        public Population p;

        //Alghoritm Settings
        const int populationSize = 50;
        const int chromosomeLength = 20;
        const double crossoverProbability = 0.85;
        const double mutationProbability = 0.02;
        const int elitismPercentage = 8;

        //Operators
        //create the elite operator
        Elite elite = new Elite(elitismPercentage);

        //create the crossover operator
        Crossover crossover = new Crossover(crossoverProbability)
        {
            CrossoverType = CrossoverType.DoublePointOrdered,
            AllowDuplicates = true,
        };

        //create the mutation operator
        SwapMutate mutate = new SwapMutate(mutationProbability);


        public AlghService()
        {
            userData = new UserData();
            _sightRepository = new AppRepository();
            _distanceService = new ComputingDistService();
            p = new Population();


        }

        public Chromosome CreateChromosome()
        {
            var chromosome = new Chromosome();
            var sights = _sightRepository.GetSights();

            foreach (var sight in sights)
            {
                chromosome.Genes.Add(new Gene(sight));
            }

            return chromosome;
        }

        //Create Chromosome and check time and cost
        public async System.Threading.Tasks.Task<Chromosome> CreateChromosomeAsync(UserData userData)
        {
            var chromosome = new Chromosome();
            var sights = _sightRepository.GetSights();

            double totalCost = 0;
            double totalTime = 0;
            Sight previousSight = null;

            foreach (var sight in sights)
            {
                var currentSight = sight;

                if (chromosome.Count < chromosomeLength && totalCost <= userData.Cost && totalTime <= userData.Time)
                {
                    totalCost += sight.Cost;
                    totalTime += sight.Time;

                    if (previousSight != null)
                    {
                        if (userData.TrasportModeId == 1)
                        {
                            var result = await _distanceService.GetDistanceDrivingResult(previousSight, currentSight);
                            totalTime += result.time;
                        }
                        else if (userData.TrasportModeId == 2)
                        {
                            var result = await _distanceService.GetDistanceWalkingResult(previousSight, currentSight);
                            totalTime += result.time;
                        }

                    }


                    chromosome.Genes.Add(new Gene(sight));
                }

                previousSight = currentSight;
            }

            return chromosome;
        }

        //Create Initial Population

        public Population CreateInitialPopulation()
        {
            var initialPopulation = new Population()
            {
                ReEvaluateAll = true,
                LinearlyNormalised = true,

            };


            for (var p = 0; p < populationSize; p++)
            {
                var chromosome = CreateChromosome();

                chromosome.Genes.ShuffleFast();

                initialPopulation.Solutions.Add(chromosome);
            }

            return initialPopulation;
        }

        //Create Population

        public async System.Threading.Tasks.Task<Population> CreatePopulationAsync(UserData userData)
        {
            var population = new Population()
            {
                ReEvaluateAll = true,
                LinearlyNormalised = true,

            };



            for (var p = 0; p < populationSize; p++)
            {
                var chromosome = await CreateChromosomeAsync(userData);

                chromosome.Genes.ShuffleFast();

                population.Solutions.Add(chromosome);
            }



            return population;
        }

        //Calculates transport time between sights in chromosome
        public async System.Threading.Tasks.Task<double> CalculateTransportTimeAsync(Chromosome chromosome, UserData userData)
        {
            double transportTime = 0;
            Sight previousSight = null;

            foreach (var gene in chromosome.Genes)
            {
                var sight = (Sight)gene.ObjectValue;

                var currentSight = sight;

                //Adding the time of transport
                if (previousSight != null)
                {
                    if (userData.TrasportModeId == 1)
                    {
                        var result = await _distanceService.GetDistanceDrivingResult(previousSight, currentSight);
                        transportTime += result.time;
                    }
                    else if (userData.TrasportModeId == 2)
                    {
                        var result = await _distanceService.GetDistanceWalkingResult(previousSight, currentSight);
                        transportTime += result.time;
                    }
                    previousSight = currentSight;
                }

            }
            return transportTime;
        }

        //Calculate totalTime and totalCost in chromosome
        public double CalculateTime(Chromosome chromosome)
        {
            double totalTime = 0;

            foreach (var gene in chromosome.Genes)
            {
                var sight = (Sight)gene.ObjectValue;
                totalTime += sight.Time;

            }

            return totalTime;
        }

        public double CalculateCost(Chromosome chromosome)
        {
            double totalCost = 0;

            foreach (var gene in chromosome.Genes)
            {
                var sight = (Sight)gene.ObjectValue;
                totalCost += sight.Cost;

            }

            return totalCost;
        }

        //Evauates solution of initial population
        public async System.Threading.Tasks.Task EvaluateFirstSolutionsAsync(Population initialPopulation, UserData userData)
        {
            double totalTime = 0;
            double totalCost = 0;
            Population newPopulation = new Population();

            foreach (var chromosome in initialPopulation.Solutions)
            {
                //Index of gene
                int index = 0;

                //Total time
                totalTime = await CalculateTransportTimeAsync(chromosome, userData);
                totalTime += CalculateTime(chromosome);

                //TotalCost
                totalCost = CalculateCost(chromosome);

                while (!(totalCost <= userData.Cost && totalTime <= userData.Time))
                {

                    chromosome.Genes.RemoveAt(index);

                    //New Cost
                    totalCost = CalculateCost(chromosome);

                    //New Time
                    totalTime = await CalculateTransportTimeAsync(chromosome, userData);
                    totalTime += CalculateTime(chromosome);
                }
                index = 0;
                //Add changed chromosome to new population
                chromosome.Genes.ShuffleFast();

                newPopulation.Solutions.Add(chromosome);

            }

            p = newPopulation;
        }





        //Computing fitness function
        public double CalculateFitness(Chromosome chromosome)
        {
            double fitness = 0;

            foreach (var gene in chromosome.Genes)
            {
                var sight = (Sight)gene.ObjectValue;

                if (userData.CategoryId == sight.CategoryId)
                    fitness += sight.Attractivenes;
                else
                    fitness += 0.1*sight.Attractivenes;
            }   

            return fitness / 1000;
        }

        
        public GeneticAlgorithm CreateGeneticAlgorithm(Population population)
        {
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(Terminate);

            return ga;
        }

        public static bool Terminate(Population population,
            int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 400;
        }

        private static double DegreesToRadians(double deg)
        {
            return deg * (System.Math.PI / 180);
        }

        public static async System.Threading.Tasks.Task<double> CalculateDistanceAsync(Chromosome chromosome)
        {
            var distanceToTravel = 0.0;
            Sight previousSight = null;
            var _distanceService = new ComputingDistService();

            //run through each sight in the order specified in the chromosome
            foreach (var gene in chromosome.Genes)
            {
                var currentSight = (Sight)gene.ObjectValue;

                if (previousSight != null)
                {
                    var result = await _distanceService.GetDistanceDrivingResult(currentSight, previousSight);

                    distanceToTravel += result.distance;
                }

                previousSight = currentSight;
            }

            return distanceToTravel;
        }

        public static async System.Threading.Tasks.Task<double> CalculateTimeAsync(Chromosome chromosome)
        {
            var timeToTravel = 0.0;
            Sight previousSight = null;
            var _timeService = new ComputingDistService();

            //run through each sight in the order specified in the chromosome
            foreach (var gene in chromosome.Genes)
            {
                var currentSight = (Sight)gene.ObjectValue;

                if (previousSight != null)
                {
                    var result = await _timeService.GetDistanceDrivingResult(currentSight, previousSight);

                    timeToTravel += result.time;
                }

                previousSight = currentSight;
            }

            return timeToTravel;
        }

        public static async System.Threading.Tasks.Task<double> CalculateTimeTwoPointsAsync(Sight sight1, Sight sight2)
        {
            var _timeService = new ComputingDistService();
            var _alghService = new AlghService();

            //run through each sight in the order specified in the chromosome
           if(_alghService.userData.TrasportModeId==1)
            {
                var result = await _timeService.GetDistanceDrivingResult(sight1, sight2);
                return result.time;
            }

            else if (_alghService.userData.TrasportModeId == 2)
            {
                var result = await _timeService.GetDistanceWalkingResult(sight1, sight2);
                return result.time;
                
            }
           else
            {
                var result = await _timeService.GetDistanceTransitResult(sight1, sight2);
                return result.time;
               
            }
        }

        public static async System.Threading.Tasks.Task<double> CalculateDistanceTwoPointsAsync(Sight sight1, Sight sight2)
        {
            var distanceToTravel = 0.0;
            var _distService = new ComputingDistService();
            var _alghService = new AlghService();

            //run through each sight in the order specified in the chromosome

            if (_alghService.userData.TrasportModeId == 1)
            {
                var result = await _distService.GetDistanceDrivingResult(sight1, sight2);
                return result.distance;
               
            }

            else if (_alghService.userData.TrasportModeId == 2)
            {
                var result = await _distService.GetDistanceWalkingResult(sight1, sight2);
                return result.distance;
              
            }
            else 
            {
                var result = await _distService.GetDistanceTransitResult(sight1, sight2);
                return result.distance;
                
            }
           
        }




    }
}