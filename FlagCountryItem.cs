using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNClient
{
    class FlagCountryItem
    {

            public string code;
            public string country;

       
            public FlagCountryItem(string cd, string ct)
            {
                 code = cd;
                 country = ct;
            }

            public override string ToString()
            {
                return country;
            }
    
    }
}
