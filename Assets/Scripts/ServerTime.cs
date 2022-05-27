using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Scripts
{
    public class ServerTime
    {
        public static bool GetTime(string ntpServer, out DateTime time)
        {
            var ntpData = new byte[48];
            
            ntpData[0] = 0x1B;

            IPAddress[] addresses;
            try
            {
                addresses = Dns.GetHostEntry(ntpServer).AddressList;
            }
            catch
            {
                time = DateTime.MinValue;
                return false;
            }
            
            
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                
                socket.ReceiveTimeout = 3000;
 
                socket.Send(ntpData);
                try
                {
                    socket.Receive(ntpData);
                }
                catch
                {
                    time = DateTime.MinValue;
                    return false;
                }
                
            }
            
            const byte serverReplyTime = 40;
            
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
            
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);
            
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);
 
            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            time = networkDateTime.ToLocalTime();
            return true;
        }

        public static bool GetTime(string[] ntpServers, out DateTime time)
        {
            foreach (var ntpServer in ntpServers)
                if (GetTime(ntpServer, out time))
                    return true;
            
            time = DateTime.MinValue;
            return false;
        }
        
        private static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                          ((x & 0x0000ff00) << 8) +
                          ((x & 0x00ff0000) >> 8) +
                          ((x & 0xff000000) >> 24));
        }
    }
}