using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll
{
    class Client
    {
        UserData mUserData;

        public TcpClient tcpClient;

        public UserData userData
        {
            get
            {
                return mUserData;
            }
        }

        public Client(TcpClient tcpClient_)
        {
            tcpClient = tcpClient_;

            mUserData = new UserData();
        }

        public void SetUserInfo(string nickName_, string roomName_, string userID_, int clientType_, int avatarType_)
        {
            mUserData.nickName = nickName_;
            mUserData.roomName = roomName_;
            mUserData.userID = userID_;
            mUserData.ClientType = clientType_;
            mUserData.AvatarType = avatarType_;
        }
    }
}
