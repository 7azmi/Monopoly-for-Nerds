namespace Monopoly.Interfaces;

[Flags]
public enum PlayerState
{
    Active = 1, 
    MyTurn = 2,
    ReadyForRolling = 4,
    InJail = 8,
    Moving = 16,
    Negotiating = 32
}