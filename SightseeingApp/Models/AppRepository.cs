using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Models
{
    public class AppRepository : IAppRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public AppRepository()
        {
            _context = new ApplicationDbContext();
        }

        public void Dispose()
        {
            ((IDisposable)_context).Dispose();
        }

        public Sight GetSightByID(int SightId)
        {
            return _context.Sights.Find(SightId);
        }

        public IEnumerable<Sight> GetSights()
        {
            return _context.Sights.ToList();
        }

         public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public void AddSight(RandomSight sight)
        {
            _context.RandomSight.Add(sight);
        }

        public void AddSight(Sight sight)
        {
            _context.Sights.Add(sight);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void RemoveSight(Sight sight)
        {
            _context.Sights.Remove(sight);
        }

       public IEnumerable<TransportMode> GetTransportModes()
        {
            return _context.TransportModes.ToList();
        }

        public IEnumerable<RandomSight> GetRandomSights()
        {
            return _context.RandomSight.ToList();
        }

        public RandomSight GetRandomSightByID(int id)
        {
            return _context.RandomSight.Find(id);
        }

        public void AddRandomSight(RandomSight sight)
        {
            _context.RandomSight.Add(sight);
        }

        public void RemoveRandomSight(RandomSight sight)
        {
            throw new NotImplementedException();
        }
    }
}