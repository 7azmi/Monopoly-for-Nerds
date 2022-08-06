namespace Monopoly_for_Nerds;
using static Monopoly.Engine;
using static Monopoly.Board;
using static Console;
public partial class Monopoly
{
    
    public partial class Player
    {
        public abstract class Command
        {
            protected Player _player;

            public abstract void Execute();

            public abstract bool IsLegal();
        }

        public class RollDice : Command
        {
            public RollDice(Player player) => _player = player;

            public override void Execute()
            {
                if (!IsLegal()) return;

                Dice.Roll();
            }

            public override bool IsLegal()
            {
                if (!_player.MyTurn)
                {
                    WriteLine("It's not your turn you dummy");
                    return false;
                }

                if (_player.InJail)
                {
                    WriteLine("you need to get out of jail before rolling");
                    return false;
                }
                return true;
            }
        }

        public class BuyProperty : Command
        {
            private Property _property;
            public BuyProperty(Player player, Property property)
            {
                _player = player;
                _property = property;
            } 

            public override void Execute()
            {
                if (!IsLegal()) return;
                
                _player.BuyProperty(_property);
            }

            public override bool IsLegal()
            {
                if (!_player.MyTurn)
                {
                    WriteLine("It's not your turn to buy");
                    return false;
                }

                if (!_player.HasEnoughMoney(_property.GetPrice()))
                {
                    WriteLine("you don't have enough money, try bidding.");
                    return false;
                }
                return true;
            }
        }

        public class PayRent : Command
        {
            private Property _property;

            public PayRent(Player player, Property property)
            {
                _player = player;
                _property = property;
            } 
            
            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.PayRent(_property);
            }

            public override bool IsLegal()
            {
                if (!_player.HasEnoughMoney(_property.GetRentalValue()))
                {
                    WriteLine("you're too broke to pay rent, sell some of your properties, or 'declare bankruptcy' :)");
                    return false;
                }
                return true;
            }
        }

        public class EndTurn : Command
        {
            public EndTurn(Player player) => _player = player;
            
            public override void Execute()
            {
                if (!IsLegal()) return;
                
                SwitchNextPlayerTurn();
            }

            public override bool IsLegal()
            {
                if (!_player.MyTurn)
                {
                    WriteLine("hom many times I have to tell you it's not your turn?!");
                    return false;
                }
                if (Dice.PlayerCanRoll())
                {
                    WriteLine("you need to roll the dice before ending your turn");
                    return false;
                }
                return true;
            }
        }

        public class OpenAuction : Command
        {
            private Property _property;
            public OpenAuction(Property onSale)
            {
                _property = onSale;
            } 

            public override void Execute()
            {
                if (!IsLegal()) return;
                
                OnAuction?.Invoke(_property);
            }

            public override bool IsLegal()
            {
                return true;//always
            }
        }

        public class Bid : Command
        {
            private int _newBidding;

            public Bid(Player newBidder, int newBid)
            {
                _player = newBidder;
                _newBidding = newBid;
            } 

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.Bid(Auction.OnSale, _newBidding);
            }

