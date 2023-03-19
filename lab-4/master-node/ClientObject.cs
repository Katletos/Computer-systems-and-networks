using System.Net.Sockets;

namespace master_node;

public class ClientObject
{
    private string Id { get;} 
    private StreamWriter Writer { get;}
    private StreamReader Reader { get;}

    private ServerObject _server;
    private Socket _client;
    
    public ClientObject(Socket client, ServerObject server)
    {
        _client = client;
        _server = server;
        Id = Guid.NewGuid().ToString();
        
        NetworkStream stream = new NetworkStream(_client);
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
    }

    public Task ProcessAsync()
    {
        return Task.Delay(500);
    }
}