using SMS.Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models.ViewModel
{
    public class BringInViewModel
    {
        public IEnumerable<Bring_In> Bring_Ins { get; set; }
        public IEnumerable<Bring_In_Items> Bring_In_Items { get; set; }
    }
}
