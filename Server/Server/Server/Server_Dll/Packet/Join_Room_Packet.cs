using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Packet
{
    
    public class Join_Room_Packet
    {
        public Join_Room_Packet()
        {
        }

        public string roomName { get; set; }
        public string userId { get; set; }
        public string nickName { get; set; }
        public int clientType { get; set; }
        public int avatarType { get; set; }
    }
}
