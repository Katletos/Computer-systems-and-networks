namespace master_node;

using System.Net;
using System.Net.Sockets;
public class ServerObject
{
    private IPEndPoint _ipEndPoint;
    private Socket _listener;
    private List<ClientObject> _clients;
    private Queue<CalculationTask> _calculationTasks;
    
    public ServerObject(string serverName, int port)
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(serverName);
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        _ipEndPoint = new(ipAddress, port);
        
        _listener = new(
            _ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        
        _clients = new List<ClientObject>();;
        _calculationTasks = new Queue<CalculationTask>();
    }
    
    public async Task ListenAsync()
    {
        _listener.Bind(_ipEndPoint);
        _listener.Listen(16);
        Console.WriteLine("Server started. Waiting connections...");
        
        while (true)
        { 
            Socket client = await _listener.AcceptAsync();
            ClientObject clientObject = new ClientObject(client, this);
            _clients.Add(clientObject);
            Console.WriteLine("New client connected");

            while (true)
            {
                while (_calculationTasks.Count < 10)
                {
                    _calculationTasks.Enqueue(CalculationTask.GetNextTask(_calculationTasks));
                }
                await Task.Run(clientObject.ProcessAsync);
            }
        }
    }
}