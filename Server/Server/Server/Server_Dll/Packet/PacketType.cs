using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll.Packet
{
    enum PacketType : int
    {
        JoinRoom,
        Position,
        Screen,
        Audio,
        MikeList,
        JoinUser,
        ExitUser,
    }
}
