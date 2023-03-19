using System.Net;
using System.Net.Sockets;
using System.Text;

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 11_000);

using Socket client = new(
    ipEndPoint.AddressFamily, 
    SocketType.Stream, 
    ProtocolType.Tcp);


await client.ConnectAsync(ipEndPoint);
NetworkStream stream = new NetworkStream(client);


while (true)
{
    StreamReader reader = new StreamReader(stream);
    StreamWriter writer = new StreamWriter(stream);
    if (reader is null || writer is null) return;
    
    // запускаем новый поток для получения данных
    Task.Run(()=>ReceiveMessageAsync(reader));
    // запускаем ввод сообщений
    await SendMessageAsync(writer);
}


async Task ReceiveMessageAsync(StreamReader reader)
{
    while (true)
    {
        try
        {
            
        }
        catch
        {
            break;
        }
    }
}


async Task SendMessageAsync(StreamWriter writer)
{
    // сначала отправляем имя
    await writer.WriteLineAsync();
    await writer.FlushAsync();
    Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
 
    while (true)
    {
        string? message = Console.ReadLine();
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}
