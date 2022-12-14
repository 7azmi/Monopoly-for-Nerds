using Monopoly.Human;
using Monopoly.Interfaces;
using static Monopoly.Monopoly.Board;

namespace Monopoly;

public partial class Monopoly
{
    public partial class Player
    {
        public Player(string name, ConsoleColor label, int money, Input input = null)
        {
            _name = name;
            _label = label;
            _money = money;
            _input = input;
            _input.Init(this);
            _properties = new List<Board.Property>();
        }
        
        private string _name;
        //for console
        private ConsoleColor _label;
        //private Color _label;
        private int _money;
        //private int _currentPlace = 0;
        private Input _input;
        public Input GetInput() => _input;
        public PlayerState State;
        private List<Property> _properties;
        public bool MyTurn => State.HasFlag(PlayerState.MyTurn);
        public bool IsBot => _input is not Humanoid;
        public bool HasJailFreeCard { get; set; }
        public bool InJail => Board.Jail.InJail(this);
        public bool IsBroke => !ActivePlayers.Contains(this);
        public string GetName() => _name;
        public void AddMoney(int amount) => _money += amount;
        private Place _currentOccupation;
        public ConsoleColor GetLabel() => _label;
        public List<Property> Properties => _properties;
        public List<Street> Streets => _properties.Where(property => property is Street).Cast<Street>().ToList();
        public void SetCurrentOccupation(Place place) => _currentOccupation = place;
        public Place GetCurrentOccupation() => _currentOccupation;
        public void SetCurrentOccupationByIndex(int index) => _currentOccupation = GetPlace(index);
        public int GetCurrentOccupationByIndex() => _currentOccupation.GetIndex();
        public void SpendMoney(int amount) => _money -= amount;
        public bool HasEnoughMoney(int amount) => _money >= amount;
        public void AddProperty(Board.Property prop) => _properties.Add(prop);
        public void SetStartingOccupation(int index) => SetCurrentOccupation(GetPlace(index));
        public int TotalNetWorth
        {
            get
            {
                var netWorth = _money;
                foreach (var property in _properties)
                {
                    if (property is Street street) _money += street.HouseCount * (street.GetHousePrice() / 2);
                    _money += property.GetPrice() - (property.IsMortgaged() ? property.MortgageValue : 0);
                }
                return netWorth;
            }
        }

        public bool HasSet => _properties.Any(property => property is Street s && s.GetSetOwner() == this);

        public int GetMoney() => _money;
    }
}