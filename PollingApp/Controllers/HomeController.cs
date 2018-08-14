using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollingApp.Entities;
using PollingApp.Services;
using PollingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PollingApp.Controllers
{
    [Authorize]
    public class HomeController:Controller
    {
        private IPollDataRepository _pollDataRepository;

        public HomeController(IPollDataRepository pollDataRepository)
        {
            _pollDataRepository = pollDataRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserPolls()
        {
            var model = _pollDataRepository.GetUserPolls(GetADUserName());

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePollDto createPollDto)
        {
            if (createPollDto.title == null || createPollDto.title == "")
            {
                return View();
            }
            else
            {
                var poll = new Poll();
                poll.Name = createPollDto.title;
                poll.UserName = GetADUserName();
                poll = _pollDataRepository.AddPoll(poll);

                foreach (var o in createPollDto.options)
                {
                    if (o != null && o != "")
                    {
                        var option = new Option();
                        option.Name = o;
                        option.Votes = 0;

                        option = _pollDataRepository.AddOption(option, poll.Id);
                    }
                }

                return RedirectToAction(nameof(Details), poll);               
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOption(string optionTitle, int pollId)
        {
            var option = new Option();
            option.Name = optionTitle;
            option.Votes = 0;

            option = _pollDataRepository.AddOption(option, pollId);

            return RedirectToAction(nameof(Vote), new { id = pollId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePoll(int pollId)
        {
            var poll = _pollDataRepository.GetPoll(pollId);

            if(poll != null)
            {
                foreach (var o in poll.Options)
                {
                    _pollDataRepository.DeleteOption(o);
                }

                _pollDataRepository.DeletePoll(poll);

                _pollDataRepository.SavePollDataChanges();
            }

            return RedirectToAction(nameof(UserPolls));
        }

        public IActionResult Details(Poll poll)
        {

            return View(poll);
        }

        [AllowAnonymous]
        public IActionResult Vote(int id)
        {
            var poll = _pollDataRepository.GetPoll(id);

            if(poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVote(int optionId)
        {
            var option = _pollDataRepository.GetOption(optionId);

            if(option == null)
            {
                return NotFound();
            }

            option.Votes += 1;

            option = _pollDataRepository.UpdateOption(option);

            return RedirectToAction(nameof(PollResults), new { id = option.PollId });
        }

        [AllowAnonymous]
        public IActionResult PollResults(int id)
        {
            var poll = _pollDataRepository.GetPoll(id);

            if(poll == null)
            {
                return NotFound();
            }

            string chartLabel = "";
            string chartData = "";

            foreach (var o in poll.Options)
            {
                chartLabel = chartLabel + "\"" + o.Name + "\",";
                chartData = chartData + o.Votes + ",";
            }

            ViewBag.Labels = "[" + chartLabel + "]";
            ViewBag.Data = "[" + chartData + "]";

            return View(poll);
        }

        private string GetADUserName()
        {
            return User.Claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault().Value;
        }

    }
}
