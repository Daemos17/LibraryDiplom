using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extentions
{
    public static class ExtentionsString
    {
        public static bool IsNullOrEmptyOrWhitespace(this string inputString)
        {
            if (inputString == "" || inputString == null || inputString.Trim()== "")
            {
                return true;
            }
            return false;
        }
    }
}
