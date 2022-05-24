using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll.Packet
{
    public class Audio_Packet 
    {
        public Audio_Packet()
        { 
        }
        public byte[] audio_bytes { get; set; }
    }
}
