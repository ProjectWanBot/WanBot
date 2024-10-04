using WanBot.Adapter.Api.CSharp;
using WanBot.Adapter.Api.Protobuf;
    
var adapter = new WanBot.Adapter.ConsoleChat.ConsoleChatAgent();
adapter.Connect("http://localhost:8081");
adapter.Start();
while (true) {
    var evt = new ChatEvent {
        ReceiveChat = new ChatEvent.Types.EvtReceiveChat {
            Chain = new MessageChain {
                Elements = {
                    new MessageChain.Types.MessageBlock {
                        Text =
                            new MessageChain.Types.MessageBlock.Types.MsgText {
                                Plain = Console.ReadLine()!
                            }
                    }
                }
            }
        }
    };
    await adapter.SendEvent(evt);
}

namespace WanBot.Adapter.ConsoleChat {
    class ConsoleChatAgent : ChatAgentBase {
        private static void PrintMessageChain(MessageChain chain) {
            foreach (var elem in chain.Elements) {
                switch (elem.ContentCase) {
                    case MessageChain.Types.MessageBlock.ContentOneofCase.Mention:
                        Console.Write($"@{elem.Mention.Nickname}({elem.Mention.Member}) ");
                        break;
                    case MessageChain.Types.MessageBlock.ContentOneofCase.Text:
                        Console.Write(elem.Text.Plain);
                        break;
                }
            }
        }
        protected override Task OnReceiveCommandAsync(ChatCommand cmd) {
            if (cmd.PayloadCase == ChatCommand.PayloadOneofCase.PostChat)
                PrintMessageChain(cmd.PostChat.Chain);
            return Task.CompletedTask;
        }
    }
}