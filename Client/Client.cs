using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        IPEndPoint serverEndPoint;
        Socket socket;

        public Client(string address, int port)
        {
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Process()
        {
            while (true)
            {
                Console.WriteLine("Insert first number");
                float firstNum = Convert.ToSingle(Console.ReadLine());

                Console.WriteLine("Insert second number");
                float secondNum = Convert.ToSingle(Console.ReadLine());

                Console.WriteLine("Select operator (0 = +, 1 = -, 2 = *, 3 = /");
                byte command = Convert.ToByte(Console.ReadLine());

                Packet packet = new Packet(command, firstNum, secondNum);

                byte[] data = packet.GetData();
                socket.SendTo(data, serverEndPoint);

                byte[] response = new byte[data.Length];
                int rlen = socket.Receive(response);
                Console.WriteLine(BitConverter.ToSingle(response, 1));
            }
        }

        string StringFromBytes(byte[] data)
        {
            return new string(Encoding.UTF8.GetChars(data, 0, data.Length));
        }
        
        byte[] BytesFromString(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }
    }
}
