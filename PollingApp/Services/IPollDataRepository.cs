using PollingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollingApp.Services
{
    public interface IPollDataRepository
    {
        IEnumerable<Poll> GetPolls();
        Poll GetPoll(int pollId);
        Option GetOption(int optionId);
        IEnumerable<Poll> GetUserPolls(string UserId);
        Poll AddPoll(Poll poll);
        Option AddOption(Option option, int pollId);
        Poll UpdatePoll(Poll poll);
        Option UpdateOption(Option option);
        void DeletePoll(Poll poll);
        void DeleteOption(Option option);
        void SavePollDataChanges();
        AccountRequest AddAccountRequest(AccountRequest accountRequest);
        AccountRequest GetAccountRequest(int id);
        AccountRequest UpdateAccountRequest(AccountRequest accountRequest);
        void DeleteAccountRequest(AccountRequest accountRequest);
        IEnumerable<AccountRequest> GetAllAccountRequests();
    }
}
