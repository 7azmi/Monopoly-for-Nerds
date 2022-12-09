using Monopoly.Interfaces;

namespace Monopoly;

public partial class Monopoly
{
    public static partial class Board
    {
        public class Jail : Place
        {
            private static List<Prisoner> _prisoners = new(); //may change this to dictionary

            public Jail(int index) : base(index)
            {
                Name = "Jail";
                _prisoners = new List<Prisoner>();
            }

            public void GetHimIn(Player player)
            {
                _prisoners.Add(new Prisoner(player));
                player.State &= ~PlayerState.InJail;
            }

            public void GetemOut(Player player)
            {
                var prisoner = _prisoners.FirstOrDefault(prisoner => prisoner.GetPrisoner() == player);
                _prisoners.Remove(prisoner);

                player.State |= ~PlayerState.InJail;
            }

            public static Prisoner GetPrisoner(Player player)
            {
                //prisoners.FirstOrDefault(prisoner => prisoner.GetPrisoner() == player);//don't use code you don't know
                foreach (var prisoner in _prisoners)
                {
                    if (prisoner.GetPrisoner() == player)
                    {
                        return prisoner;
                    }
                }

                return _prisoners.Last(); //won't reach here anyway.
            }

            public static bool InJail(Player player) => _prisoners.Any(p => p.GetPrisoner() == player);

            public override void Land()
            {
                base.Land();

                Engine.OnLandingCompleted?.Invoke(this);
            }

            public static void StayInJail(Player player) => GetPrisoner(player).Stay();

            public class Prisoner
            {
                private Player _player;
                private int _times = 0;

                public Prisoner(Player prisoner)
                {
                    _player = prisoner;
                }

                public Player GetPrisoner() => _player;
                public int GetHowManyRoundsPlayerIsInJail() => _times;

                public void Stay()
                {
                    _times++;
                }

                public bool ExceededJailPeriod() => _times >= 2;
            }
        }
    }
}