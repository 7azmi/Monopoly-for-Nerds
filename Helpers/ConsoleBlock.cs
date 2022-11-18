
using MonopolyTerminal;

public class ConsoleBlock
{
	public Coord Coord { get; private set; }
	public string[] Values { get; private set; }
	public ITextStyle<ConsoleColor> TextStyle { get; private set; }
	
	public ConsoleBlock(Coord coord, string[] lines, ITextStyle<ConsoleColor> textStyle)
	{
		Coord = coord;
		Values = lines;
		TextStyle = textStyle;
	}
	
	public ConsoleBlock(Coord coord, string[] lines)
	{
		Coord = coord;
		Values = lines;
		TextStyle = ConsoleTextStyle.Default;
	}
	
	public ConsoleBlock(Coord coord)
	{
		Coord = coord;
		Values = new string[] { };
		TextStyle = ConsoleTextStyle.Default;
	}

	public void Print() => Print(Values);
	private void Print(string[] lines)
	{
		var previousCoord = new Coord(Console.CursorLeft, Console.CursorTop);
		
		Console.ForegroundColor = TextStyle.Foreground;
		Console.BackgroundColor = TextStyle.Background;

		for (var i = 0; i < lines.Length; i++)
		{
			var line = lines[i];
			Console.SetCursorPosition(Coord.X, Coord.Y + i);

			Console.Write(line);
		}

		Console.SetCursorPosition(previousCoord.X, previousCoord.Y);
		Console.ResetColor();
	}

	public void Update(string[] text)
	{
		Clear();
		Values = text;
		Print();
	}

	public void Clear()
	{
		string[] empty = new string[Values.Length];

		for (var i = 0; i < Values.Length; i++)
		{
			empty[i] = new string(' ', Values[i].Length);
			//Print(new string[]new string(' ', value.Length));
		}
		
		Print(empty);
	}
}
