using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service
{
    public class RandomDataBase
    {
        //Dodać randomowe dane od użytkownika

        private IAppRepository _rndSightRepository;

        public RandomDataBase()
        {
            _rndSightRepository = new AppRepository();
        }

        RandomSight sight { get; set; }

        int NumberOfSights { get; set; }

        public void GetRandomDataBase(int number) //it works!
        {
            var rnd = new Random();

            for(int i = 0; i < number; i++)
            {
                sight = new RandomSight();
                sight.Name = "Zabytek" + i;
                sight.Address = "Adres" + i;
                sight.Cost = rnd.Next(40);
                sight.Time = (rnd.NextDouble())*(3-0.2)+0.2;
                sight.Latitude = rnd.NextDouble() + 50.0;
                sight.Longtitude = rnd.NextDouble()+ 19.0;
                sight.CategoryId = (byte)rnd.Next(1,5);
                sight.Attractivenes = rnd.Next(1, 10);

                if (sight.Id == 0)
                    _rndSightRepository.AddRandomSight(sight);

                _rndSightRepository.SaveChanges();


            }
        }
    }

    
}