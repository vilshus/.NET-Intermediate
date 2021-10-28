using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AsyncAwait.Task2.CodeReviewChallenge.Models;
using AsyncAwait.Task2.CodeReviewChallenge.Models.Support;
using AsyncAwait.Task2.CodeReviewChallenge.Services;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAssistant _assistant;

        private readonly IPrivacyDataService _privacyDataService;
        private readonly IStatisticService _statisticService;

        public HomeController(IAssistant assistant, IPrivacyDataService privacyDataService, IStatisticService statisticService)
        {
            _assistant = assistant ?? throw new ArgumentNullException(nameof(assistant));
            _privacyDataService = privacyDataService ?? throw new ArgumentNullException(nameof(privacyDataService));
            _statisticService = statisticService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Privacy()
        {
            ViewBag.Message = await _privacyDataService.GetPrivacyDataAsync();
            return View();
        }

        public async Task<IActionResult> Help()
        {
            ViewBag.RequestInfo = await _assistant.RequestAssistanceAsync("guest");
            return View();
        }

        public async Task<IActionResult> Statistics(string visitedPagePath)
        {
            await _statisticService.RegisterVisitAsync(visitedPagePath);
            var visits = await _statisticService.GetVisitsCountAsync(visitedPagePath);
            ViewBag.VisitsCount = visits;
            return PartialView("_StatisticsPartial");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
