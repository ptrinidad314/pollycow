using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PollingApp.Entities;

namespace PollingApp.Services
{
    public class PollDataRepository : IPollDataRepository
    {
        private PollDataContext _context;

        public PollDataRepository(PollDataContext context)
        {
            _context = context;
        }

        public AccountRequest AddAccountRequest(AccountRequest accountRequest)
        {
            _context.AccountRequests.Add(accountRequest);
            _context.SaveChanges();
            return accountRequest;
        }

        public Option AddOption(Option option, int pollId)
        {
            var poll = GetPoll(pollId);
            poll.Options.Add(option);

            UpdatePoll(poll);

            return option;
        }

        public Poll AddPoll(Poll poll)
        {
            _context.Polls.Add(poll);
            _context.SaveChanges();
            return poll;
        }

        public void DeleteAccountRequest(AccountRequest accountRequest)
        {
            _context.AccountRequests.Remove(accountRequest);
            _context.SaveChanges();
        }

        public void DeleteOption(Option option)
        {
            _context.Options.Remove(option);
        }

        public void DeletePoll(Poll poll)
        {
            _context.Polls.Remove(poll);
        }

        public AccountRequest GetAccountRequest(int id)
        {
            return _context.AccountRequests.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<AccountRequest> GetAllAccountRequests()
        {
            return _context.AccountRequests.OrderBy(r => r.Email);
        }

        public Option GetOption(int optionId)
        {
            return _context.Options.Where(p => p.Id == optionId).FirstOrDefault();
        }

        public Poll GetPoll(int pollId)
        {
            return _context.Polls.Include(p => p.Options).Where(p => p.Id == pollId).FirstOrDefault();
        }

        public IEnumerable<Poll> GetPolls()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Poll> GetUserPolls(string UserId)
        {
            return _context.Polls.Include(p => p.Options).Where(p => p.UserName == UserId).OrderBy(p => p.Name);
        }

        public void SavePollDataChanges()
        {
            _context.SaveChanges();
        }

        public AccountRequest UpdateAccountRequest(AccountRequest accountRequest)
        {
            _context.Attach(accountRequest).State = EntityState.Modified;
            _context.SaveChanges();
            return accountRequest;
        }

        public Option UpdateOption(Option option)
        {
            _context.Attach(option).State = EntityState.Modified;
            _context.SaveChanges();
            return option;
        }

        public Poll UpdatePoll(Poll poll)
        {
            _context.Attach(poll).State = EntityState.Modified;
            _context.SaveChanges();
            return poll;
        }
    }
}
