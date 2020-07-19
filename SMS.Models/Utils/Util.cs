using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models.Utils
{
    class Util
    {
        public static string FormatDate(string date)
        {
            var strs = date.Split('/');
            return strs[1] + "/" + strs[0] + "/" + strs[2];
        }
    }
}
