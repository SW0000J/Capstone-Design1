using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll.Packet
{
    public class join_User_Data
    {
        public join_User_Data(UserData userData_)
        {
            roomName = userData_.roomName;
            userId = userData_.userID;
            nickName = userData_.nickName;
            clientType = userData_.ClientType;
            AvatarType = userData_.AvatarType;
        }

        public string roomName { get; set; }
        public string userId { get; set; }
        public string nickName { get; set; }
        public int clientType { get; set; }
        public int AvatarType { get; set; }
    }


    public class Join_User_Packet
    {
        public List<join_User_Data> join_User_Data = new List<join_User_Data>();
    }
}
