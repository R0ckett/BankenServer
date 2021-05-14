using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankenServer
{
    class xmlException : Exception
    {  
            public xmlException() : base("XML fil finns inte! kör programmet utan Load funktionen och skriv i klient en gång först!")
            {

            }
    }
}
