namespace MonopolyTerminal.Interfaces;

[Flags]
public enum PlayerState
{
    Dead = 0, 
    MyTurn = 1,
    ReadyForRolling = 2,
    InJail = 4,
    Moving = 8,
    Negotiating = 16
}