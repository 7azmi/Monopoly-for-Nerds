///Created by nGAGEOnline
///https://www.youtube.com/c/MMOJunkie
/// 
namespace Monopoly_for_Nerds;
using static Console;
public static class TypingSimulator
{
	static TypingSimulator()
	{
		//rng = new Random();
	}
	
		static float overallSpeed = 1.5f;

		static int pauseMin = 20;
		static int pauseMax = 50;
		static int multiplier = 7;
		static int demultiplier = 3;
		static int breakMin = 100;
		static int breakMax = 600;
		static int punctuationBreakMin = 2;
		static int punctuationBreakMax = 6;

		static char[] pauses = { ' ', '.', ',', '?', '!', '$', '&', '*', '-', '=', '+', '@', '#', '%', '(', ')', '[', ']', '<', '>', '|', ':', ';', '/', '\\', '\'', '\"', '\n', '\t', '\r', '\b' };
		private static Random rng = new Random();

		//static void Main(string[] args)
		//{
		//	rng = new Random();
		//	
		//	string text = "Hello, World!\b\b\b\b\b\b\b\n\t" +
		//		"This is a little program that types out some text, making it appear as if it were being typed out by a human.\b\b\b\b\b\bperson.\n" +
		//		"There's not really much to it, but I thought it a fun little distraction. It should be easy enough to expand upon it." +
		//		"\n\nEnjoy.\b!!!\n\n\nThis wacky little \"typing\" program was written/created by: \n\t\tnGAGEOnline :P\n\n\n";
//
		//	WriteLine("Press Any Key To Start Demo...");
		//	ReadKey();
		//	Clear();
//
		//	TypeOutText(text);
//
		//	Thread.Sleep(1000);
		//}

		public static string TypeOutText(string text, bool startDelay = true) => TypeOutText(text.ToCharArray(), startDelay);

		private static string TypeOutText(char[] charArray, bool startDelay = true)
		{
			if (startDelay)
				Thread.Sleep(rng.Next(250, 1000));

			for (int i = 0; i < charArray.Length; i++)
			{
				char letter = charArray[i];
				int sleep = (int)(rng.Next(pauseMin, rng.Next(pauseMin + 1, pauseMax) * multiplier) / overallSpeed);
				if (pauses.Any(x => x == letter))
					sleep += rng.Next(breakMin, breakMax);

				if (letter == '\b')
				{
					sleep /= demultiplier;
					Write(letter + " ");
				}

				if (letter == ' ' || letter == '.' || letter == ',' || letter == '?' || letter == '!')
					Thread.Sleep(sleep / rng.Next(punctuationBreakMin, punctuationBreakMax));
				else if (letter == '\n' || letter == '\r' || letter == '\"' || letter == '\'')
					Thread.Sleep(sleep);

				Write(letter);

				Thread.Sleep(sleep);
			}
			Thread.Sleep(rng.Next(250, 1000));

			WriteLine();
			return new string(charArray);
		}
}