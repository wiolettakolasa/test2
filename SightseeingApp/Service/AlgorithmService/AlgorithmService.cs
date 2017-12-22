using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service.AlgorithmService
{
    public class AlgorithmService
    {
        private IAppRepository _sightRepository;
        private ComputingDistService _distanceService;
        public UserData userData;

        public AlgorithmService()
        {
            userData = new UserData();
            _sightRepository = new AppRepository();
            _distanceService = new ComputingDistService();
        }

        //Alghoritm Settings
        const int populationSize = 200;
        const int chromosomeLength = 10;
        const double crossoverProbability = 0.6;
        const double mutationProbability = 0.02;
        const int elitismPercentage = 10;
        int iterationNumber = 20;

        //Create Chromosome
        public Chromosome CreateChromosomeAsync(UserData userData)
        {
            var chromosome = new Chromosome();
            chromosome.List = new List<RandomSight>();
            var list = chromosome.List;
            var rnd = new Random();
            double totalTime = 0;
            double totalCost = 0;
            int count = 0;
            var prevoiusSight = new RandomSight();
            double travelTime = 0;

            while (count < 50)
            {
                var currentSight = FindNewSight();

                if (prevoiusSight == null)
                {
                    totalTime += currentSight.Time;
                    totalCost += currentSight.Cost;

                    if (totalCost < userData.Cost && totalTime < userData.Time)
                    {
                        list.Add(currentSight);

                    }
                    else
                    {
                        totalTime -= currentSight.Time;
                        totalCost -= currentSight.Cost;
                    }
                }
                else
                {
                   // travelTime = await CalculateTimeAsync(chromosome);
                    totalTime += travelTime;
                    totalTime += currentSight.Time;
                    totalCost += currentSight.Cost;

                    if (totalCost < userData.Cost && totalTime < userData.Time)
                    {
                        list.Add(currentSight);

                    }
                    else
                    {
                        totalTime -= travelTime;
                        totalTime -= currentSight.Time;
                        totalCost -= currentSight.Cost;
                    }
                }

                count++;
                prevoiusSight = currentSight;
            }

            chromosome.ChromosomeLength = chromosome.List.Count;
            chromosome.Fitness = EvaluateSolutions(chromosome, userData);

            return chromosome;
        }

        //CalculateFitness
        public double EvaluateSolutions(Chromosome chromosome, UserData userData)
        {
            double fitness = 0;
            double totalTime = 0;
            double totalCost = 0;
            RandomSight previousSight = new RandomSight();
            RandomSight currentSight = new RandomSight();
            Chromosome temp = new Chromosome();
            temp.List = new List<RandomSight>();
            var tempList = temp.List;
            var list = chromosome.List;
            int count = 0;
            double travelTime = 0;


            foreach (var gene in list)
            {
                count = 0;

                while (count < 50)
                {
                    currentSight = gene;

                    if (!temp.List.Contains(gene))
                    {
                        if (previousSight != null)
                        {
                           // travelTime = await CalculateTimeAsync(chromosome);
                            totalTime += travelTime;
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                tempList.Add(currentSight);
                            }
                            else
                            {
                                totalTime -= travelTime;
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
                                tempList.Add(currentSight);

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
                           // travelTime = await CalculateTimeAsync(chromosome);
                            totalTime += travelTime;
                            totalTime += currentSight.Time;
                            totalCost += currentSight.Cost;

                            if (totalCost < userData.Cost && totalTime < userData.Time)
                            {
                                tempList.Add(currentSight);
                            }
                            else
                            {
                                totalTime -= travelTime;
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
                                tempList.Add(currentSight);
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
            list = temp.List;

            foreach (var gene in list)
            {
              

                if (userData.Interests.Contains(gene.CategoryId))
                    fitness += gene.Attractivenes;
                
               
            }

            return fitness / 1000;
        }

        public Population CreatePopulationAsync(UserData userData)
        {
            var population = new Population()
            {
                Solutions = new List<Chromosome>()
               
            };

            for (var p = 0; p < populationSize; p++)
            {
                var chromosome = CreateChromosomeAsync(userData);

                ShuffleFast(chromosome);

                population.Solutions.Add(chromosome);
            }


            return population;
        }

       
        //Genetic algorithm
        public GeneticAlgorithm CreateGeneticAlgorithm(Population population)
        {
            var ga = new GeneticAlgorithm()
            {
                Population = population,
                GetResult = new Result(),
                Generation = new Generation()
            };
            ga.Generation.List = new List<Population>();

            int count = 0;

            while(count<iterationNumber)
            {
                var newPopulation = new Population();
                var sortPopulation = population.Solutions.OrderByDescending(c => c.Fitness);
                population.Solutions = sortPopulation.ToList();

               
                ga.Generation.List.Add(population); 


                //Find elite solutions
                var eliteList = new List<Chromosome>();
                eliteList = ElitismOperator(population, elitismPercentage);

                //Create new population

                //Adding elite solutions
                newPopulation.Solutions = eliteList;


                //Select parents
                var number = (int)(populationSize * crossoverProbability);
                var parents = SelectParentsRoulette(population, number);

                //Crossover
                PerformCrossover(number, parents, newPopulation, population);

                //Nowa populacja
                population = newPopulation;
                newPopulation = null;

                //Mutate
                PerformMutation(population);


                
                //Evaluate solutions
                foreach (var sol in population.Solutions)
                {
                    sol.Fitness = EvaluateSolutions(sol, userData);
                }

                //Sort Solutions
                var ordered = population.Solutions.OrderByDescending(c => c.Fitness);
                population.Solutions = ordered.ToList();

                count++;
            }

            //Sort Solutions
            var sorted = population.Solutions.OrderByDescending(c => c.Fitness);
            ga.Population.Solutions = sorted.ToList();


            //Get Result

           var best = population.Solutions[0];

            ga.GetResult.BestOrder = best;
            ga.GetResult.TotalCost = CalculateCost(best);
            ga.GetResult.TotalTime = CalculateTime(best);
            ga.GetResult.TotalFitness = best.Fitness;
            //ga.Generation.List.Add(population);
            ga.Generation.FirstFittest =ga.Generation.List[0].Solutions.OrderByDescending(c => c.Fitness).First();
            ga.Generation.LastFittest = ga.Generation.List[iterationNumber-1].Solutions.OrderByDescending(c => c.Fitness).First();


            //ga.GetResult.BestOrder.Fitness = population.List[0].Fitness;
            return ga;
        }

        //Methods of parents selection

        //Roulette (Proportionate selection)
        public List<Chromosome> SelectParentsRoulette(Population population, int number)
            {
            var parents = new List<Chromosome>();
            var roulette = new List<double>();
            var chromosomes = population.Solutions;
            double percent = 0;

            var fitnessSum = chromosomes.Sum(c => c.Fitness);

            foreach(var chr in chromosomes)
            {
                percent += chr.Fitness / fitnessSum;
                roulette.Add(percent);
            }


            for(int i=0; i<number; i++)
            {
                var rnd = new Random();
                var pointer = rnd.NextDouble();
                var chromosomeId = roulette.Select((value, index) => new { Value = value, Index = index }).FirstOrDefault(r => r.Value >= pointer).Index;
                parents.Add(chromosomes[chromosomeId]);
            }

            return parents;

            }

        //Tournament selection

        public List<Chromosome> SelectParentsTournament(Population population, int number)
        {
            var parents = new List<Chromosome>();
            var selected = new List<Chromosome>();
            var candidates = population.Solutions;
            var rnd = new Random();
            var found = 0;
            var size = 20;

            for(int i = 0; i<number; i++)
            {
                while(found<size)
                {
                    var randomChromosome = rnd.Next(0, populationSize);
                    var tournamentWinner = candidates[randomChromosome];
                    selected.Add(tournamentWinner);
                   
                    found++;
                }

                var winner = selected.OrderByDescending(c => c.Fitness).First();
                parents.Add(winner);
                candidates.Remove(winner);
            }

            return parents;
        }

        //Rank selection
        public List<Chromosome> SelectParentsRank(Population population, int number)
        {
            var parents = new List<Chromosome>();
            var selected = new List<Chromosome>();
            

            return parents;
        }


        //Genetic operators
        public List<Chromosome> ElitismOperator(Population population, int elitismPercentage)
        {
            var number = (int) elitismPercentage * populationSize /100;
            var ordered = population.Solutions.OrderByDescending(c => c.Fitness);

            var eliteList = ordered.Take(number).ToList();

            foreach (var chromosome in eliteList)
            {
                chromosome.IsElite = true;
            }
            return eliteList;

        }

        //Operator krzyżowania - SinglePoint
        public List<Chromosome> CrossoverOperator(Chromosome parent1, Chromosome parent2)
        {
            var children = new List<Chromosome>();
            var child1 = new Chromosome();
            var child2 = new Chromosome();
            child1.List = new List<RandomSight>();
            child2.List = new List<RandomSight>();
            var rnd = new Random();
            var sight = new RandomSight();

            //Check if chromosomes are the same length
            if (parent1.List.Count != parent2.List.Count)
            {
                if (parent1.List.Count > parent2.List.Count)
                {
                    var toAdd = parent1.List.Count - parent2.List.Count;
                    for (int i = 0; i < toAdd; i++)
                    {
                        sight = FindNewSight();
                        SetSightToNull(sight);
                        parent2.List.Add(sight);
                        ShuffleFast(parent2);
                    }
                    parent2.ChromosomeLength = parent2.List.Count;
                }
                else
                {
                    var toAdd = parent2.List.Count - parent1.List.Count;
                    for (int i = 0; i < toAdd; i++)
                    {
                        sight = FindNewSight();
                        SetSightToNull(sight);
                        parent1.List.Add(sight);
                        ShuffleFast(parent1);
                    }
                    parent1.ChromosomeLength = parent1.List.Count;
                }
            }

                var locus = rnd.Next(0, parent1.List.Count);

                for(int i = 0; i<parent1.List.Count; i++)
                {
                    if(i<locus)
                    {
                        child1.List.Add(parent1.List.ElementAt(i));
                        child2.List.Add(parent2.List.ElementAt(i));
                }
                    else
                    {
                        child1.List.Add(parent2.List.ElementAt(i));
                        child2.List.Add(parent1.List.ElementAt(i));
                }
                   
                }

                var temp1 = child1;

                for(int i = 0; i<child1.List.Count; i++)
                {
                    
                    if(child1.List[i].Attractivenes==0)
                    {
                        sight = child1.List[i];
                        temp1.List.Remove(sight);
                    }
                }

            child1 = temp1;

            var temp2 = child2;
            for (int i = 0; i < child2.List.Count; i++)
            {

                if (child2.List[i].Attractivenes == 0)
                {
                    sight = child2.List[i];
                    temp2.List.Remove(sight);
                }
            }

            child2 = temp2;

            child1.Fitness = EvaluateSolutions(child1, userData);
            child1.ChromosomeLength = child1.List.Count;

            child2.Fitness = EvaluateSolutions(child2, userData);
            child2.ChromosomeLength = child2.List.Count;


            return children;
        }

        //Operator krzyżowania - DoublePoint
        public Chromosome CrossoverOperatorDoublePoint(Chromosome parent1, Chromosome parent2)
        {
            var child = new Chromosome();
            child.List = new List<RandomSight>();
            var rnd = new Random();
            var sight = new RandomSight();

            //Check if chromosomes are the same length - zrobic funkcję
            if (parent1.List.Count != parent2.List.Count)
            {
                if (parent1.List.Count > parent2.List.Count)
                {
                    var toAdd = parent1.List.Count - parent2.List.Count;
                    for (int i = 0; i < toAdd; i++)
                    {
                        sight = FindNewSight();
                        SetSightToNull(sight);
                        parent2.List.Add(sight);
                        ShuffleFast(parent2);
                    }
                    parent2.ChromosomeLength = parent2.List.Count;
                }
                else
                {
                    var toAdd = parent2.List.Count - parent1.List.Count;
                    for (int i = 0; i < toAdd; i++)
                    {
                        sight = FindNewSight();
                        SetSightToNull(sight);
                        parent1.List.Add(sight);
                        ShuffleFast(parent1);
                    }
                    parent1.ChromosomeLength = parent1.List.Count;
                }
            }

            //Two different locuses
            var locus1 = rnd.Next(0, parent1.List.Count);
            var locus2 = rnd.Next(0, parent1.List.Count);

            while(locus1==locus2)
            {
                locus2 = rnd.Next(0, parent1.List.Count);
            }

            if(locus1>locus2)
            {
                var locus = locus2;
                locus2 = locus1;
                locus1 = locus;
            }

            for (int i = 0; i < parent1.List.Count; i++)
            {
                
                if (i>locus1 && i<locus2)
                {
                    child.List.Add(parent1.List[i]);
                }
                else
                {
                    child.List.Add(parent2.List[i]);
                }

            }

            var temp = child;

            for (int i = 0; i < child.List.Count; i++)
            {

                if (child.List[i].Attractivenes == 0)
                {
                    sight = child.List[i];
                    temp.List.Remove(sight);
                }
            }

            child = temp;


            child.ChromosomeLength = child.List.Count;
            


            return child;
        }

        public void PerformCrossover(int number, List<Chromosome> parents, Population newPopulation, Population population)
        {
            var rnd = new Random();
            var children = new List<Chromosome>();

            var numberOfChildren = (int)number / 2;
            for (int m = 0; m < numberOfChildren; m++)
            {
                var parent1 = parents[rnd.Next(0, parents.Count)];
                parents.Remove(parent1);
                var parent2 = parents[rnd.Next(0, parents.Count)];
                parents.Remove(parent2);

                var child = CrossoverOperator(parent1, parent2);

                foreach(var c in child)
                {
                    children.Add(c);
                }
                
            }



            newPopulation.Solutions.AddRange(children);

            int j = 0;

            while (newPopulation.Solutions.Count < populationSize-1)
            {

                if (population.Solutions[j].IsElite == false)
                {
                    newPopulation.Solutions.Add(population.Solutions[j]);
                }
                j++;
            }
        }

        public Chromosome MutationOperator(Chromosome chromosome, int locus)
        {
            
            var rnd = new Random();
            locus = rnd.Next(0, chromosome.ChromosomeLength);

            chromosome.List.RemoveAt(locus);

            double time = CalculateTime(chromosome);
            double cost = CalculateCost(chromosome);

            var sight = FindNewSight();

            while(!chromosome.List.Contains(sight))
            {
                sight = FindNewSight();
            }

            chromosome.List.Insert(locus, sight);


            return chromosome;
         }
        
        public void PerformMutation(Population population)
        {
            var rnd = new Random();

            for (int l = 0; l < (int)(populationSize * mutationProbability); l++)
            {
                var index = rnd.Next(0, populationSize);
                var chromosome = population.Solutions[index];

                while (chromosome.IsElite == false)
                {
                    index = rnd.Next(0, populationSize);
                    chromosome = population.Solutions[index];
                }

                population.Solutions.Remove(chromosome);

                var locus = rnd.Next(0, chromosome.List.Count);
                chromosome = MutationOperator(chromosome, locus);
                population.Solutions.Insert(index, chromosome);

            }
        }


        //Helpful methods

            //Set sight to null
        public void SetSightToNull(RandomSight sight)
        {
            sight.Attractivenes = 0;
            sight.Latitude = 0;
            sight.Latitude = 0;
            sight.Time = 0;
            sight.Cost = 0;
            sight.CategoryId = 0;
        }
        //Mixing genes in chromosome
        public void ShuffleFast(Chromosome chromosome)
        {
            if (chromosome!=null)
            {
                List<RandomSight> list = chromosome.List;
                int n = chromosome.List.Count;
                var rnd = new Random();

                while (n > 1)
                {
                    int i = (rnd.Next(0, n) % n);
                    n--;

                    var sight = list[i];
                    list[i] = list[n];
                    list[n] = sight;

                }
               
            }
                
        }

        //Find New Sight
        public RandomSight FindNewSight()
        {
            RandomSight sight;
            var rnd = new Random();
            var sights = _sightRepository.GetRandomSights();

            sight = sights.ElementAt(rnd.Next(1, 100));

            return sight;
        }

        //Check if genes duolicate
        public bool CheckIfGenesDuplicate(Chromosome chromosome, RandomSight sight)
        {
            bool result = false;

            foreach (var gene in chromosome.List)
            {

                if (gene.Attractivenes != 0)
                {
                    if (sight == gene)
                    {
                        result = true;

                        return true;
                    }
                }

            }

            return result;
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

        //Calculate totalTime and totalCost in chromosome
        public double CalculateTime(Chromosome chromosome)
        {
            double totalTime = 0;
      

            foreach (var gene in chromosome.List)
            {


                totalTime += gene.Time;
            }

            return totalTime;
        }

        public double CalculateCost(Chromosome chromosome)
        {
            double totalCost = 0;

            foreach (var gene in chromosome.List)
            {
              totalCost += gene.Cost;

            }

            return totalCost;
        }

        //Calculate time of transport (driving)
        public static async System.Threading.Tasks.Task<double> CalculateTimeAsync(Chromosome chromosome)
        {
            var timeToTravel = 0.0;
            RandomSight previousSight = null;
            var _timeService = new ComputingDistService();

            //run through each sight in the order specified in the chromosome
            foreach (var gene in chromosome.List)
            {
                var currentSight = (RandomSight)gene;

                if (previousSight != null)
                {
                    var result = await _timeService.GetDistanceDrivingResultRandomSight(currentSight, previousSight);

                    timeToTravel += result.time;
                }

                previousSight = currentSight;
            }

            return timeToTravel;
        }

    }
}