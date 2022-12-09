namespace Monopoly;

public interface IPlatform
{
    void Log(string line);
    void WarningLog(string line);
    public virtual async Task ReadInput()
    {
    }
}