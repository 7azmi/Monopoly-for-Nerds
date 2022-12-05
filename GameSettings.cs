namespace MonopolyTerminal;

using static Monopoly;

[Serializable]
public class GameSettings
{
     public GameSettings(Player[] players, float interestsRate = 1.1f, bool fullJailPolicy = false, int startingPoint = 0)
     {
          Players = players;
          InterestsRate = interestsRate;
          FullJailPolicy = fullJailPolicy;
          StartingPoint = startingPoint;
     }
     public Player[] Players{ get; private set; }
     public int InitialMoney { get; private set; }
     public float InterestsRate { get; private set; } = 1.1f;
     public bool FullJailPolicy { get; private set; }
     public int StartingPoint { get; private set; }
}