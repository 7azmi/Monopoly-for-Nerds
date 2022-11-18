using MonopolyTerminal.Enums;
using MonopolyTerminal.Helpers;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Engine;
using static MonopolyTerminal.Monopoly.Dice;
using static MonopolyTerminal.Monopoly.Auction;
using static System.Console;

namespace MonopolyTerminal;

public static class Bank//to be modified 
{
    public static void GetSalary(this Player p, int salary)
    {
        p.AddMoney(salary);
        
        WriteLine($"{p.GetName()} receives $200 salary");
    }

    public static void GoJail(this Player player)
    {
        var jail = GetPlace(10) as Jail;
        
        jail.GetemIn(player);
        player.SetCurrentOccupation(jail);
        
        WriteLine($"{player.GetName()} went to jail");
        
        OnPlayerGetsToJail?.Invoke(player);//don't ask me why 2

        SwitchNextPlayerTurn();
    }

    public static void Pay(Player victim, PaymentReason reason, int value, Player collector)
    {
        
    }
    
    public static void GetOutOfJail(this Player player, int fine)
    {
        var jail = GetPlace(10) as Jail;

        player.SpendMoney(fine);
        jail.GetemOut(player);

        WriteLine($"{player.GetName()} went out of jail");

        OnPlayerGetsOutOfJail?.Invoke(player);
        
        SetDiceState(DiceState.ReadyForRolling);
    }

    public static void StayInJail(this Player player)
    {
        Jail.StayInJail(player);

        var times = Jail.GetPrisoner(player).GetHowManyRoundsPlayerIsInJail();
        var additionalInfo = times > 1 ? $" for {times} times" : "";
        
        WriteLine($"you decided to stay in jail{additionalInfo}, you can manage your properties though.");
        
        OnPlayerDecidesToStayInJail?.Invoke(player);
    }
    
    public static void BuyProperty(this Player player, Property property)
    {
        player.SpendMoney(property.GetPrice());
        property.SetOwner(player);
        player.AddProperty(property);
        
        WriteLine($"{player.GetName()} bought {property.GetName()} for ${property.GetPrice()}.");
        
        OnBuyProperty?.Invoke(property);
    }
    
    public static void BuyHouse(this Player player, Street street)
    {
        street.AddHouse();
        player.SpendMoney(street.GetHousePrice());
        
        WriteLine($"you bought a house on {street.GetName()} for ${street.GetHousePrice()}");
        
        OnPlayerBuyHouse?.Invoke(player, street);
    }
    
    public static void SellHouse(this Player player, Street street)
    {
        street.RemoveHouse();
        player.AddMoney(street.GetHousePrice() /2);//half price
        
        WriteLine($"you sold a house from {street.GetName()} for ${street.GetHousePrice()/2}");
        
        OnPlayerSellHouse?.Invoke(player, street);
    }

    public static void MortgageProperty(this Player player, Property property)
    {
        property.Mortgage();
        player.AddMoney(property.MortgageValue);
        
        WriteLine($"you mortgaged {property.GetName()} for ${property.MortgageValue}");
        
        OnPlayerMortgage?.Invoke(player, property);
    }
    public static void UnmortgageProperty(this Player player, Property property)
    {
        property.Unmortgage();
        player.SpendMoney(property.UnmortgageValue);
        
        WriteLine($"you unmortgaged {property.GetName()} for ${property.UnmortgageValue}");
        
        OnPlayerUnmortgage?.Invoke(player, property);
    }

    public static void PayRent(this Player victim, Property property)
    {
        var owner = property.GetOwner();
        var rental = property.GetRentalValue();
        
        victim.SpendMoney(rental);
        owner.AddMoney(rental);
        
        WriteLine($"{victim.GetName()} paid {owner.GetName()} ${rental}");

        OnRentalPaid?.Invoke(property, victim, rental);
    }

    public static void OpenAuction(this Property onSale)
    {
        Auction.OpenAuction(onSale);
        
        WriteLine($"{onSale.GetName()} is on sale. Let's see who bids most!");

        OnAuction?.Invoke(OnSale);
    }
    
    public static void Bid(this Player bidder, Property onSale, int newBid)
    {
        MostBidder = bidder;
        MostBid = newBid;

        OnBid?.Invoke(bidder, onSale, newBid);
    }

    public static void CloseAuction(this Player winner, Property property)
    {
        if (winner != null)
        {
            winner.SpendMoney(MostBid);
            winner.AddProperty(property);
            property.SetOwner(winner);
        
            WriteLine($"{MostBidder.GetName()} wins {property.GetName()} for ${MostBid}");
        } 
        else CancelAuction();

        OnCloseAuction?.Invoke(winner, property);

        void CancelAuction()
        {
            WriteLine("No one bids! strange.");
        }
    }
    
    public static void Move(this Player player, Place[] steps)
    {
        foreach (var step in steps)
        {
            player.SetCurrentOccupation(step);

            OnMovingOnPlace?.Invoke(step, WhoseTurn);
            
            Thread.Sleep(150);
        }

        if (steps.IsNullOrEmpty())
        {
            Console.WriteLine("Step path is null or empty! Reload the program.");
            return;
        }
        steps.Last().Land();
    }

    public static void DeclareBankruptcy(this Player player)
    {
        //todo later
    }

    public static void SetOffer(this Player offeror, Player offeree, Player.SetOffer.Offer offer)
    {
        OnOffering?.Invoke(offeror, offeree, offer);
    }

    public static void AcceptOffer(this Player offeree, Player offeror, Player.SetOffer.Offer offer)
    {
        SwitchProperties();
        
        OnAcceptOffer?.Invoke(offeree, offeror, offer);

        void SwitchProperties()
        {
            offeror.SpendMoney(offer.MoneyToOffer);
            offeree.AddMoney(offer.MoneyToOffer); 
            
            offeree.SpendMoney(offer.MoneyToRequest);
            offeror.AddMoney(offer.MoneyToRequest);

            foreach (var property in offer.PropertiesToOffer) property.SetOwner(offeree);
            foreach (var property in offer.PropertiesToRequest) property.SetOwner(offeror);
        }
    }
    
    public static void DeclineOffer(this Player offeree, Player offeror)
    {
        OnDeclineOffer?.Invoke(offeree, offeror);
    }


    public static int TotalNetWorth(Property[] properties, int money = 0, bool withCard = false)
    {
        var sum = (float) money;
        sum += withCard ? 50 : 0;

        var interests = Monopoly.GameSettings.InterestsRate;
        foreach (var property in properties)
        {
            if (property is Street s && s.HasHouses) sum += s.GetHousePrice() / 2 * s.HouseCount;
            sum += property.IsMortgaged() ? property.GetPrice() / 2 * interests : property.GetPrice();
        }

        return (int)sum;
    }
    public static int TotalNetWorth(Player player) => 
        TotalNetWorth(player.Properties.ToArray(), player.GetMoney(), player.HasJailFreeCard);
    
}