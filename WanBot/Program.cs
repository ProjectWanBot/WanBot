using Grpc.Core;
using WanBot.Adapter.Api.Protobuf;

namespace WanBot;

class Program
{
    static void Main(string[] args)
    {
        var server = new Server
        {
            Services = { ChatAgent.BindService(new ChatAgentService()) },
            Ports = { new ServerPort("localhost", 8081, ServerCredentials.Insecure) }
        };
        
        server.Start();
        server.ShutdownTask.Wait();
    }
}
