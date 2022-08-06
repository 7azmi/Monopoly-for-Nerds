namespace Monopoly_for_Nerds;
using static Monopoly;
using static Monopoly.Board;
using static Monopoly.Engine;
using static Monopoly.Dice;
using static Monopoly.Auction;
using static Console;

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
        
        OnBuyProperty?.Invoke(player, property);
    }
    
    public static void BuyHouse(this Player player, Street street)
    {
        street.AddHouse();
        player.SpendMoney(street.GetHousePrice());
        
        WriteLine($"you bought a house on {street.GetName()} for ${street.GetHousePrice()}");
    }
    
    public static void SellHouse(this Player player, Street street)
    {
        street.RemoveHouse();
        player.AddMoney(street.GetHousePrice() /2);//half price
        
        WriteLine($"you sold a house from {street.GetName()} for ${street.GetHousePrice()/2}");
    }

    public static void MortgageProperty(this Player player, Property property)
    {
        property.Mortgage();
        player.AddMoney(property.MortgageValue);
        
        WriteLine($"you mortgaged {property.GetName()} for ${property.MortgageValue}");
    }
    public static void UnmortgageProperty(this Player player, Property property)
    {
        property.Unmortgage();
        player.SpendMoney(property.UnmortgageValue);
        
        WriteLine($"you unmortgaged {property.GetName()} for ${property.UnmortgageValue}");
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

    public static void Bid(this Player bidder, Property onSale, int newBid)
    {
        MostBidder = bidder;
        MostBid = newBid;

        OnBid?.Invoke(bidder, onSale, newBid);
    }

    public static void CloseAuction(this Player winner, Property property)
    {
        winner.SpendMoney(MostBid);
        winner.AddProperty(property);
        property.SetOwner(winner);
        
        WriteLine($"{MostBidder} wins {property.GetName()} for ${MostBid}");
        
        OnCloseAuction?.Invoke(winner, property);
    }
    
    public static void Move(this Player player, Place[] steps)
    {
        foreach (var step in steps)
        {
            player.SetCurrentOccupation(step);

            OnMovingOnPlace?.Invoke(step, WhoseTurn);
            
            Thread.Sleep(150);
        }
        steps.Last().Land();
    }

    public static void DeclareBankruptcy(this Player player)
    {
        //todo later
    }
}