            public override bool IsLegal()
            {
                if (_newBidding <= Auction.MostBid)
                {
                    if (_newBidding < Auction.MostBid) WriteLine("you can't bid less you dummy");
                    else WriteLine("you need to bid more");
                    return false;
                }
                return true;
            }
        }

        public class GetOutOfJail : Command
        {
            private int _jailFee;

            public GetOutOfJail(Player player)
            {
                _player = player;
                _jailFee = Jail.GetPrisoner(_player).ExceededJailPeriod() ? 0 : 50;
            }

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.GetOutOfJail(_jailFee);
            }

            public override bool IsLegal()
            {
                if (!_player.MyTurn)
                {
                    WriteLine("It's not your turn");
                    return false;
                }
                if (!_player.HasEnoughMoney(_jailFee))//player balance should never gets in minus, you'll find some
                {                                     //issues here otherwise.
                    WriteLine("you can't afford jail fees");
                    return false;
                }
                return true;
            }
        }

        public class StayInJail : Command
        {
            public StayInJail(Player player) => _player = player;

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.StayInJail();
            }

            public override bool IsLegal()
            {
                if (Jail.GetPrisoner(_player).ExceededJailPeriod())//twice
                {
                    WriteLine("you can't stay in jail for more than twice as you're free to go :)");
                    return false;
                }
                return true;//you can't stay in jail three times, that's why you get out for free the third time
                //no need for that roll for double feature
            }
        }

        public class BuildHouse : Command
        {
            private Street _street;
            
            public BuildHouse(Player player, Street street)
            {
                _player = player;
                _street = street;
            } 

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.BuyHouse(_street);
            }

            public override bool IsLegal()
            {
                if (!_player._properties.Contains(_street))
                {
                    WriteLine("you don't own this property");
                    return false;
                }
                if (_street.MaxHouses)
                {
                    WriteLine("you already have build 5 houses on " + _street.GetName());
                    return false;
                }

                if (!_player.HasEnoughMoney(_street.GetHousePrice()))
                {
                    WriteLine("you don't have enough money to buy a house here");
                    return false;
                }

                if (_street.IsMortgaged())
                {
                    WriteLine("you need to unmortgage before building");
                    return false;
                }

                if (false)//todo if there is houses on...
                {
                    
                }
                return true;
            }
        }

        public class SellHouse : Command
        {
            private Street _street;
            public SellHouse(Player player, Street street)
            {
                _player = player;
                _street = street;
            } 

            public override void Execute()
            {
                if (!IsLegal()) return;
                
                _player.SellHouse(_street);
            }

            public override bool IsLegal()
            {
                if (!_player._properties.Contains(_street))
                {
                    WriteLine("you don't own this property");
                    return false;
                }
                if (_street.NoHouses)
                {
                    WriteLine("there is no house to sell here");
                    return false;
                }
                //todo selling order
                
                return true;
            }
        }
        public class MortgageProperty : Command
        {
            private Property _property;
            public MortgageProperty(Player player, Property property)
            {
                _player = player;
                _property = property;
            }

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.MortgageProperty(_property);
            }
            
            public override bool IsLegal()
            {
                if (!_player._properties.Contains(_property))
                {
                    WriteLine("you don't own this property");
                    return false;
                }
                if (_property.IsMortgaged())
                {
                    WriteLine($"{_property.GetName()} is already mortgaged, so sad");
                    return false;
                }
                return true;
            }
        }

        public class UnmortgageProperty : Command
        {
            private Property _property;
            public UnmortgageProperty(Player player, Property property)
            {
                _player = player;
                _property = property;
            }

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.UnmortgageProperty(_property);
            }

            public override bool IsLegal()
            {
                if (!_player._properties.Contains(_property))
                {
                    WriteLine("you don't own this property");
                    return false;
                }
                if (!_property.IsMortgaged())
                {
                    WriteLine($"{_property.GetName()} is not mortgaged");
                    return false;
                }
                if (!_player.HasEnoughMoney(_property.MortgageValue))
                {
                    WriteLine($"you don't have enough money to unmortgage this {_property.GetName()}");
                    return false;
                }
                return true;
            }
        }

        public class SetOffer : Command
        {
            public SetOffer(Player player) => _player = player;

            public override void Execute()
            {
                if (!IsLegal()) return;
                
                
            }

            public override bool IsLegal()
            {
                return true;
            }
        }

        public class AcceptOffer : Command
        {
            public AcceptOffer(Player player) => _player = player;

            public override void Execute()
            {
                if (!IsLegal()) return;


            }

            public override bool IsLegal()
            {
                return true;
            }
        }

        public class RefuseOffer : Command
        {
            public RefuseOffer(Player player) => _player = player;

            public override void Execute()
            {
                if (!IsLegal()) return;

                
            }

            public override bool IsLegal()
            {
                return true;
            }
            
        }

        public class DeclareBankruptcy : Command
        {
            public DeclareBankruptcy(Player player) => _player = player;

            public override void Execute()
            {
                if(!IsLegal()) return;

                _player.DeclareBankruptcy();
            }

            public override bool IsLegal()
            {
                return true;
            }
            
        }

        public class Quit : Command //noooooooo
        {
            public Quit(Player player) => _player = player;

            public override void Execute()
            {
                if (!IsLegal()) return;

                Environment.Exit(0);//bye
            }

            public override bool IsLegal()
            {
                return true;
            }
        }
    }
}