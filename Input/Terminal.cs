using MonopolyTerminal.Terminal;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Board;
namespace MonopolyTerminal.Human;

public class Terminal : IPlatform
{
    public readonly string STAYINJAIL = "stay"; 
    protected readonly string GETOUTOFJAIL = "out"; 
    protected readonly string ROLLDICE = "roll"; 
    protected readonly string FOLD = "fold";
    //protected readonly string 
    //todo ....
    public static (int, int) StreamLinePosition;
    public static (int, int) CommandBarPosition;

    private static ConsoleColor _commandBGColor = ConsoleColor.DarkGreen;
    private static ConsoleColor _defaultBGColor = ConsoleColor.Black;
    public static void Log(string line)
    {
        Console.BackgroundColor = _defaultBGColor;
        
        Console.CursorVisible = false;
        CommandBarPosition = Console.GetCursorPosition();
        Console.SetCursorPosition(StreamLinePosition.Item1, StreamLinePosition.Item2);
        Console.WriteLine(line);
        StreamLinePosition = Console.GetCursorPosition();
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        Console.BackgroundColor = _commandBGColor;
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

    public void LogLine(string line)
    {
        throw new NotImplementedException();
    }

    public Task ReadInput()
    {
        throw new NotImplementedException();
    }

    public static void PrintLine(string line)
    {
        Console.BackgroundColor = _defaultBGColor;
        Console.CursorVisible = false;
        
        //CommandBarPosition = Console.GetCursorPosition();
        //Console.SetCursorPosition(StreamLinePosition.Item1, StreamLinePosition.Item2);
        
        Console.WriteLine(line);
        
        //StreamLinePosition = Console.GetCursorPosition();
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        Console.BackgroundColor = _commandBGColor;
    }
    
    public static void Print(string text)
    {
        Console.BackgroundColor = _defaultBGColor;
        Console.CursorVisible = false;
        Console.Write(text);
        Console.SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);
        Console.BackgroundColor = _commandBGColor;
    }
}