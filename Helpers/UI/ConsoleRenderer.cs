using MonopolyTerminal.Enums;
using static System.Console;

namespace MonopolyTerminal;

// ═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬
// ─│┌┐└┘├┬┴┼
// ♦◊◌●☼
// █▓▒░
// ■□▪▫
// ▲►▼◄

public class ConsoleRenderer
{
	#region CONST SYMBOLS
	
	
	#endregion

	#region CONST COLORS

	private const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
	private const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;
	private const ConsoleColor SNAKE_COLOR = ConsoleColor.Green;
	private const ConsoleColor SNAKE_DEAD_COLOR = ConsoleColor.DarkGreen;
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;
	private const ConsoleColor WALL_COLOR = ConsoleColor.Gray;
	private const ConsoleColor WALL_DARK_COLOR = ConsoleColor.DarkGray;
	private const ConsoleColor SCORE_COLOR = ConsoleColor.DarkCyan;
	private const ConsoleColor PLAYER_DEATH_COLOR = ConsoleColor.DarkRed;
	private const ConsoleColor RESTART_TEXT_COLOR = ConsoleColor.Cyan;
	private const ConsoleColor BOMB_ON_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor BOMB_OFF_COLOR = ConsoleColor.DarkYellow;

	#endregion

	
	public void Render(Coord coord, char symbol, ConsoleColor color = DEFAULT_COLOR, ConsoleColor bgColor = ConsoleColor.Black) 
		=> Print(coord, symbol, color, bgColor);

	private void Print(Coord coord, char character, ConsoleColor color = DEFAULT_COLOR, ConsoleColor bgColor = ConsoleColor.Black)
		=> Print(coord, $"{character}", color, bgColor);

	private static void Print(Coord coord, string text, ConsoleColor color = DEFAULT_COLOR, ConsoleColor bgColor = ConsoleColor.Black)
	{
		var foregroundColor = ForegroundColor;
		var backgroundColor = BackgroundColor;
		ForegroundColor = color;
		BackgroundColor = bgColor;
		SetCursorPosition(coord.X + 3, coord.Y + 2);
		Write(text);
		ForegroundColor = foregroundColor;
		BackgroundColor = backgroundColor;
	}

	public void Clear(Coord coord) 
		=> Print(coord, " ");

	private static ConsoleColor GetColorByType(ColorType type)
		=> type switch
		{
			ColorType.White => DEFAULT_COLOR,
			ColorType.Black => DEFAULT_BACKGROUND_COLOR,
			ColorType.Green => SNAKE_COLOR,
			ColorType.DarkGreen => SNAKE_DEAD_COLOR,
			ColorType.Red => FRUIT_COLOR,
			ColorType.Yellow => BOMB_ON_COLOR,
			ColorType.DarkYellow => BOMB_OFF_COLOR,
			ColorType.DarkCyan => SCORE_COLOR,
			ColorType.Grey => WALL_COLOR,
			ColorType.DarkGrey => WALL_DARK_COLOR,
			ColorType.DarkRed => PLAYER_DEATH_COLOR,
			ColorType.Cyan => RESTART_TEXT_COLOR,
			_ => DEFAULT_COLOR
		};
}