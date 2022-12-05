using MonopolyTerminal.Human;
using MonopolyTerminal.Interfaces;
using static MonopolyTerminal.Monopoly.Board;

namespace MonopolyTerminal;

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

        public PlayerState PlayerState;

        private List<Property> _properties;

        public bool MyTurn => this == WhoseTurn;
        public bool IsBot => _input is not Humanoid;
        public bool HasJailFreeCard { get; set; }
        public bool InJail => Board.Jail.InJail(this);

        public bool IsBroke => !ActivePlayers.Contains(this);
        
        public string GetName() => _name;
        public void AddMoney(int amount)
        {
            _money += amount;
        }

        private Place _currentOccupation;

        public ConsoleColor GetLabel() => _label;

        public List<Property> Properties => _properties; 
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
        public int GetMoney() => _money;
    }
}