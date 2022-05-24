using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll
{
    class Room
    {
        public string roomName;
        public List<Client> clientList;

        public Room(string roomName_)
        {
            roomName = roomName_;
            clientList = new List<Client>();
        }

        public void JoinUser(Client client_)
        {
            clientList.Add(client_);
        }

        public void ExitUser(Client client_)
        {
            for (int i = clientList.Count - 1; i >= 0; i--)
            {
                if (!clientList[i].Equals(null))
                {
                    if (clientList[i].Equals(client_))
                    {
                        clientList.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
