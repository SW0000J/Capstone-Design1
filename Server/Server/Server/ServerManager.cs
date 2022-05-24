using TCPServer.Server_Dll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class ServerManager
    {
        private Server mServer;

        public ServerManager()
        {
            Init();
        }

        private void Init()
        {
            mServer = new Server();
        }

        public void OnApplicationQuit()
        {
            mServer.OnApplicationQuit();
        }
    }
}
