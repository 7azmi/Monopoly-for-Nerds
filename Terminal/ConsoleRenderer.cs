using MonopolyTerminal.Enums;

namespace MonopolyTerminal.Terminal;

public class ConsoleRenderer : IConsoleRenderer
{
    private const int OFFSET_X = 0;
	private const int OFFSET_Y = 0;
	private static Coord OFFSET => new Coord(OFFSET_X, OFFSET_Y);
	
	#region CONST SYMBOLS // ░▒▓█■∙
	
	private const char EMPTY_SYMBOL = '∙';
	private const char BORDER_SYMBOL = '█';
	private const char SNAKE_SYMBOL = '▓';
	private const char SNAKE_HEAD_SYMBOL = 'S';
	private const char FRUIT_SYMBOL = 'F';
	private const char BOMB_SYMBOL = 'B';
	
	#endregion

	#region CONST COLORS

	private const ConsoleColor WHITE_COLOR = ConsoleColor.White;
	private const ConsoleColor BLACK_COLOR = ConsoleColor.Black;
	private const ConsoleColor SNAKE_COLOR = ConsoleColor.Green;
	private const ConsoleColor SNAKE_DEAD_COLOR = ConsoleColor.DarkGreen;
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;
	private const ConsoleColor BORDER_COLOR = ConsoleColor.Gray;
	private const ConsoleColor GRID_COLOR = ConsoleColor.DarkGray;
	private const ConsoleColor SCORE_COLOR = ConsoleColor.DarkCyan;
	private const ConsoleColor BOMB_ON_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor BOMB_OFF_COLOR = ConsoleColor.DarkYellow;
	private const ConsoleColor PLAYER_DEATH_COLOR = ConsoleColor.DarkRed;
	private const ConsoleColor RESTART_TEXT_COLOR = ConsoleColor.Cyan;

	#endregion

	private readonly GameSettings _settings;

	private static ConsoleBlock dice1 = new ConsoleBlock(new Coord(95, 15));
	private static ConsoleBlock dice2 = new ConsoleBlock(new Coord(104, 15));
	
	private static ConsoleBlock doge = new ConsoleBlock(new Coord(30, 6));
	

	public ConsoleRenderer(GameSettings settings)
	{
		_settings = settings;
		Console.CursorVisible = false;
	}

	public static void InitDiceRenderer()
	{
		Monopoly.Engine.OnDiceShuffle += (d1, d2) =>
		{
			dice1.Update(GetDiceInfo(d1));
			dice2.Update(GetDiceInfo(d2));
		};
		
		string[] GetDiceInfo(int face)
		{
			//Console.WriteLine("═║╒╓╔╕╖╗╘╙╚ ╛ ╜╝╞╟╠╡╢╣╤╥╦ ╧╨╩ ╪╫╬─│┌┐└┘├┬┴┼");
			string[] result = face switch
			{
				1 => new string[]
				{
					"┌───────┐",
					"│       │",
					"│   #   │",
					"│       │",
					"└───────┘"
				},
				2 => new string[]
				{
					"┌───────┐",
					"│ #     │",
					"│       │",
					"│     # │",
					"└───────┘"
				},
				3 => new string[]
				{
					"┌───────┐",
					"│ #     │",
					"│   #   │",
					"│     # │",
					"└───────┘"
				},
				4 => new string[]
				{
					"┌───────┐",
					"│ #   # │",
					"│       │",
					"│ #   # │",
					"└───────┘"
				},
				5 => new string[]
				{
					"┌───────┐",
					"│ #   # │",
					"│   #   │",
					"│ #   # │",
					"└───────┘"
				},
				6 => new string[]
				{
					"┌───────┐",
					"│ # # # │",
					"│       │",
					"│ # # # │",
					"└───────┘"
				},

				_ => new string[] {" mdri?? "}
			};

			return result;
		}
	}

