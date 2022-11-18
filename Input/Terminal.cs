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

    public void Log(string line)
    {
        
        Console.Write(line);
    }

    public void LogLine(string line)
    {
        Console.Write(line);

    }

    public Task ReadInput()
    {
        throw new NotImplementedException();
    }
}