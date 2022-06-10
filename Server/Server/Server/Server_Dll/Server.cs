using TCPServer.Server_Dll.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer.Server_Dll
{
    class Server
    {
        public int m_Port = 7777;
        private TcpListener m_TcpListener;

        private RoomManager mRoomManager;
        private Thread m_ThrdtcpListener;
        private TcpClient m_Client;

        public Server()
        {
            Init();
        }

        private void Init()
        {
            mRoomManager = new RoomManager();

            m_ThrdtcpListener = new Thread(new ThreadStart(ListenForIncommingRequests));
            m_ThrdtcpListener.IsBackground = true;
            m_ThrdtcpListener.Start();

            Console.WriteLine("TCP Server Start....");
        }

        public void OnApplicationQuit()
        {
            m_ThrdtcpListener.Abort();

            if (m_TcpListener != null)
            {
                m_TcpListener.Stop();
                m_TcpListener = null;
            }
        }

        void ListenForIncommingRequests()
        {
            m_TcpListener = new TcpListener(IPAddress.Any, m_Port);
            m_TcpListener.Start();
            ThreadPool.QueueUserWorkItem(ListenerWorker, null);
        }

        void ListenerWorker(object token)
        {
            while (m_TcpListener != null)
            {
                m_Client = m_TcpListener.AcceptTcpClient();

                Client client = new Client(m_Client);

                ThreadPool.QueueUserWorkItem(HandleClientWorker, client);

            }
        }

        private void CheckPacket(byte[] byte_, Client client)
        {
            Packet_Data packet = Packet_Utility.Deserialize(byte_) as Packet_Data;

            if (packet == null)
            {
                return;
            }

            switch ((int)packet.packet_Type)
            {
                case (int)PacketType.JoinRoom: // 방입장 후, 닉네임 설정. 만약 방이 없으면 첫번째 들어온 사람이 방을 생성

                    client.SetUserInfo(packet.join_Room_Packet.nickName, packet.join_Room_Packet.roomName, packet.join_Room_Packet.userId, packet.join_Room_Packet.clientType, packet.join_Room_Packet.avatarType);


                    mRoomManager.JoinRoom(packet.join_Room_Packet.roomName, client);

                    //Console.WriteLine("몇 명 접속   " + mRoomManager.roomList[0].clientList.Count + "  /   패킷 사이즈 : " + byte_.Length + " 바이트");*/

                    Packet_Data packet_data = new Packet_Data();

                    Join_User_Packet join_user_packet = new Join_User_Packet();

                    packet_data.packet_Type = (int)PacketType.JoinUser;

                    for (int i = GetClientRoom(client).clientList.Count - 1; i >= 0; i--)
                    {
                        if (!GetClientRoom(client).clientList.Count.Equals(null))
                        {
                            join_User_Data join_user_data = new join_User_Data(GetClientRoom(client).clientList[i].userData);
                            join_user_packet.join_User_Data.Add(join_user_data);

                        }
                    }

                    packet_data.join_User_Packet = join_user_packet;

                    byte[] byte__ = Packet_Utility.Serialize(packet_data);


                    SendMessage(byte__, client, true);

                    break;

                case (int)PacketType.JoinUser: // 유저가 방에 들어왔을 때

                    SendMessage(byte_, client, true);


                    break;

                case (int)PacketType.Position: // 캐릭터 위치 동기화

                    SendMessage(byte_, client, true);

                    string str = Encoding.Default.GetString(byte_);

                    Console.WriteLine(str);

                    break;

                case (int)PacketType.Screen: // 화면 공유

                    SendMessage(byte_, client, false);

                    break;

                case (int)PacketType.MikeList: // 마이크 리스트 동기화, 강의자가 설정한 학생들의 마이크

                    SendMessage(byte_, client, false);

                    break;
            }
        }

        void HandleClientWorker(object token)
        {
            byte[] bytes = new byte[207];

            Client client = token as Client;

                
            using (var stream = client.tcpClient.GetStream())
            {
                int length = 0;

                int currentOffset = 0;

                byte[] length_bytes = new byte[4];

                byte[] send_bytes = { };
                int len = 0;

                int length_byte_count = 0;
                int send_byte_count = 0;

                bool flag_length_byte = false;
                bool flag_send_byte = false;

                try
                {

                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        currentOffset = 0;
                        while (true)
                        {
                            if (flag_send_byte == true)
                            {
                                flag_send_byte = false;
                            }
                            else if (flag_send_byte == false)
                            {
                                for (int i = length_byte_count; i < length_bytes.Length; i++)
                                {
                                    length_byte_count = i;
                                    length_bytes[i] = bytes[currentOffset];
                                    currentOffset++;

                                    if (currentOffset >= length)
                                    {
                                        length_byte_count++;
                                        flag_length_byte = true;
                                        break;
                                    }
                                }
                            }

                            if (flag_length_byte == true)
                            {
                                flag_length_byte = false;
                                break;
                            }

                            if (send_byte_count == 0)
                            {
                                length_byte_count = 0;
                                len = BitConverter.ToInt32(length_bytes, 0);

                                send_bytes = new byte[len];
                            }

                            ///////////////////////////////


                            bool ifBreak = false;
                            int test = send_byte_count;
                            for (int i = send_byte_count; i < len; i++)
                            {
                                send_byte_count = i;
                                send_bytes[send_byte_count] = bytes[currentOffset];
                                currentOffset++;

                                if (currentOffset >= length)
                                {
                                    if (send_byte_count == len - 1)
                                    {
                                        ifBreak = true;
                                        break;
                                    }
                                    send_byte_count++;
                                    flag_send_byte = true;
                                    break;
                                }
                            }

                            if (flag_send_byte == true && ifBreak == false)
                            {
                                break;
                            }

                            CheckPacket(send_bytes, client);

                            string data = Encoding.Default.GetString(send_bytes, 0, send_bytes.Length);

                            send_byte_count = 0;

                            //Console.WriteLine(data + "     /  " + data.Length);
                            //Console.WriteLine("몇 명 접속   " + mRoomManager.roomList[0].clientList.Count);

                            if (ifBreak == true)
                            {
                                break;
                            }
                        }
                        stream.Flush();
                    }

                    GetClientRoom(client).ExitUser(client);

                    if (m_Client == null)
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erorr : " + e);
                    GetClientRoom(client).ExitUser(client);
                }
            }
        }

        Room GetClientRoom(Client client) // 클라이언트가 속한 룸을 찾는 함수
        {
            Room room = new Room("");

            for (int i = 0; i < mRoomManager.roomList.Count; i++)
            {
                if (mRoomManager.roomList[i].roomName.Equals(client.userData.roomName)) // 연결 끊길 시
                {
                    return mRoomManager.roomList[i];
                }
            }

            return room;
        }

        void SendMessage(byte[] byte_, Client client_, bool isAll)
        {
            if (m_Client == null)
                return;

            byte[] length = BitConverter.GetBytes(byte_.Length);

            byte[] merge = new byte[length.Length + byte_.Length];
            Array.Copy(length, 0, merge, 0, length.Length);
            Array.Copy(byte_, 0, merge, length.Length, byte_.Length);

            for (int i = GetClientRoom(client_).clientList.Count - 1; i >= 0; i--)
            {
                if (GetClientRoom(client_).clientList[i].Equals(null))
                {
                    GetClientRoom(client_).clientList.RemoveAt(i);
                }
                if (isAll == true)
                {
                    try
                    {
                        NetworkStream stream = GetClientRoom(client_).clientList[i].tcpClient.GetStream();
                        if (stream.CanWrite)
                        {
                            stream.Write(merge, 0, merge.Length);
                            // Console.WriteLine(byte_.Length);
                        }
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                else if (isAll == false)
                {
                    if (GetClientRoom(client_).clientList[i] != client_)
                    {
                        try
                        {
                            NetworkStream stream = GetClientRoom(client_).clientList[i].tcpClient.GetStream();
                            if (stream.CanWrite)
                            {
                                stream.Write(merge, 0, merge.Length);
                                // Console.WriteLine(byte_.Length);
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }

            }
        }
    }
}