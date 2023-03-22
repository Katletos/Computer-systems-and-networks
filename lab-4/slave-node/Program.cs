using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using master_node;

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
    await GetNewTask(writer);
    await SendTaskResult(writer, Process(reader));
    
}

async Task GetNewTask(StreamWriter writer)
{
    await writer.WriteLineAsync("<FREE>");
    await writer.FlushAsync();
    Console.WriteLine("Send request for a new task.");
}

CalculationTask Process(StreamReader reader)
{
    var str = reader.ReadLine();
    var task = JsonSerializer.Deserialize<CalculationTask>(str); 
    Console.Write("Readed task: ");
    Console.WriteLine(task.ToString());
    task.IsPrime();
    Console.WriteLine(task.ToString());
    return task;
}

async Task SendTaskResult(StreamWriter writer, CalculationTask task)
{
    await writer.WriteLineAsync(JsonSerializer.Serialize(task));
    await writer.FlushAsync();
    
    Console.WriteLine("Send completed task.");
}

bool IsPrime(ulong number)
{
    for (uint i = 0; i < Math.Round(Math.Sqrt(number)); ++i)
    {
        if (number % i == 0)
        {
            return true;
        }
    }

    return false;
}