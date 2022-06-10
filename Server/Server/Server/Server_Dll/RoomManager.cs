using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll
{
    class RoomManager
    {
        public List<Room> roomList;
        public RoomManager()
        {
            roomList = new List<Room>();
        }

        public void CreateRoom(string roomName_)
        {
            Room room = new Room(roomName_);

            roomList.Add(room);
        }

        public void JoinRoom(string roomName_, Client client_)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].roomName.Equals(roomName_))
                {
                    roomList[i].JoinUser(client_);

                    return;
                }
            }

            CreateRoom(roomName_);
            roomList[roomList.Count - 1].JoinUser(client_);
        }

        public void ExitRoom(string roomName_)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].roomName.Equals(roomName_))
                {
                    roomList[i].clientList.Clear();
                    roomList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
