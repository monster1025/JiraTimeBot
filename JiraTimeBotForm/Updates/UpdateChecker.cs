using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiraTimeBotForm.Updates
{
    public class UpdateChecker
    {
        public bool NewVersionAvilable()
        {
            var current = Application.ProductVersion;
            return false;
        }
    }
}
