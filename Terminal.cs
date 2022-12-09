using Monopoly.Terminal;
using static Monopoly.Monopoly;
using static Monopoly.Monopoly.Board;
namespace Monopoly.Human;

public class Terminal : IPlatform
{
    public readonly string STAYINJAIL = "stay"; 
    protected readonly string GETOUTOFJAIL = "out"; 
    protected readonly string ROLLDICE = "roll"; 
    
    //protected readonly string FOLD = "fold";
    //protected readonly string 
    //todo ....
    public static (int, int) StreamLinePosition;
    public static (int, int) CommandBarPosition;

    private static ConsoleColor _commandBgColor = ConsoleColor.DarkGreen;
    private static ConsoleColor _defaultBgColor = ConsoleColor.Black;
    public void Log(string line)
    {
        Console.BackgroundColor = _defaultBgColor;
        
        Console.CursorVisible = false;
        CommandBarPosition = Console.GetCursorPosition();
        Console.SetCursorPosition(StreamLinePosition.Item1, StreamLinePosition.Item2);
        Console.WriteLine(line);
        StreamLinePosition = Console.GetCursorPosition();
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        Console.BackgroundColor = _commandBgColor;
    }
    public void WarningLog(string line)
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        
        Console.CursorVisible = false;
        CommandBarPosition = Console.GetCursorPosition();
        Console.SetCursorPosition(StreamLinePosition.Item1, StreamLinePosition.Item2);
        Console.WriteLine(line);
        StreamLinePosition = Console.GetCursorPosition();
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        Console.BackgroundColor = _commandBgColor;
    }
    public static void CommandBarLogger()
    {
        var BGColor = ConsoleColor.DarkGreen;
        
        //draw bg
        Console.BackgroundColor = BGColor;
        //Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);
        Console.WriteLine(new string(' ', 30));
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        var t =new Task( () =>
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    //Console.BackgroundColor = BGColor;
                    Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);
                    Console.CursorVisible = true;
                    
                    var s = Console.ReadLine();
                    
                    //clear
                    //Console.BackgroundColor = BGColor;
                    Console.SetCursorPosition(0, CommandBarPosition.Item2);
                    if (s != null) Console.Write(new string(' ', s.Length));
                    CommandBarPosition = (0, CommandBarPosition.Item2);
                    Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);
                    //Console.CursorVisible = true;
                }
            }
        });
        t.Start();
    }

    public Task ReadInput()
    {
        throw new NotImplementedException();
    }

    public static void PrintLine(string line)
    {
        Console.BackgroundColor = _defaultBgColor;
        Console.CursorVisible = false;
        
        //CommandBarPosition = Console.GetCursorPosition();
        //Console.SetCursorPosition(StreamLinePosition.Item1, StreamLinePosition.Item2);
        
        Console.WriteLine(line);
        
        //StreamLinePosition = Console.GetCursorPosition();
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        Console.BackgroundColor = _commandBgColor;
    }
    
    public static void Print(string text)
    {
        Console.BackgroundColor = _defaultBgColor;
        Console.CursorVisible = false;
        Console.Write(text);
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);
        Console.BackgroundColor = _commandBgColor;
    }

    #region Lovely checkers

    bool TryExecuteDivestPropertyCommand(Player player, string line)
    {
        var isLegalCommand = TryGetDivestPropertyCommand(player, line, out var command);
        if (isLegalCommand) command.Execute();

        return isLegalCommand;

        bool TryGetDivestPropertyCommand(Player player, string line, out Player.Command command)
        {
            command = null;

            string[] commands = { "sell house", "mort" }; //followed by digits

            if (!TryToGetDigits(line, out var i) || !InBounds(i)) return false;

            if (line.Contains(commands[0]) && TryToGetStreet(i, out var street))
                command = new Player.SellHouse(player, street);
            else if (line.Contains(commands[1]) && TryToGetProperty(i, out var property))
                command = new Player.MortgageProperty(player, property);

            return command != null ? command.IsLegal() : false;

        }
    }
    bool TryExecuteManagePropertyCommand(Player player, string line)
    {
        var isLegalCommand = TryGetPropertyManagementCommand(player, line, out var command);
        if (isLegalCommand) command.Execute();

        return isLegalCommand;

        bool TryGetPropertyManagementCommand(Player player, string line, out Player.Command command)
        {
            command = null;

            string[] commands = { "buy house", "sell house", "unmort", "mort" }; //followed by digits

            if (!TryToGetDigits(line, out var i) || !InBounds(i)) return false;

            var isStreet = TryToGetStreet(i, out var street);

            if (line.Contains(commands[0]) && isStreet) command = new Player.BuildHouse(player, street);
            else if (line.Contains(commands[1]) && isStreet) command = new Player.SellHouse(player, street);

            var isProperty = TryToGetProperty(i, out var property);

            if (line.Contains(commands[2]) && isProperty) command = new Player.UnmortgageProperty(player, property);
            else if (line.Contains(commands[3]) && isProperty) command = new Player.MortgageProperty(player, property);


            return command != null ? command.IsLegal() : false;
        }
    }
    bool TryToGetProperty(int i, out Property property)
    {
        property = InBounds(i) && GetPlace(i) is Property ? GetPlace(i) as Property : null;

        if (property == null) WarningLog(GetPlace(i).GetName() + " is not a property");

        return property != null;
    }
    bool TryToGetStreet(int i, out Street street)
    {
        street = InBounds(i) && GetPlace(i) is Street ? GetPlace(i) as Street : null;

        if (street == null) WarningLog(GetPlace(i).GetName() + " is not a street you dumb");
        return street != null;
    }
    bool InBounds(int i) //board length
    {
        if (i >= 0 && i < 40) return true;

        WarningLog("wrong index");
        return false;
    }
    bool TryToGetDigits(string line, out int value)
    {
        var areDigits = int.TryParse(new String(line.Where(char.IsDigit).ToArray()), out var digits);

        //if(!areDigits) 
        value = digits;

        return areDigits;
    }

    #endregion
}