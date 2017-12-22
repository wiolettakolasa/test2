using System;
using System.Collections.Generic;

namespace SightseeingApp.Models
{
    public interface IAppRepository :IDisposable
    {
        IEnumerable<Sight> GetSights();

        Sight GetSightByID(int SightId);

        IEnumerable<RandomSight> GetRandomSights();

        RandomSight GetRandomSightByID(int RandomSightId);

        void AddRandomSight(RandomSight sight);

        void RemoveRandomSight(RandomSight sight);

        IEnumerable<Category> GetCategories();

        void AddSight(Sight sight);

        void SaveChanges();
        void RemoveSight(Sight sight);
        IEnumerable<TransportMode> GetTransportModes();

    }
}