using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTimeBotForm.Mercurial
{
    public class MercurialCommitItem
    {
        public string Description { get; set; }
        public string Branch { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
