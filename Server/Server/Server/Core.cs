using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Core
    {
        static ServerManager mServerManager;
        static void Main(string[] args)
        {
            mServerManager = new ServerManager();
            while (true)
            {
                
            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            mServerManager.OnApplicationQuit();
            throw new NotImplementedException();
        }
    }
}
