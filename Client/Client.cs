using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Client
{
    public void Start()
    {
        var hostName = Dns.GetHostName();
        IPHostEntry localHost = Dns.GetHostEntry(hostName);

        IPAddress localHostAddress = localHost.AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(localHostAddress, 2469);

        using Socket client = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

        client.Connect(ipEndPoint);

        Console.WriteLine("[CLIENTE] Conectado ao servidor. Você pode começar a enviar mensagens.");

        Thread receiveThread = new Thread(() =>
        {
            while (true)
            {
                var buffer = new byte[1024];
                var received = client.Receive(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("[SERVIDOR]: " + response);
                }
            }
        });
        receiveThread.Start();

        while (true)
        {
            var message = Console.ReadLine();
            var messageBytes = Encoding.UTF8.GetBytes(message);
            client.Send(messageBytes);
        }
    }
}
