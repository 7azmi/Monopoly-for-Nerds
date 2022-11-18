
using System.Data;
using MonopolyTerminal;

public class ConsoleText : IText<ITextStyle<ConsoleColor>>
{
	public Coord Coord { get; private set; }
	public string Value { get; private set; }
	public ITextStyle<ConsoleColor> TextStyle { get; private set; }

	public ConsoleText(Coord coord, char symbol) 
		: this(coord, symbol, ConsoleTextStyle.Default) { }
	public ConsoleText(Coord coord, char symbol, ITextStyle<ConsoleColor> textStyle) 
		: this(coord, $"{symbol}", textStyle) { }
	public ConsoleText(Coord coord, string text) 
		: this(coord, text, ConsoleTextStyle.Default) { }
	public ConsoleText(Coord coord, string text, ITextStyle<ConsoleColor> textStyle)
	{
		Coord = coord;
		Value = text;
		TextStyle = textStyle;
	}

	public void Print() => Print(Value);
	private void Print(string text)
	{
		var previousCoord = new Coord(Console.CursorLeft, Console.CursorTop);

		Console.SetCursorPosition(Coord.X, Coord.Y);
		Console.ForegroundColor = TextStyle.Foreground;
		Console.BackgroundColor = TextStyle.Background;
		Console.Write(text);
		Console.ResetColor();
		Console.SetCursorPosition(previousCoord.X, previousCoord.Y);
	}

	public void Update(string text)
	{
		Clear();
		Value = text;
		Print();
	}

	public void Clear()
	{
		Print(new string(' ', Value.Length));
	}
}
