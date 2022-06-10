using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll.Packet
{
    public class Position_Packet
    {
        public bool LeftEyes { get; set; }
        public bool RightEyes { get; set; }
        public bool Mouse { get; set; }
        public float HeadPos_X { get; set; }
        public float HeadPos_Y { get; set; }
    }
}
