using System.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Net.Client;
using WanBot.Adapter.Api.Protobuf;

namespace WanBot.Adapter.Api.CSharp;

public abstract class ChatAgentBase : IDisposable {
    private HttpClient _httpClient;
    private GrpcChannel? _channel;
    private ChatAgent.ChatAgentClient? _client;
    private AsyncServerStreamingCall<ChatCommand>? _cmdStream;
    private AsyncClientStreamingCall<ChatEvent, Empty>? _evtStream;
    
    public ChatAgentBase() {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestVersion = HttpVersion.Version20;
        _httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    }
    public void Connect(string address) {
        _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions {HttpClient = _httpClient});
        _client = new ChatAgent.ChatAgentClient(_channel);
        var version = _client.GetVersion(new Empty());
        Console.WriteLine(version);
        _cmdStream = _client.StartChatCommandStream(new Empty());
        _evtStream = _client.StartChatEventStream();
    }
    protected abstract Task OnReceiveCommandAsync(ChatCommand cmd);
    public async Task SendEvent(ChatEvent evt) {
        await _evtStream?.RequestStream.WriteAsync(evt)!;
    }
    public void Start() {
        _cmdStream?.ResponseStream.ForEachAsync(OnReceiveCommandAsync);
    }
    public void Dispose() {
        _cmdStream?.Dispose();
        _evtStream?.Dispose();
        _channel?.Dispose();
    }
}