using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateShip
{
    public class Session
    {
        public string url;

        public Session()
        {

        }

        public Session(PirateShip ship)
        {
            this.url = ship.url.AbsoluteUri;
        }
    }
}
