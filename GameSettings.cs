namespace MonopolyTerminal;

using static Monopoly;

[Serializable]
public class GameSettings
{
     public GameSettings(Player[] players, float interestsRate = 1.1f, bool fullJailPolicy = false)
     {
          Players = players;
          InterestsRate = interestsRate;
          FullJailPolicy = fullJailPolicy;
     }
     public Player[] Players{ get; private set; }
     public int InitialMoney { get; private set; }
     public float InterestsRate { get; private set; } = 1.1f;
     public bool FullJailPolicy { get; private set; }
}