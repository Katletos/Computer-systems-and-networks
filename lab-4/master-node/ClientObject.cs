using System.Net.Sockets;

namespace master_node;

public class ClientObject
{
    private string Id;
    public StreamReader reader;
    public StreamWriter writer;
    private ServerObject _server;
    private Socket _client;
    
    public ClientObject(Socket client, ServerObject server)
    {
        _client = client;
        _server = server;
        Id = Guid.NewGuid().ToString();
        
        NetworkStream stream = new NetworkStream(_client);
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        new StreamWriter(stream);
    }

    public void SendTask(CalculationTask task)
    {
        Thread.Sleep(500);
    }
}