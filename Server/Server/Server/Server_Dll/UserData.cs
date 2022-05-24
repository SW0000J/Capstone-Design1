using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll
{
    public class UserData
    {
        public string nickName; // 유저 이름
        public string roomName; // 강의실 이름

        public string userID;  // 유저 아이디
        public int ClientType; // 선생인지 학생인지
        public int AvatarType; // 아바타 타입

        public UserData()
        {
            init();
        }

        private void init()
        {
            nickName = "";
            roomName = "";
            userID = "";
            ClientType = 0;
            AvatarType = 0;
        }
    }
}
