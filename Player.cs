using System.Drawing;
using static Monopoly_for_Nerds.Monopoly.Board;

namespace Monopoly_for_Nerds;

public partial class Monopoly
{
    public partial class Player
    {
        public Player(string name, ConsoleColor label, int money, bool isBot)
        {
            _name = name;
            _label = label;
            _money = money;
            _isBot = isBot;
            _properties = new List<Board.Property>();
        }
        
        private string _name;
        
        //for console
        private ConsoleColor _label;
        
        //private Color _label;
        private int _money;
        //private int _currentPlace = 0;
        private bool _isBot;

        private bool _inJail = false;
        private List<Board.Property> _properties;

        public bool MyTurn => this == WhoseTurn;
        public bool IsBot => _isBot;
        public bool InJail => Board.Jail.InJail(this);
        public string GetName() => _name;
        public void AddMoney(int salary)
        {
            _money += salary;
        }

        private Place _currentOccupation;

        public ConsoleColor GetLabel() => _label;
        public void SetCurrentOccupation(Place place) => _currentOccupation = place;
        public Place GetCurrentOccupation() => _currentOccupation;

        public void SetCurrentOccupationByIndex(int index) => _currentOccupation = Board.GetPlace(index);
        public int GetCurrentOccupationByIndex() => _currentOccupation.GetIndex();
        public void SpendMoney(int amount) => _money -= amount;

        public bool HasEnoughMoney(int amount) => _money >= amount;

        public void AddProperty(Board.Property prop) => _properties.Add(prop);

        public void SetStartingOccupation(int index) => SetCurrentOccupation(GetPlace(index));


        public int GetMoney() => _money;
    }
}