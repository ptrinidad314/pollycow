using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollingApp.Entities;
using PollingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollingApp.Controllers
{
    [Authorize]
    public class AccountController:Controller
    {
        private IPollDataRepository _pollDataRepository;

        public AccountController(IPollDataRepository pollDataRepository)
        {
            _pollDataRepository = pollDataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreateAccountRequest()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateAccountRequest(AccountRequestDto accountRequestDto)
        {

            var accountRequest = new AccountRequest();
            accountRequest.Email = accountRequestDto.Email;

            if (accountRequestDto.UserName != "")
            {
                accountRequest.UserName = accountRequestDto.UserName;
            }

            accountRequest.Complete = false;

            _pollDataRepository.AddAccountRequest(accountRequest);

            return RedirectToAction(nameof(SuccessfulRequest));
        }

        [AllowAnonymous]
        public IActionResult SuccessfulRequest()
        {
            return View();
        }


        [Authorize(Policy = "RequireElevatedRights")]
        public IActionResult ViewAccountRequests()
        {
            var model = _pollDataRepository.GetAllAccountRequests();

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAccountRequest(int accountRequestId)
        {
            var accountRequest = _pollDataRepository.GetAccountRequest(accountRequestId);

            return View(accountRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAccountRequest(AccountRequestDto accountRequestDto)
        {
            var accountRequest = _pollDataRepository.GetAccountRequest(accountRequestDto.Id);
            if (accountRequest != null)
            {
                accountRequest.Email = accountRequestDto.Email;
                accountRequest.UserName = accountRequestDto.UserName;
                accountRequest.Complete = accountRequestDto.Complete;

                _pollDataRepository.UpdateAccountRequest(accountRequest);
            }

            return RedirectToAction(nameof(ViewAccountRequests));
        }

        [HttpPost]
        public IActionResult DeleteAccountRequest(int accountRequestId)
        {
            var accountRequest = _pollDataRepository.GetAccountRequest(accountRequestId);

            if (accountRequest != null)
            {
                _pollDataRepository.DeleteAccountRequest(accountRequest);
            }

            return RedirectToAction(nameof(ViewAccountRequests));
        }

    }
}
