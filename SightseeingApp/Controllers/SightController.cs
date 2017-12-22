using SightseeingApp.Models;
using SightseeingApp.Service;
using SightseeingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SightseeingApp.Controllers
{
    [Route ("api/[Controller]")]
    public class SightController : Controller
    {
        private IAppRepository _sightRepository;
        private GeoCoordsService _geoCoordsService;
       
        public SightController()
        {
            _sightRepository = new AppRepository();
            _geoCoordsService = new GeoCoordsService();
           
        }

       
        // GET: Sight

        public ActionResult Index()
        {
            var sight = _sightRepository.GetSights();
            return View(sight);

        }

       
        public ActionResult Details(int id)
        {

            Sight sight = _sightRepository.GetSightByID(id);

            if (sight == null)
                return HttpNotFound();

            return View(sight);
        }

        // GET: Sight/Delete/5
        public ActionResult Delete(int id)
        {
            Sight sight = _sightRepository.GetSightByID(id);

            if (sight == null)
                return HttpNotFound();

            return View(sight);
          
        }

        // POST: Sight/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sight sight = _sightRepository.GetSightByID(id);

            _sightRepository.RemoveSight(sight);
            _sightRepository.SaveChanges();

            return RedirectToAction("Index", "Sight");
        }

        public ActionResult NewSight()
        {
            var category = _sightRepository.GetCategories();
            var viewModel = new NewSightViewModel()
            {
                Categories = category
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> CreateAsync(Sight sight)
        {
            //dodać block try, ismodelstatevalid..
            //Lookup the GeoCoords
            var result = await _geoCoordsService.GetGeoCoords(sight.Address);
            if(!result.Success)
            {
                return Content(result.Message);
            }

            sight.Latitude = result.Latitude;
            sight.Longtitude = result.Longtitude;

            if (sight.Id == 0)
                _sightRepository.AddSight(sight);

            else
            {
                var sightInDb = _sightRepository.GetSightByID(sight.Id);
                sightInDb.Name = sight.Name;
                sightInDb.Address = sight.Address;
                sightInDb.Cost = sight.Cost;
                sightInDb.Time = sight.Time;
                sightInDb.Latitude = sight.Latitude;
                sightInDb.Longtitude = sight.Longtitude;
                sightInDb.CategoryId = sight.CategoryId;
                sightInDb.Attractivenes = sight.Attractivenes;
            }
            _sightRepository.SaveChanges();

            return RedirectToAction("Index", "Sight");
        }

        
        public ActionResult Edit(int id)
        {
            var sight = _sightRepository.GetSightByID(id);

            if (sight == null)
                return HttpNotFound();

            var viewModel = new NewSightViewModel
            {
                Sight = sight,
                Categories = _sightRepository.GetCategories()
        };

            return View("NewSight", viewModel);
        }
        

    }
}
