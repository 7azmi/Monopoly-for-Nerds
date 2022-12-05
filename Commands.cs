using MonopolyTerminal.Enums;
using static MonopolyTerminal.Monopoly.Engine;
using static MonopolyTerminal.Monopoly.Board;
using static System.Console;
namespace MonopolyTerminal;

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
                    Log("It's not your turn you dummy");
                    return false;
                }

                if (_player.InJail)
                {
                    Log("you need to get out of jail before rolling");
                    return false;
                }
                return true;
            }
        }//OnDiceReady

        public class BuyProperty : Command
        {
            private Property _property;
            public BuyProperty(Property property)
            {
                _player = WhoseTurn;
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
                    Log("It's not your turn to buy");
                    return false;
                }

                if (!_player.HasEnoughMoney(_property.GetPrice()))
                {
                    Log("you don't have enough money, try bidding.");
                    return false;
                }
                return true;
            }
        }//OnlandingOnUnownedProperty

        // public static Command Pay(Player victim, PaymentReason reason, int value)
        // {
        //     switch (reason)
        //     {
        //         case PaymentReason.Rental:
        //             return new PayRent(victim, victim._currentOccupation as Property);
        //             break;
        //         case PaymentReason.Bid:
        //             
        //             break;
        //         case PaymentReason.BankFees:
        //             break;
        //     }
        //
        //     return null;
        // } 
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
                // if (!_player.HasEnoughMoney(_property.GetRentalValue()))
                // {
                //     WriteLine("you're too broke to pay rent, sell some of your properties, or 'declare bankruptcy' :)");
                //     return false;
                // }
                return true;
            }
        }//OnRentalDue

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
                    Log("hom many times I have to tell you it's not your turn?!");
                    return false;
                }
                if (Dice.PlayerCanRoll())
                {
                    Log("you need to roll the dice before ending your turn");
                    return false;
                }
                return true;
            }
        }//OnTurn

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
                
                _property.OpenAuction();
            }

            public override bool IsLegal()
            {
                return true;//always
            }
        }//OnBuyOrBid

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
                    if (_newBidding < Auction.MostBid) Log("you can't bid less you dummy");
                    else Log("you need to bid more than current bid");
                    return false;
                }
                return true;
            }
        }//OnAuction

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
                    Log("It's not your turn");
                    return false;
                }
                if (!_player.HasEnoughMoney(_jailFee))//player balance should never gets in minus, you'll find some
                {                                     //issues here otherwise.
                    Log("you can't afford jail fees");
                    return false;
                }
                return true;
            }
        }//OnTurn

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
                    Log("you can't stay in jail for more than twice as you're free to go :)");
                    return false;
                }
                return true;//you can't stay in jail three times, that's why you get out for free the third time
                //no need for that roll for double feature
            }
        }//OnTurn

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
                    Log("you don't own this property");
                    return false;
                }
                if (_street.MaxHouses)
                {
                    Log("you already have build 5 houses on " + _street.GetName());
                    return false;
                }

                if (!_player.HasEnoughMoney(_street.GetHousePrice()))
                {
                    Log("you don't have enough money to buy a house here");
                    return false;
                }

                if (_street.IsMortgaged())
                {
                    Log("you need to unmortgage before building");
                    return false;
                }

                
                if (false)//todo if there is houses on...
                {
                    
                }
                return true;
            }
        }//OnTurn

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
                    Log("you don't own this property");
                    return false;
                }
                if (_street.NoHouses)
                {
                    Log("there is no house to sell here");
                    return false;
                }
                //todo selling order
                
                return true;
            }
        }//OnTurn
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
                    Log("you don't own this property");
                    return false;
                }
                if (_property is Street street && street.HasHouses)
                {
                    Log("you can't mortgage a street with houses, sell them first");
                    return false;
                }
                if (_property.IsMortgaged())
                {
                    Log($"{_property.GetName()} is already mortgaged, so sad");
                    return false;
                }
                return true;
            }
        }//OnTurn

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
                    Log("you don't own this property");
                    return false;
                }
                if (!_property.IsMortgaged())
                {
                    Log($"{_property.GetName()} is not mortgaged");
                    return false;
                }
                if (!_player.HasEnoughMoney(_property.MortgageValue))
                {
                    Log($"you don't have enough money to unmortgage this {_property.GetName()}");
                    return false;
                }
                return true;
            }
        }//OnTurn

        public class SetOffer : Command
        {
            private Offer _offer;
            private Player Target => _player;
            public SetOffer(Player target, Offer offer)
            {
                //if target is not given then check if there is only one opponent to set
                //If not, return first offered property's owner if given, set null otherwise.
                _player = target is null ? ActivePlayers.Count ==2 ? GetOpponent(WhoseTurn) : offer.PropertiesToOffer is null ? null: offer.PropertiesToOffer.First().GetOwner() : target;
                
                _offer = offer;
                
                Player GetOpponent(Player player) => ActivePlayers.FirstOrDefault(p => p != player);

            }

            public override void Execute()
            {
                if (!IsLegal()) return;
                
                WhoseTurn.SetOffer(_player, _offer);
            }

            public override bool IsLegal()
            {
                Log("do you hear me???");
                ///can be legal only if
                ///there are both-parts assets (money or properties at least)
                ///target is chosen
                ///chosen properties are owned by their actual owners
                ///both players have enough money if included
                ///both sides don't present streets with houses 

                if (!IsCorrectDealStructure()) 
                    return false;
                
                if (_player is null)
                {
                    Log("the computer still cannot identify your opponent from the given offer");
                    Log("you need to include your opponent's special letter");
                    return false;
                }
                
                if (!AreSelectedPropertiesOwnedByTheirOwners())
                    return false;

                if (!BothSidesHaveEnoughMoney())
                    return false;
                if (!BothDoNotPresentStreetWithHouses())
                    return false;
                
                Log("yes you can");

                return true;

                bool IsCorrectDealStructure()
                {
                    var moneyIsOffered = _offer.MoneyToOffer > 0;
                    var moneyIsRequested = _offer.MoneyToRequest > 0;
                    var aPropertyIsOffered = !(_offer.PropertiesToOffer == null || _offer.PropertiesToOffer.Length ==0);//todo null or empty helper
                    var aPropertyIsRequested = !(_offer.PropertiesToRequest == null || _offer.PropertiesToRequest.Length ==0);
                    var offerorOffersSomething = moneyIsOffered || aPropertyIsOffered;
                    var offerorRequestsSomething = moneyIsRequested || aPropertyIsRequested;
                    var bothSidesPresentMoney = moneyIsOffered && moneyIsRequested;
                    var bothSidesPresentProperties = aPropertyIsOffered && aPropertyIsRequested;
                    var bothSidesPresentMoneyOnly = bothSidesPresentMoney && !bothSidesPresentProperties;
                    var bothSidesPresentPropertiesOnly = !bothSidesPresentMoney && bothSidesPresentProperties;
                    var bothSidesPresentSomethingToTrade = offerorOffersSomething && offerorRequestsSomething;

                    if (bothSidesPresentSomethingToTrade && !bothSidesPresentMoneyOnly) return true;
                    
                    else if (bothSidesPresentMoneyOnly)
                    {
                        if(_offer.MoneyToOffer < _offer.MoneyToRequest) Log("https://bit.ly/3K6D5Pj");
                        else if (_offer.MoneyToOffer > _offer.MoneyToRequest) return true;//trolling
                    }else if(offerorRequestsSomething && !offerorOffersSomething) Log("you can't demand something for nothing!");
                    else if (!offerorRequestsSomething && offerorOffersSomething)
                    {
                        Log("!you can't demand nothing for something");
                        //return true;//trolling
                    }

                    Log("offer structure error, try again");
                    return false;
                }

                bool AreSelectedPropertiesOwnedByTheirOwners()
                {
                    var allRequestedPropertiesHaveTheSameOwner =
                        _offer.PropertiesToRequest.All(property => property.GetOwner() == Target);
                    var allOfferedPropertiesOwnedByOfferor =
                        _offer.PropertiesToOffer.All(property => property.GetOwner() == WhoseTurn);

                    if (allOfferedPropertiesOwnedByOfferor && allRequestedPropertiesHaveTheSameOwner) return true;
                    
                    Log("Ownership error");//todo write possible errors
                    return false;
                    /*
                 * 
                if(_offer.PropertiesToOffer != null)
                    foreach (var property in _offer.PropertiesToOffer)
                    {
                        if (property.GetOwner() == null)
                        {
                            WriteLine($"{property.GetName()} is not acquired yet, you think you can get it from the bank?");
                            return false;
                        }
                        else if(property.GetOwner() != _offer.Target)
                    }

                return false;
                                     */
                }

                bool BothSidesHaveEnoughMoney()
                {
                    var offerorHasEnoughMoney = WhoseTurn.HasEnoughMoney(_offer.MoneyToOffer);
                    var offereeHasEnoughMoney = Target.HasEnoughMoney(_offer.MoneyToRequest);

                    if (offereeHasEnoughMoney && offerorHasEnoughMoney) return true;
                    
                    if(!offereeHasEnoughMoney) Log("your opponent can't accept a deal where you demand more money than he actually has");
                    if(offerorHasEnoughMoney) Log("you can't offer money you don't have :)");
                    
                    Log("hello2");
                    return false;
                }

                bool BothDoNotPresentStreetWithHouses()
                {
                    var offerorOffersStreetWithHouses =
                        _offer.PropertiesToOffer.Any(property => property is Street s && s.HasHouses);
                    
                    var offerorRequestsStreetWithHouses =
                        _offer.PropertiesToRequest.Any(property => property is Street s && s.HasHouses);

                    if (!offerorOffersStreetWithHouses && !offerorRequestsStreetWithHouses) return true;
                    if(offerorOffersStreetWithHouses) Log("you can't offer nor request a street invested with houses. are you dumb?");
                    if(offerorRequestsStreetWithHouses) Log("you can't offer nor request a street invested with houses");

                    Log("hello3");

                    return false;
                }
            }
            public struct Offer
            {
                public Offer(Property[] propertiesToOffer, Property[] propertiesToRequest, int moneyToOffer = 0, int moneyToRequest = 0)
                {
                    PropertiesToOffer = propertiesToOffer;
                    MoneyToOffer = moneyToOffer;
                    PropertiesToRequest = propertiesToRequest;
                    MoneyToRequest = moneyToRequest;
                }
                internal Property[] PropertiesToOffer;
                internal int MoneyToOffer;
                internal Property[] PropertiesToRequest;
                internal int MoneyToRequest;

                public string offerInfo()
                {
                    string info = "";
                    foreach (var p in PropertiesToOffer) info += $"- {p.GetName()}\n";
                    info += $"- ${MoneyToOffer}\n";

                    info += "for\n";
                    
                    foreach (var p in PropertiesToRequest) info += $"- {p.GetName()}\n";
                    info += $"- ${MoneyToRequest}\n";
                    return info;
                }
            }
            
            

        }//OnTurn

        public class AcceptOffer : Command
        {
            private Player _offeror;
            private SetOffer.Offer _offer;
            public AcceptOffer(Player offeree , Player offeror, Player.SetOffer.Offer offer)
            {
                _player = offeree;
                _offeror = offeror;
                _offer = offer;
            }

            public override void Execute()
            {
                if (!IsLegal()) return;
                
                _player.AcceptOffer(_offeror, _offer);
            }

            public override bool IsLegal()
            {
                return true;//should be able to accept
            }
        }//OnReceiveDeal

        public class DeclineOffer : Command
        {
            private Player _offeror;
            public DeclineOffer(Player offeree, Player offeror)    
            {
                _player = offeree;
                _offeror = offeror;
            } 

            public override void Execute()
            {
                if (!IsLegal()) return;

                _player.DeclineOffer(_offeror);
            }

            public override bool IsLegal()
            {
                return true;
            }
            
        }//OnReceiveDeal

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
            
        }//OnTurn

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

        static void Log(string line)
        {
            Human.Terminal.Log(line);
        }
    }
}