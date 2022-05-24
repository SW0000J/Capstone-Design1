using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TCPServer.Server_Dll.Packet
{
    public class Packet_Utility
    {
        public Packet_Utility()
        {

        }

        public static byte[] Serialize(Object data)
        {
            try
            {
                return JsonSerializer.SerializeToUtf8Bytes(data);
            }
            catch
            {
                return null;
            }
        }

        public static Object Deserialize(byte[] data)
        {
            try
            {
                return JsonSerializer.Deserialize<Packet_Data>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
                return null;
            }
        }
    }
}
