
public class ConsoleTextStyle : ITextStyle<ConsoleColor> 
{
	#region STATICS

	public static ConsoleTextStyle Default { get; }
	public static ConsoleTextStyle Border { get; }
	
	public static ConsoleTextStyle Player1 { get; }
	public static ConsoleTextStyle Player2 { get; }
	public static ConsoleTextStyle Player3 { get; }
	public static ConsoleTextStyle Player4 { get; }

	#endregion

	public ConsoleColor Foreground { get; }
	public ConsoleColor Background { get; }

	public ConsoleTextStyle(ConsoleColor foreground, ConsoleColor background)
	{
		Foreground = foreground;
		Background = background;
	}
	static ConsoleTextStyle()
	{
		Default = new ConsoleTextStyle(ConsoleColor.Gray, ConsoleColor.Black);
		Border = new ConsoleTextStyle(ConsoleColor.DarkGray, ConsoleColor.Gray);
		Player1 = new ConsoleTextStyle(ConsoleColor.Red, ConsoleColor.Black);
		Player2 = new ConsoleTextStyle(ConsoleColor.Green, ConsoleColor.Black);
		Player3 = new ConsoleTextStyle(ConsoleColor.Yellow, ConsoleColor.Black);
		Player4 = new ConsoleTextStyle(ConsoleColor.Blue, ConsoleColor.Black);
	}
}
