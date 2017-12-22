using GAF;
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
    // odbierać dane od użytkownika
    public class UserDataController : Controller
    {
        private AppRepository _userDataRepository;
        private AlghService _alghService;
        

        public UserDataController()
        {
            _userDataRepository = new AppRepository();
            _alghService = new AlghService();
         
        }


        public ActionResult Index()
        {
           return View();

        }
        public ActionResult UserDataForm()
        {
          var selectedItems =  _userDataRepository.GetCategories();

          var viewModel = new NewUserDataViewModel();

          viewModel.GetInterests = _userDataRepository.GetCategories().Select(c => new Category { Id = c.Id, CategoryName = c.CategoryName });

          viewModel.GetTransportMode = _userDataRepository.GetTransportModes().Select(c => new TransportMode { Id = c.Id, TransportModeName = c.TransportModeName });


           return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> SaveAsync(NewUserDataViewModel viewModel)
        {
            var chromosome = await _alghService.CreateChromosomeAsync(viewModel.userData);

            var population = await _alghService.CreatePopulationAsync(viewModel.userData);
            var fitness = _alghService.CalculateFitness(chromosome);
            var ag = _alghService.CreateGeneticAlgorithm(population);
            var fittest = ag.Population.GetTop(1)[0];
            var fittest2 = ag.Population.GetTop(2)[0];
            var elites = ag.Population.GetElites();
            var maxfitness = ag.Population.MaximumFitness;
            var minfitness = ag.Population.MinimumFitness;
            var ag2 = _alghService.CreateGeneticAlgorithm(ag.Population);


            return RedirectToAction("ShowResult", "Result");

        }
    }
}