using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netmo
{
    public class Token
    {
        public string access_token = string.Empty;
        public string refresh_token = string.Empty;
        public int expires_in = 0;
        public int expire_in = 0;

        public List<string> scope;

    }
}
