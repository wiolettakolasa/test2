using SightseeingApp.Models;
using SightseeingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SightseeingApp.Controllers
{
    public class SightController : Controller
    {
        private ApplicationDbContext _context;

        public SightController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Sight

        public ActionResult Index()
        {
            var sights = _context.Sights.ToList();
            return View(sights);
        }


        public ActionResult Details(int id)
        {
            var sight = _context.Sights.SingleOrDefault(s => s.Id == id);

            if (sight == null)
                return HttpNotFound();

            return View(sight);
        }

        public ActionResult NewSight()
        {
            var category = _context.Categories.ToList();
            var viewModel = new NewSightViewModel()
            {
                Categories = category
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sight sight)
        {
            if (sight.Id == 0)
                _context.Sights.Add(sight);

            else
            {
                var sightInDb = _context.Sights.Single(s => s.Id == sight.Id);
                sightInDb.Name = sight.Name;
                sightInDb.Cost = sight.Cost;
                sightInDb.Time = sight.Time;
                sightInDb.CoordinateX = sight.CoordinateX;
                sightInDb.CoordinateY = sight.CoordinateY;
                sightInDb.CategoryId = sight.CategoryId;
                sightInDb.Attractivenes = sight.Attractivenes;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Sight");
        }

        public ActionResult Edit(int id)
        {
            var sight = _context.Sights.SingleOrDefault(c => c.Id == id);

            if (sight == null)
                return HttpNotFound();

            var viewModel = new NewSightViewModel
            {
                Sight = sight,
                Categories = _context.Categories.ToList()
            };

            return View("NewSight", viewModel);
        }

    }
}
