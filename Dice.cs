namespace Monopoly_for_Nerds;

public partial class Monopoly
{
    public static class Dice
    {
        private static int _dice1;
        private static int _dice2;
        private static int _doubles;

        public static int SumDice() => _dice1 + _dice2;

        public static (int, int) Roll()
        {
            Random r = new Random();
            _dice1 = r.Next(1, 6);
            _dice2 = r.Next(1, 6);
            return (_dice1, _dice2);
        }


        public static void IncreaseDoubleCounter() => _doubles++;
        public static void ResetDoubleCounter() => _doubles = 0;
    }
}