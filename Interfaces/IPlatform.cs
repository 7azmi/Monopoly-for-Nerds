namespace MonopolyTerminal;

public interface IPlatform
{
    void Log(string line);
    void LogLine(string line);

    public virtual async Task ReadInput()
    {
        Log("heheboi");
    }
}