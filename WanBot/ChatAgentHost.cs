using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using WanBot.Adapter.Api.Protobuf;
using Version = WanBot.Adapter.Api.Protobuf.Version;

namespace WanBot;

public class ChatAgentService : ChatAgent.ChatAgentBase {
    public override async Task<Empty> StartChatEventStream(IAsyncStreamReader<ChatEvent> requestStream, ServerCallContext context) {
        while (await requestStream.MoveNext())
            Console.WriteLine(requestStream.Current);
        return new Empty();
    }
    
    public override Task StartChatCommandStream(Empty request, IServerStreamWriter<ChatCommand> responseStream, ServerCallContext context) {
        return Task.CompletedTask;
    }
    
    public override Task<Empty> Verify(Empty request, ServerCallContext context) {
        return Task.FromResult(new Empty());
    }
    
    public override Task<Version> GetVersion(Empty request, ServerCallContext context) {
        return Task.FromResult(new Version { Version_ = "1.0.0" });
    }
}