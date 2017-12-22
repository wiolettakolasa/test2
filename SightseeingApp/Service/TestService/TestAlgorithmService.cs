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

    public class TestAlgorithmSerivce
    {
        private IAppRepository _sightRepository;
        private ComputingDistService _distanceService;
        public UserData userData;


        //Alghoritm Settings
        const int populationSize = 200;
        const int chromosomeLength = 10;
        const double crossoverProbability = 0.95;
        const double mutationProbability = 0.02;
        const int elitismPercentage = 20;

        //Operators
        //create the elite operator
        Elite elite = new Elite(elitismPercentage);

        //create the crossover operator
        Crossover crossover = new Crossover(crossoverProbability)
        {
            CrossoverType = CrossoverType.SinglePoint,
            AllowDuplicates = true,
            ReplacementMethod = ReplacementMethod.GenerationalReplacement
        };

        //create the mutation operator
        SwapMutate mutate = new SwapMutate(mutationProbability);


        public TestAlgorithmSerivce()
        {
            userData = new UserData();
            _sightRepository = new AppRepository();
            _distanceService = new ComputingDistService();

        }


        //Convert degrees to radians
        public static double DegreesToRadians(double deg)
        {
            return deg * (System.Math.PI / 180);
        }

        //Count Distance having Longtitude and Latitude
        public double CountDistance(RandomSight sight1, RandomSight sight2)
        {
            double distance = 0;
            double value1 = DegreesToRadians(sight1.Latitude) - DegreesToRadians(sight2.Latitude);
            double value2 = DegreesToRadians(sight1.Longtitude) - DegreesToRadians(sight2.Longtitude);
            double power = 2;

            value1 = System.Math.Pow(value1, power);
            value2 = System.Math.Pow(value2, power);

            distance = System.Math.Sqrt(value1 + value2);


            return distance;
        }

        //Check if in a chromosome genes duplicate
       public bool CheckIfGenesDuplicate(Chromosome chromosome, RandomSight sight)
        {
            bool result = false;

            foreach (var gene in chromosome.Genes)
            {
                var sight1 = (RandomSight)gene.ObjectValue;

                if (sight1.Attractivenes != 0)
                {
                    if (sight == sight1)
                    {
                        result = true;

                        return true;
                    }
                }

            }

            return result;
        }

        //Get Random sight
        public RandomSight FindNewSight()
        {
            RandomSight sight;
            var rnd = new Random();
            var sights = _sightRepository.GetRandomSights();

            sight = sights.ElementAt(rnd.Next(1, 200));

            return sight;
        }

        public bool CheckIfTimeAndCostFits(UserData ud, RandomSight previousSight, RandomSight sight, double totalCost, double totalTime)
        {
            var result = false;
            totalCost += sight.Cost;
            totalTime += sight.Time;

            //It does not count the distace between to sights
            if (previousSight == null || previousSight.Attractivenes == 0)
            {
                if (totalCost < userData.Cost && totalTime < userData.Time)
                {
                    result = true;
                }
                else
                    totalCost -= sight.Cost;
                    totalTime -= sight.Time;
                result = false;
            }
            else
            {
                totalTime += CountDistance(previousSight, sight);

                if (totalCost < userData.Cost && totalTime < userData.Time)
                {
                    result = true;
                }
                else
                    totalCost -= sight.Cost;
                    totalTime -= sight.Time;
                    totalTime -= CountDistance(previousSight, sight);

                result = false;
            }


            return result;
        }

       public void SetSightToNull(RandomSight sight)
        {
            sight.Attractivenes = 0;
            sight.Latitude = 0;
            sight.Latitude = 0;
            sight.Time = 0;
            sight.Cost = 0;
            sight.CategoryId = 0;
        }

        public Chromosome CreateOneChromosome(UserData userData)
        {
            var chromosome = new Chromosome();
            var rnd = new Random();
            double totalTime = 0;
            double totalCost = 0;
            int count = 0;
            var prevoiusSight = new RandomSight();

            while (count<50)
            {
                var currentSight = FindNewSight();
                    if(prevoiusSight==null)
                {
                    totalTime += currentSight.Time;
                    totalCost += currentSight.Cost;

                    if (totalCost < userData.Cost && totalTime < userData.Time)
                    {
                        chromosome.Add(new Gene(currentSight));

                    }
                    else
                    {
                        totalTime -= currentSight.Time;
                        totalCost -= currentSight.Cost;
                    }
                }
                else
                {
                    totalTime += CountDistance(prevoiusSight, currentSight);
                    totalTime += currentSight.Time;
                    totalCost += currentSight.Cost;

                    if (totalCost < userData.Cost && totalTime < userData.Time)
                    {
                        chromosome.Add(new Gene(currentSight));

                    }
                    else
                    {
                        totalTime -= CountDistance(prevoiusSight, currentSight);
                        totalTime -= currentSight.Time;
                        totalCost -= currentSight.Cost;
                    }
                }
                   
                count++;
                prevoiusSight = currentSight;
            }

           return chromosome;
        }

        
        public Chromosome CreateChromosome(UserData userData)
        {
            Chromosome chromosome = new Chromosome();
            double totalCost = 0;
            double totalTime = 0;
            var previousSight = new RandomSight();
            bool added = false;

            for(int i = 0; i<chromosomeLength; i++)
            {
                added = false;

                while (added==false)
                {
                    var currentSight = FindNewSight();

                    if (!CheckIfGenesDuplicate(chromosome, currentSight))
                    {
                        if (previousSight != null)
                        {
                            totalTime += CountDistance(previousSight, currentSight);
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                chromosome.Add(new Gene(currentSight));
                                added = true;
                            }
                            else
                            {
                                totalTime -= CountDistance(previousSight, currentSight);
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }

                        }
                        else
                        {
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                chromosome.Add(new Gene(currentSight));
                                added = true;
                            }
                            else
                            {
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }
                        }

                    }
                    else
                    {
                        while (CheckIfGenesDuplicate(chromosome, currentSight))
                        {
                            currentSight = FindNewSight();
                        }

                        if (previousSight != null)
                        {
                            totalTime += CountDistance(previousSight, currentSight);
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                chromosome.Add(new Gene(currentSight));
                                added = true;
                            }
                            else
                            {
                                totalTime -= CountDistance(previousSight, currentSight);
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }

                        }
                        else
                        {
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                chromosome.Add(new Gene(currentSight));
                                added = true;
                            }
                            else
                            {
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }
                        }

                    }
                    previousSight = currentSight;
                }
            }

            while(chromosome.Count<chromosomeLength)
            {
                var sight = FindNewSight();
                SetSightToNull(sight);
                chromosome.Add(new Gene(sight));
            }


            return chromosome;
        }

        
        //Create Population

        public Population CreatePopulationAsync(UserData userData)
        {
            var population = new Population()
            {
                ReEvaluateAll = false,
                LinearlyNormalised = true,

            };

            for (var p = 0; p < populationSize; p++)
            {
                var chromosome = CreateOneChromosome(userData);

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
                var sight = (RandomSight)gene.ObjectValue;
                totalTime += sight.Time;

            }

            return totalTime;
        }

        public double CalculateCost(Chromosome chromosome)
        {
            double totalCost = 0;

            foreach (var gene in chromosome.Genes)
            {
                var sight = (RandomSight)gene.ObjectValue;
                totalCost += sight.Cost;

            }

            return totalCost;
        }


        //Computing fitness function
        public double CalculateFitness(Chromosome chromosome)
        {
            double fitness = 0;
            double totalTime = 0;
            double totalCost = 0;
            RandomSight previousSight = new RandomSight();
            RandomSight currentSight = new RandomSight();
            Chromosome temp = new Chromosome();
            int count = 0;
           

            foreach (var gene in chromosome.Genes)
            {
                count = 0;

                while(count < 300)
                {
                    currentSight = (RandomSight)gene.ObjectValue;

                    if (!temp.Genes.Contains(gene))
                    {
                        if (previousSight != null)
                        {
                            totalTime += CountDistance(previousSight, currentSight);
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                temp.Add(new Gene(currentSight));
                            }
                            else
                            {
                                totalTime -= CountDistance(previousSight, currentSight);
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }

                        }
                        else
                        {
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                temp.Add(new Gene(currentSight));

                            }
                            else
                            {
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }
                        }
                    }
                    else
                    {

                        while (!CheckIfGenesDuplicate(temp, currentSight))
                        {

                            currentSight = FindNewSight();
                        }

                        if (previousSight != null)
                        {
                            totalTime += CountDistance(previousSight, currentSight);
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                temp.Add(new Gene(currentSight));
                            }
                            else
                            {
                                totalTime -= CountDistance(previousSight, currentSight);
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }

                        }
                        else
                        {
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                temp.Add(new Gene(currentSight));
                            }
                            else
                            {
                                totalTime -= currentSight.Time;
                                totalCost -= currentSight.Cost;
                            }
                        }

                    }
                    previousSight = currentSight;
                    count++;
                }
                
            }

          chromosome = temp;
            

            

            foreach (var gene in chromosome.Genes)
            {
                var sight = (RandomSight)gene.ObjectValue;

                  fitness += sight.Attractivenes;
                    
            }

            return fitness / 1000;
        }

        public void CheckChromosomeLength(Chromosome chromosome, int chromosomeLength)
        {
            if (chromosome.Count < chromosomeLength)
            {
                do
                {
                    var sight = FindNewSight();
                    SetSightToNull(sight);
                    chromosome.Add(new Gene(sight));

                }
                while (chromosome.Count < chromosomeLength);


            }

            else if (chromosome.Count > chromosomeLength)
            {
                do
                {
                    int i = 0;

                    foreach( var gene in chromosome.Genes)
                    {
                        var sight = (RandomSight)gene.ObjectValue;
                        if(sight.Attractivenes==0)
                        {
                            //chromosome.Genes.Remove(gene);
                        }
                    }
                    
                    chromosome.Genes.RemoveAt(i);
                    i++;
                } while (chromosome.Count > chromosomeLength);

            }
        }
        public GeneticAlgorithm CreateGeneticAlgorithm(Population population)
        {
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);
            
            //add the operators

            ga.Operators.Add(elite);

           // ga.Operators.Add(crossover);

            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(Terminate);

            return ga;
        }

        public static bool Terminate(Population population,
            int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 10;
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
            if (_alghService.userData.TrasportModeId == 1)
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