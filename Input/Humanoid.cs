using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.TypingSimulator;
namespace MonopolyTerminal.Human;

public class Humanoid : Input
{
    public Humanoid(IPlatform platform) : base(platform)
    {
        Platform = platform;
    }

    public override async Task OnDiceReady()
    {
        Platform.ReadInput();
        
    }
}