	public static void RenderDoge()
	{
		doge.Update(new []{
			new string("         ▄              ▄    "),
		new string("        ▌▒█           ▄▀▒▌   "),
		new string("        ▌▒▒█        ▄▀▒▒▒▐   "),
		new string("       ▐▄█▒▒▀▀▀▀▄▄▄▀▒▒▒▒▒▐   "),
		new string("     ▄▄▀▒▒▒▒▒▒▒▒▒▒▒█▒▒▄█▒▐   "),
		new string("   ▄▀▒▒▒░░░▒▒▒░░░▒▒▒▀██▀▒▌   "),
		new string("  ▐▒▒▒▄▄▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▀▄▒▌  "),
		new string("  ▌░░▌█▀▒▒▒▒▒▄▀█▄▒▒▒▒▒▒▒█▒▐  "),
		new string(" ▐░░░▒▒▒▒▒▒▒▒▌██▀▒▒░░░▒▒▒▀▄▌ "),
		new string(" ▌░▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▌ "),
		new string("▌▒▒▒▄██▄▒▒▒▒▒▒▒▒░░░░░░░░▒▒▒▐ "),
		new string("▐▒▒▐▄█▄█▌▒▒▒▒▒▒▒▒▒▒░▒░▒░▒▒▒▒▌"),
		new string("▐▒▒▐▀▐▀▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒░▒▒▐ "),
		new string(" ▌▒▒▀▄▄▄▄▄▄▒▒▒▒▒▒▒▒░▒░▒░▒▒▒▌ "),
		new string(" ▐▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒▒▄▒▒▐  "),
		new string("  ▀▄▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒▄▒▒▒▒▌  "),
		new string("    ▀▄▒▒▒▒▒▒▒▒▒▒▄▄▄▀▒▒▒▒▄▀   "),
		new string("      ▀▄▄▄▄▄▄▀▀▀▒▒▒▒▒▄▄▀     "),
		new string("         ▀▀▀▀▀▀▀▀▀▀▀▀        "),
		// new string("         ▄              ▄    "),
		// new string("        ▌▒█           ▄▀▒▌   "),
		// new string("        ▌▒▒█        ▄▀▒▒▒▐   "),
		// new string("       ▐▄█▒▒▀▀▀▀▄▄▄▀▒▒▒▒▒▐   "),
		// new string("     ▄▄▀▒▒▒▒▒▒▒▒▒▒▒█▒▒▄█▒▐   "),
		// new string("   ▄▀▒▒▒░░░▒▒▒░░░▒▒▒▀██▀▒▌   "),
		// new string("  ▐▒▒▒▄▄▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▀▄▒▌  "),
		// new string("  ▌░░▌█▀▒▒▒▒▒▄▀█▄▒▒▒▒▒▒▒█▒▐  "),
		// new string(" ▐░░░▒▒▒▒▒▒▒▒▌██▀▒▒░░░▒▒▒▀▄▌ "),
		// new string(" ▌░▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▌ "),
		// new string("▌▒▒▒▄██▄▒▒▒▒▒▒▒▒░░░░░░░░▒▒▒▐ "),
		// new string("▐▒▒▐▄█▄█▌▒▒▒▒▒▒▒▒▒▒░▒░▒░▒▒▒▒▌"),
		// new string("▐▒▒▐▀▐▀▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒░▒▒▐ "),
		// new string(" ▌▒▒▀▄▄▄▄▄▄▒▒▒▒▒▒▒▒░▒░▒░▒▒▒▌ "),
		// new string(" ▐▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒▒▄▒▒▐  "),
		// new string("  ▀▄▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒▄▒▒▒▒▌  "),
		// new string("    ▀▄▒▒▒▒▒▒▒▒▒▒▄▄▄▀▒▒▒▒▄▀   "),
		// new string("      ▀▄▄▄▄▄▄▀▀▀▒▒▒▒▒▄▄▀     "),
		// new string("         ▀▀▀▀▀▀▀▀▀▀▀▀        "),
		});
	}
	
	
	private static void Print(Coord coord, string text, ITextStyle<ConsoleColor> textStyle)
	{
		var consoleText = new ConsoleText(coord + OFFSET, text, textStyle);
		consoleText.Print();
	}

	public void RenderCard(Card card)
	{
		
	}
}