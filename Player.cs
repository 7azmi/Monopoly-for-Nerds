using System.Drawing;

namespace Monopoly_for_Nerds;

public partial class Monopoly
{
    public class Player
    {
        public Player(string name, Color label, int money)
        {
            _name = name;
            _label = label;
            _money = money;

            _properties = new List<Board.Property>();
        }

        private string _name;
        private Color _label;
        private int _money;
        private int _currentPlace = 0;

        private bool _inJail = false;
        private List<Board.Property> _properties;
        public char name;

        public bool MyTurn => this == WhoseTurn;
        public bool IsBot { get; set; }
        public bool InJail => Board.Jail.InJail(this);


        public abstract class Command
        {
            protected Player _player;

            public virtual void Execute()
            {
                if (!IsLegal()) return;
            }

            public abstract bool IsLegal();
        }

        public class RollDice : Command
        {
            //public RollDice(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                Dice.Roll();
            }

            public override bool IsLegal()
            {
                return _player == WhoseTurn;
            }
        }

        public class BuyProperty : Command
        {
            public BuyProperty(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                NextPlayer();
            }

            public override bool IsLegal()
            {
                return true;
            }
        }

        public class PayRent : Command
        {
            public PayRent(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _PayRent();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _PayRent()
            {
            }
        }

        public class EndTurn : Command
        {
            public EndTurn(Player player) => _player = player;


            public override void Execute()
            {
                base.Execute();

                NextPlayer();
            }

            public override bool IsLegal()
            {
                return true;
            }
        }

        public class OpenAuction : Command
        {
            public OpenAuction(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _OpenAuction();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _OpenAuction()
            {
            }
        }

        public class Bid : Command
        {
            public Bid(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _Bid();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _Bid()
            {
            }
        }

        public class PayJailFee : Command
        {
            public PayJailFee(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _PayJailFee();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _PayJailFee()
            {
            }
        }

        public class StayInJail : Command
        {
            public StayInJail(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _StayInJail();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _StayInJail()
            {
            }
        }

        public class BuildHouse : Command
        {
            public BuildHouse(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _BuildHouse();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _BuildHouse()
            {
            }
        }

        public class SellHouse : Command
        {
            public SellHouse(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _SellHouse();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _SellHouse()
            {
            }
        }

        public class MortgageProperty : Command
        {
            public MortgageProperty(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _MortgageProperty();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _MortgageProperty()
            {
            }
        }

        public class UnmortgageProperty : Command
        {
            public UnmortgageProperty(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _UnmortgageProperty();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _UnmortgageProperty()
            {
            }
        }

        public class SetOffer : Command
        {
            public SetOffer(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _SetOffer();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _SetOffer()
            {
            }
        }

        public class AcceptOffer : Command
        {
            public AcceptOffer(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _AcceptOffer();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _AcceptOffer()
            {
            }
        }

        public class RefuseOffer : Command
        {
            public RefuseOffer(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _RefuseOffer();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _RefuseOffer()
            {
            }
        }

        public class DeclareBankruptcy : Command
        {
            public DeclareBankruptcy(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _DeclareBankruptcy();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _DeclareBankruptcy()
            {
            }
        }

        public class Quit : Command //noooooooo
        {
            public Quit(Player player) => _player = player;

            public override void Execute()
            {
                base.Execute();

                _Quit();
            }

            public override bool IsLegal()
            {
                return true;
            }

            void _Quit()
            {
            }
        }

        public void AddMoney(int salary)
        {
            _money += salary;
        }

        private Board.Place currentOccupation;

        public void SetCurrentOccupation(Board.Place place)
        {
            currentOccupation = place;
        }

        public void SpendMoney(int amount) => _money -= amount;

        public bool HasEnoughMoney(int amount) => _money >= amount;

        public void AddProperty(Board.Property prop) => _properties.Add(prop);
    }
}