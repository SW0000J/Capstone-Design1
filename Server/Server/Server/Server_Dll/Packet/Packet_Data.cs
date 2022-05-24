using TCPServer.Server_Dll.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServer.Packet;

namespace TCPServer.Server_Dll.Packet
{
    public class Packet_Data
    {
        public Packet_Data()
        {
            
        }

        public int packet_Type { get; set; }
        public int packet_Length { get; set; }

        public string userId { get; set; }

        public Join_Room_Packet join_Room_Packet { get; set; }
        public Audio_Packet Audio_Packet { get; set; }
        public MikeList_Packet mikeList_Packet { get; set; }
        public Position_Packet position_Packet { get; set; }
        public Screen_Packet screen_Packet { get; set; }
        public Join_User_Packet join_User_Packet { get; set; }
    }
}
