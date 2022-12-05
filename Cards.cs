namespace MonopolyTerminal;

using static Monopoly.Board;
using static Monopoly.Engine;

public class Cards
{
    static ConsoleBlock cardBlock = new (new Coord(90, 23));
    
    public static string[] GetStreetCardInfo(Street s)
    {
        var cardName = s.GetName().PadRight(15);
        var p = s.GetPrice().ToString().PadRight(4);
        var r0 = s.Rentals[0].ToString().PadRight(4);
        var r1 = s.Rentals[1].ToString().PadRight(4);
        var r2 = s.Rentals[2].ToString().PadRight(4);
        var r3 = s.Rentals[3].ToString().PadRight(4);
        var r4 = s.Rentals[4].ToString().PadRight(4);
        var r5 = s.Rentals[5].ToString().PadRight(4);

        var hp = s.GetHousePrice().ToString().PadRight(4);

        return new[]
        {
            new string($"|------------------------|"),
            new string($"|  {cardName      } ${p} |"),
            new string($"|                  ${r0} |"),
            new string($"|  ■               ${r1} |"),
            new string($"|  ■■              ${r2} |"),
            new string($"|  ■■■             ${r3} |"),
            new string($"|  ■■■■            ${r4} |"),
            new string($"|  ■■■■■           ${r5} |"),
            new string($"|  ■ :             ${hp} |"),
            new string($"|------------------------|")
        };

        //Print("═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬─│┌┐└┘├┬┴┼");
    }

    public static string[] GetRailroadCardInfo(Railroad r)
    {
        var cardName = r.GetName().PadRight(15);
        var p = r.GetPrice().ToString().PadRight(4);

        return new[]
        {
            new string($"|------------------------|"),
            new string($"|  {cardName      } ${p} |"),
            new string($"|  Rental                |"),
            new string($"|  One railroad    $25   |"),
            new string($"|  Two railroads   $50   |"),
            new string($"|  Three railroads $100  |"),
            new string($"|  Four railroads  $200  |"),
            new string($"|                        |"),
            new string($"|                        |"),
            new string($"|------------------------|")
        };

    }

    public static string[] GetCompanyCardInfo(Company c)
    {
        var cardName = c.GetName().PadRight(15);
        var p = c.GetPrice().ToString().PadRight(4);

        return new[]
        {
            new string($"|------------------------|"),
            new string($"|  {cardName      } ${p} |"),
            new string($"|  Rental                |"),
            new string($"|  Sum dice multiplied   |"),
            new string($"|  by four for owning    |"),
            new string($"|  a utility, by ten     |"),
            new string($"|  for both utilities.   |"),
            new string($"|                        |"),
            new string($"|                        |"),
            new string($"|------------------------|")
        };
    }
    
    private static void Print(string line) => Console.WriteLine(line);

    private static void PrintCard(Place place)
    {
        if(place is Street s) GetStreetCardInfo(s);
    }
    public static void Init()
    {
        OnLanding += place => cardBlock.Update(GetCardInfo(place));
        
        
        OnLandingCompleted += (place) =>cardBlock.Clear();
        OnBuyProperty += (_) => cardBlock.Clear();

        string[] GetCardInfo(Place place)
        {
            if (place is Street s) return GetStreetCardInfo(s);
            if (place is Railroad r) return GetRailroadCardInfo(r);
            if (place is Company c) return GetCompanyCardInfo(c);
            return new []{":)"};
        }
    }
}