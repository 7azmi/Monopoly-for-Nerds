namespace Monopoly_for_Nerds;
using static Monopoly.Engine;
public partial class Monopoly
{
    public static class Dice
    {
        static Dice()
        {
            OnRentalPaid += (property, player, rentalValue) =>
            {
                if (PlayerCanRollAgain())
                {
                    SetDiceState(DiceState.ReadyForRolling);
                }
            };
        }
        private static int _dice1;
        private static int _dice2;
        private static int _doubles = 0;

        private static DiceState _state;
        public static int SumDice() => _dice1 + _dice2;

        public static DiceState GetDiceState() => _state;

        public static void SetDiceState(DiceState s)
        {
            switch (s)
            {
                case DiceState.ReadyForRolling:
                    OnDiceReadyForRolling?.Invoke(GetDoubles(), WhoseTurn);
                    _state = DiceState.ReadyForRolling;
                    break;
                
                case DiceState.Rolling:
                    _state = DiceState.Rolling;
                    break;

                case DiceState.Rolled:
                    _state = DiceState.Rolled;
                    OnDiceRolled?.Invoke(GetDoubles());
                    break;
                
                default: _state = DiceState.ReadyForRolling;
                    break;
            }
        }
        public static (int, int) Roll()//might be changed on Unity
        {
            SetDiceState(DiceState.Rolling);
            
            Random r = new Random();
            _dice1 = r.Next(1, 7);
            _dice2 = r.Next(1, 7);
            
            if(IsDouble()) IncreaseDoubleCounter();
            
            Console.WriteLine("rolled " + SumDice());//could be temp
            
            SetDiceState(DiceState.Rolled);

            return (_dice1, _dice2);
        }
        
        public static bool PlayerCanRollAgain() => _dice1 ==_dice2 && _doubles <3 && _doubles != 0;//don't remove last condition
        public static bool PlayerCanRoll() => _state == DiceState.ReadyForRolling;//must actually
        public static bool IsDouble() => _dice1 ==_dice2;
        public static void IncreaseDoubleCounter() => _doubles++;
        public static void ResetDoubleCounter() => _doubles = 0;
        
        public enum DiceState
        {
            ReadyForRolling, Rolling, Rolled
        }

        public static int GetDoubles() => _doubles;
    }
}
