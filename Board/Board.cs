using MonopolyTerminal.Interfaces;
using static MonopolyTerminal.Monopoly.Engine;
namespace MonopolyTerminal;

public partial class Monopoly
{
    public static partial class Board
    {
        private static Place[] _places = new Place[]
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

        public abstract class Property : Place
        {
            protected Property(int index, int price) : base(index)
            {
                Price = price;
            }
            
            private bool _mortgaged;
            private Player _owner;
            protected int Price;
            public bool Owned => GetOwner() != null;
            public int MortgageValue => Price / 2;
            public int UnmortgageValue => (int)((float)MortgageValue * 1.1f);//that's what it's all about

                
            public override void Land()
            {
                base.Land();
                
                LandOnProperty();
            }
            private void LandOnProperty()
            {
                if (Owned)
                {
                    if (GetOwner() != WhoseTurn)
                    {
                        if(!IsMortgaged()) RequestRental();
                        else OnLandingCompleted.Invoke(this);
                    }
                    else OnLandingOnMyProperty?.Invoke(WhoseTurn, this);//home sweet home
                }
                else OnLandingOnUnownedProperty?.Invoke(this);
            }
            private void RequestRental() => OnRentalDue?.Invoke(this, WhoseTurn, GetRentalValue());
            public void SetOwner(Player newOwner) => _owner = newOwner;
            public Player GetOwner() => _owner;
            public bool HasOwner => _owner != null;
            public int GetPrice() => Price;
            public bool IsMortgaged() => _mortgaged;
            public bool CanBeMortgaged() => !IsMortgaged();
            public void Mortgage()
            {
                _mortgaged = true;
            }
            public bool CanBeUnmortgaged(Player player) => player.HasEnoughMoney(UnmortgageValue) && IsMortgaged();
            public abstract int GetRentalValue();

            public void Unmortgage()
            {
                _mortgaged = false;
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

        public class Street : Property
        {
            public Street(int index, string name, int price, int housePrice, int[] rental, int setIndex) : base(index, price)
            {
                Name = name;
                Price = price;
                _housePrice = housePrice;
                _rental = rental;
                _setIndex = setIndex;
            }
            private int _housePrice;
            private int _houses = 0; public bool NoHouses => _houses == 0; public bool MaxHouses => _houses == 5; 
            public bool HasHouses => _houses > 0; public int HouseCount => _houses;

            private int _setIndex;
            public int SetIndex => _setIndex;

            public Street[] GetStreetSet()
            {
                var set = new List<Street>();
                foreach (var place in _places)
                {
                    if (place is Street s && s._setIndex == _setIndex) set.Add(s);
                }
                return set.ToArray();
            }

            private int[] _rental; //6
            public new bool CanBeMortgaged() => !IsMortgaged() && NoHouses;



            public int[] Rentals => _rental;

            public bool PlayerCanBuildHouseHere => GetOwner().HasEnoughMoney(_housePrice) 
                                                   && IsCompleteSetProperty 
                                                   && HouseCanBeBuiltOrderRule;
            public bool HouseCanBeBuiltOrderRule =>
                GetStreetSet().Any(street => street._houses > _houses && street != this)
                || GetStreetSet().All(street => street._houses == _houses);
            
            //public bool HouseOrder

            public bool IsCompleteSetProperty => GetStreetSet().All(s => s.Owned && s.GetOwner() == GetOwner());
            public override int GetRentalValue()
            {
                return _rental[_houses];
            }
            public void AddHouse() => _houses++;//yeah!
            public int GetHousePrice() => _housePrice;

            public void RemoveHouse() => _houses--;

            public Player GetSetOwner()
            {
                var set = GetStreetSet();
                Player owner = GetStreetSet().First().GetOwner();
                
                foreach (var street in set)
                {
                    if (street.GetOwner() == null || street.GetOwner() != owner) return null;
                }

                return owner;
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

        public class Railroad : Property
        {
            public Railroad(int index, string name, int price) : base(index, price)
            {
                Name = name;
            }
            public override int GetRentalValue()
            {
                var rental = 25;
                for (var i = 5; i <= 35; i += 10)
                {
                    var railroad = _places[i] as Railroad;
                    if (railroad.GetOwner() == GetOwner() && railroad != this)
                    {
                        rental *= 2;
                    }
                }
                return rental;
            }
        }

        public class Jail : Place
        {
            private static List<Prisoner> _prisoners = new ();//may change this to dictionary
            
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

                return _prisoners.Last();//won't reach here anyway.
            }

            public static bool InJail(Player player) => _prisoners.Any(p => p.GetPrisoner() == player);

            public override void Land()
            {
                base.Land();
                
                OnLandingCompleted?.Invoke(this);
            }

            public static void StayInJail(Player player) =>GetPrisoner(player).Stay();
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

        public class Company : Property
        {
            public Company(int index, string name, int price) : base(index, price)
            {
                Name = name;
            }
            public override int GetRentalValue()
            {
                var elec = (Property) _places[12];
                var water = (Property) _places[28];

                if (elec.GetOwner() == water.GetOwner()) return Dice.SumDice() * 10;
                else return Dice.SumDice() * 4;
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