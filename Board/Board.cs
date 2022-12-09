using MonopolyTerminal.Interfaces;
using static MonopolyTerminal.Monopoly.Engine;
namespace MonopolyTerminal;

public partial class Monopoly
{
    public static partial class Board
    {
        private static Place[] _places = 
        {
            new Go(0),
            new Street(1,"street 1",60, 50, new[] {2000, 10, 30, 90, 160, 250}, 0),
            new Chest(2),
            new Street(3, "Street 3", 60, 50, new[] {4000, 20, 60, 180, 320, 450}, 0),
            new Tax(4, "Income Tax", 200),
            new Railroad(5, "RailRoad 5", 200),
            new Street(6, "Street 6",100, 50, new[] {6000, 30, 90, 270, 400, 550}, 1),
            new Chance(7),
            new Street(8, "Street 8", 100, 50, new[] {6000, 30, 90, 270, 400, 550}, 1),
            new Street(9, "Street 9", 120, 50, new[] {8000, 40, 100, 300, 450, 600}, 1),
            new Jail(10),
            new Street(11, "Street 11", 140, 100, new[] {10000, 50, 150, 450, 625, 750}, 2),
            new Company(12, "Electricity", 150),
            new Street(13, "Street 13",140, 100, new[] {10000, 50, 150, 450, 625, 750}, 2),
            new Street(14, "Street 14", 160, 100, new[] {12000, 60, 180, 500, 700, 900}, 2),
            new Railroad(15, "RailRoad 15", 200),
            new Street(16, "Street 16",180, 100, new[] {14000, 70, 200, 550, 750, 950}, 3),
            new Chest(17),
            new Street(18, "Street 18",180, 100, new[] {14000, 70, 200, 550, 750, 950}, 3),
            new Street(19, "Street 19",200, 100, new[] {1600, 80, 220, 600, 800, 1000}, 3),
            new FreeParking(20),
            new Street(21, "Street 21",220, 150, new[] {1800, 90, 250, 700, 875, 1050}, 4),
            new Chance(22),
            new Street(23, "Street 23",220, 150, new[] {1800, 90, 250, 700, 875, 1050}, 4),
            new Street(24, "Street 24",240, 150, new[] {2000, 100, 300, 750, 925, 1100}, 4),
            new Railroad(25, "Railroad 25", 200),
            new Street(26, "Street 26",260, 150, new[] {2200, 110, 330, 800, 975, 1150}, 5),
            new Street(27, "Street 27",260, 150, new[] {2200, 110, 330, 800, 975, 1150}, 5),
            new Company(28, "Water Work", 150),
            new Street(29, "Street 29",280, 150, new[] {2400, 120, 360, 850, 1025, 1200}, 5),
            new GotoJail(30),
            new Street(31, "Street 31",300, 200, new[] {2600, 130, 390, 900, 1100, 1275}, 6),
            new Street(32, "Street 32",300, 200, new[] {2600, 130, 390, 900, 1100, 1275}, 6), //Oxford
            new Chest(33),
            new Street(34, "Street 34",320, 200, new[] {2800, 150, 450, 1000, 1200, 1400}, 6),
            new Railroad(35, "Railroad 35", 200),
            new Chance(36),
            new Street(37, "Street 37",350, 200, new[] {3500, 175, 500, 1100, 1300, 1500}, 7),
            new Tax(38, "Luxurious Tax", 100),
            new Street(39, "Street 39", 400, 200, new[] {5000, 200, 600, 1400, 1700, 2000}, 7)
        };
        
        
        public static Place GetPlace(int index) => _places[index];
        public abstract class Place
        {
            protected Place(int index)
            {
                _index = index;
            }
            
            private readonly int _index; public int GetIndex() => _index;
            protected string Name; public string GetName() => Name;

            public Player[] GetSettlers()
            {
                var players = new List<Player>();

                foreach (var activePlayer in ActivePlayers)
                {
                    if (activePlayer.GetCurrentOccupation() == this) players.Add(activePlayer);
                }
                
                return players.ToArray();
            }

            public virtual void Land()
            {
                OnLanding?.Invoke(this);
            }
        }

        public class Go : Place
        {
            public Go(int index): base(index)
            {
                Name = "Go";
            }
            public override void Land()
            {
                //WhoseTurn.GetSalary(WhoseTurn, 200);
                base.Land();
                
                OnLandingCompleted?.Invoke(this);
            }
        }

        public class Tax : Place
        {
            private int _tax;

            public Tax(int index, string name, int tax): base(index)
            {
                Name = name;
                _tax = tax;
            }

            public int GetTax() => _tax;

            public override void Land()
            {
                base.Land();
                
                OnLandingCompleted?.Invoke(this);
            }
        }

        public class FreeParking : Place
        {
            public FreeParking(int index) : base(index)
            {
                Name = "Free Parking";
            }
            //if you feel useless in life, just remember this parking place
            public override void Land()
            {
                base.Land();
                
                OnLandingCompleted?.Invoke(this);
            }
        }

        public class GotoJail : Place
        {
            public GotoJail(int index) : base(index)
            {
                Name = "Goto Jail";
            }
            public override void Land()
            {
                base.Land();

                Bank.GoJail(WhoseTurn);
                
                //SwitchNextPlayerTurn();//duplicate
            }
        }
    }
}