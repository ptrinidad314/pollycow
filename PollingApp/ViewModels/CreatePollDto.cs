using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollingApp.ViewModels
{
    public class CreatePollDto
    {
        public string title { get; set; }
        public IEnumerable<string> options { get; set; }
    }
}
