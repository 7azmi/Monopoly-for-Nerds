using MonopolyTerminal.Enums;

namespace MonopolyTerminal.Terminal;

public interface IConsoleRenderer
{
    void RenderCard(Card card);
}