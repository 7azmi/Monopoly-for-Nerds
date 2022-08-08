using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Monopoly_for_Nerds;
using static Monopoly_for_Nerds.Monopoly;
using static Monopoly_for_Nerds.Monopoly.Engine;
using static Monopoly_for_Nerds.Monopoly.Board;
using static System.Console;
using static Monopoly_for_Nerds.Printer;
using static Monopoly_for_Nerds.TypingSimulator;
using static Monopoly_for_Nerds.Monopoly.Player;
using static Monopoly_for_Nerds.Monopoly.Auction;

var players = new[]
{
    new Player("nGAGEOnline", ConsoleColor.Red, 1500, false),
    new Player("Cear", ConsoleColor.Yellow, 1500, true),
    new Player("Ahmed", ConsoleColor.Cyan, 1500, true),
    //new Monopoly.Player("Ahmed", ConsoleColor.Green, 1500, true)
};

PrintBoard();

ConnectGameLogicWithCommands();//terminal side

var m = new Monopoly(players);//game side

Log("Finished");

Read();


//////////////////////////////////////////////////////////////////////////////////////////

void ConnectGameLogicWithCommands()
{
    OnPlayerTurnWhileInJail += AskPlayerToGetOutOrStay;
    OnPlayerDecidesToStayInJail += player => AskPlayerToProceedPlaying();
    OnDiceReadyForRolling += AskPlayerToRoll;
    OnRentalDue += AskPlayerToPayRentOrDivest;
    OnBuyProperty += (player, property) => AskPlayerToProceedPlaying();
    OnLandingOnMyProperty += (player, property) => AskPlayerToProceedPlaying(); 
    OnRentalPaid += (property, player, rentalValue) => AskPlayerToProceedPlaying();
    OnCloseAuction += (player, property) => AskPlayerToProceedPlaying();
    OnLandingCompleted +=(player, property) => AskPlayerToProceedPlaying();//todo refactor later
    OnLandingOnUnownedProperty += AskPlayerToBuyPropertyOrPutItOnAuction;
    OnAuction += AskPlayersToBid;

    void AskPlayerToGetOutOrStay(Player prisoner)
    {
        Log("you are in jail, would you like to get 'out' for $50, or 'stay' this time?");

        string line;

        tryAgain:
        
        line = !prisoner.IsBot ? ReadLine() : BotRead();

        if (line.Contains("out"))
        {
            var getOut = new Player.GetOutOfJail(prisoner);
            if(getOut.IsLegal()) getOut.Execute(); 
            else goto tryAgain;
        }
        else if (line.Contains("stay"))
        {
            var getOut = new Player.StayInJail(prisoner);
            if(getOut.IsLegal()) getOut.Execute(); 
            else goto tryAgain;
        } else goto tryAgain;
        
        string BotRead()
        {
            Random random = new Random();
            bool randomBool = random.Next(2) == 1;
            var command = "stay";

            if (prisoner.HasEnoughMoney(50) || Jail.GetPrisoner(prisoner).ExceededJailPeriod()) command = "out";

            return TypeOutText(command);
        }
    }
    void AskPlayerToRoll(int times, Player whoseTurn)
    {
        Log("'roll' the dice");

        string line;
        do
            line = !whoseTurn.IsBot ? ReadLine() : BotRead("roll");
        while (line != "roll");

        new Player.RollDice(whoseTurn).Execute();

        string BotRead(string command)
        {
            //do some typing here
            
            return TypeOutText(command);
        }
        
    }
    void AskPlayerToPayRentOrDivest(Property property, Player victim, int amount)
    {
        if (victim.HasEnoughMoney(amount))
            AskPlayerToPayRent(property, victim, amount);
        else AskPlayerToDivestBecauseHeDoesNotHaveEnoughMoney(property, victim, amount);
    }
    void AskPlayerToPayRent(Property property, Player victim, int amount)
    {
        var declareBankruptcyLog = amount >= 900 ? ", you can 'declare bankruptcy' though :)" : "";
        Log($"you have to 'pay' {property.GetOwner().GetName()} ${amount}" + declareBankruptcyLog);

        string line;
        do
        {
            line = !victim.IsBot ? ReadLine() : BotRead("pay");
        }
        while (!line.Contains("pay"));//todo divestment stuff

        new Player.PayRent(victim, property).Execute();

        string BotRead(string command)
        {
            return TypeOutText(command);
        }
    }
    void AskPlayerToDivestBecauseHeDoesNotHaveEnoughMoney(Property property, Player victim, int amount)
    {
        Log($"you owe {property.GetOwner().GetName()} ${amount} while you only have ${victim.GetMoney()}");
        Log("'mortgage ##' or 'sell ##' To proceed, or you can simply 'declare bankruptcy' :)");

        Read("divest");

        void Read(string command)
        {
            string line;
            do
            {
                tryAgain:
                line = !victim.IsBot ? ReadLine() : BotRead("divest");
                var newLine = line.Split(' ');

                switch (newLine[0])//todo refactor this
                {
                    case "mortgage":
                        var p = TryToGetProperty(newLine[1]);
                        if (p is null) goto tryAgain;
                        
                        var mortgage = new Player.MortgageProperty(victim, p);
                        if (mortgage.IsLegal()) mortgage.Execute();
                        break;
                    
                    case "sell":
                        var s = TryToGetStreet(newLine[1]);
                        if (s is null) goto tryAgain;
                        
                        var sellHouse = new Player.SellHouse(victim, s);
                        if (sellHouse.IsLegal()) sellHouse.Execute();
                        break;

                        default:
                            goto tryAgain;
                }
            }
            while (!victim.HasEnoughMoney(amount));

            new Player.PayRent(victim, property).Execute();

            Property TryToGetProperty(string line)
            {
                if (int.TryParse(line, out var index))
                {
                    if (GetPlace(index) is Property p)
                    {
                        return p;
                    }
                    else return null;
                }
                else return null;
            }
            Street TryToGetStreet(string line)
            {
                if (int.TryParse(line, out var index))
                {
                    if (GetPlace(index) is Street s)
                    {
                        return s;
                    }
                    else return null;
                }
                else return null;
            }
        }

        string BotRead(string command)
        {
            //do some typing here
            return command;
        }
    }
    void AskPlayerToBuyPropertyOrPutItOnAuction(Player player, Property property)
    {
        Log($"you landed on unowned property. you have to either 'buy' {property.GetName()} for {property.GetPrice()} or 'bid' for auction");
        
        string line;

        tryAgain:
        line = !player.IsBot ? ReadLine() : BotRead("buy");//todo buy or bid

        if (line.Contains("buy"))
        {
            var buyProperty = new Player.BuyProperty(player, property);
            if (!buyProperty.IsLegal()) goto tryAgain;
            buyProperty.Execute();
        } 
        else if (line.Contains("bid"))
        {
            new Player.OpenAuction(property).Execute();
        } 
        else goto tryAgain;

        string BotRead(string command)
        {
            //do some typing here
            //TypeOutText(command);
            return TypeOutText(command);
        }
    }
    void AskPlayersToBid(Property property)
    {
        Log($"bid '##' for {property.GetName()}, or 'fold'");

        List<Player> bidders  = new List<Player>(ActivePlayers);

        bidders.Reverse();
        
        string line;
        while(true)
        {
            if (bidders.Count == 1) break;//bad code, but works
            
            for (int i = bidders.Count - 1; i >= 0; i--)
            {
                if (bidders.Count == 1 && MostBidder != null) break;
                tryAgain:
                
                Write($"{bidders[i].GetName()}: ");
                line = !bidders[i].IsBot ? ReadLine() : BotRead(bidders[i]);

                if (line.Contains("fold"))
                    bidders.RemoveAt(i);
                else if (!TryExecuteBidCommand(bidders[i], line)) goto tryAgain;
            }
        }

        bidders.First().CloseAuction(property); 
        
        string BotRead(Player bot)
        {
            var newBid = MostBid + new Random().Next(5, 75);
            //do some typing here
            if (MostBid > property.GetPrice() || !bot.HasEnoughMoney(newBid))
            {
                return TypeOutText("fold");
            }
            return TypeOutText(newBid.ToString());
        }
        void Test()
        {
            const int stdInputHandle = -10;

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);
            
            // Start the timeout
            var read = false;
            Task.Delay(15000).ContinueWith(_ =>
            {
                if (!read)
                {
                    // Timeout => cancel the console read
                    var handle = GetStdHandle(stdInputHandle);
                    CancelIoEx(handle, IntPtr.Zero);
                }
            });

            try
            {
                // Start reading from the console
                WriteLine("Do you want to continue [Y/n] (10 seconds remaining):");
                var key = ReadKey();
                read = true;
                WriteLine("Key read");
            }
            // Handle the exception when the operation is canceled
            catch (InvalidOperationException)
            {
                WriteLine("Operation canceled");
            }
            catch (OperationCanceledException)
            {
                WriteLine("Operation canceled");
            }
        }
        
    }
    void AskPlayerToProceedPlaying()//todo refactor this shit
    {
        var player = WhoseTurn;
        
        //roll again or end turn, you can also manage your properties
        if (!player.InJail && Dice.PlayerCanRollAgain())//bad logic but seems to work for now
        {
            string line;
            Log("you got double, 'roll' again");
            tryAgain1:
            line = !player.IsBot ? ReadLine() : BotRead("roll");

            if (line.Contains("roll"))
                new Player.RollDice(player).Execute();
            else goto tryAgain1;
        }
        else
        {
            string line;
            Log("You may manage your properties, set a deal, or 'end' turn");
            tryAgain2:
            line = !player.IsBot ? ReadLine() : BotRead("end");

            if (line.Contains("end"))
                new Player.EndTurn(player).Execute();
            else
            {
                TryExecuteManagePropertyCommand(player, line);
                goto tryAgain2;
            }
        }
        
        string BotRead(string command)
        {
            return TypeOutText(command);
        }
    }
}
bool TryExecuteBidCommand(Player player, string line)
{
    var isLegalCommand = TryGetBidCommand(player, line, out var command);
    if (isLegalCommand) command.Execute();

    return isLegalCommand;

    bool TryGetBidCommand(Player bidder, string line, out Command command)
    {
        command = null;
        
        if (!TryToGetDigits(line, out var bid)) return false;

        command = new Bid(bidder, bid);

        return command != null ? command.IsLegal() : false;
    }
}
bool TryExecuteManagePropertyCommand(Player player, string line)
{
    var isLegalCommand = TryGetPropertyManagementCommand(player, line, out var command);
    if(isLegalCommand) command.Execute();
    
    return isLegalCommand;
    
    bool TryGetPropertyManagementCommand(Player player, string line, out Command command)
    {
        command = null;

        string[] commands = {"buy house", "sell house", "unmort", "mort"};//followed by digits

        if (!TryToGetDigits(line, out var i) || !InBounds(i)) return false;

        var isStreet = TryToGetStreet(i, out var street);

        if (line.Contains(commands[0]) && isStreet) command = new BuildHouse(player, street);
        else if (line.Contains(commands[1]) && isStreet) command = new SellHouse(player, street);
        
        var isProperty = TryToGetProperty(i, out var property);
        
        if (line.Contains(commands[2]) && isProperty) command = new UnmortgageProperty(player, property);
        else if (line.Contains(commands[3]) && isProperty) command = new MortgageProperty(player, property);
            
        
        return command != null ? command.IsLegal() : false;
        
        //lovely checkers
        bool TryToGetProperty(int i, out Property property)
        { 
            property = InBounds(i) && GetPlace(i) is Property? GetPlace(i) as Property : null;

            if(property == null) WriteLine(GetPlace(i).GetName() + " is not a property");

            return property != null;
        }
        bool TryToGetStreet(int i, out Street street)
        {
            street = InBounds(i) && GetPlace(i) is Street? GetPlace(i) as Street : null;

            if(street == null) WriteLine(GetPlace(i).GetName() + " is not a street you dumb");
            return street != null;
        }
        bool InBounds(int i)//board length
        {
            if (i >= 0 && i < 40) return true;
            
            WriteLine("wrong index");
            return false;
        }
    }
}
static bool TryToGetDigits(string line, out int value)
{
    var areDigits = int.TryParse(new String(line.Where(char.IsDigit).ToArray()), out var digits);

    //if(!areDigits) 
    value = digits;

    return areDigits;
}