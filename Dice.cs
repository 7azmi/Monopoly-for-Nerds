using System.Numerics;
using Kryz.Tweening;
using static MonopolyTerminal.Monopoly.Engine;
namespace MonopolyTerminal;

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
        
        static Random rand = new Random();
        private static Vector2 shuffleRange = new Vector2(10, 20);
        private static float shuffleSpeed = 500f;//best value :)
        
        public static int SumDice() => _dice1 + _dice2;

        public static DiceState GetDiceState() => _state;

        public static void SetDiceState(DiceState s)
        {
            switch (s)
            {
                case DiceState.ReadyForRolling:
                    OnDiceReadyForRolling?.Invoke(GetDoubles());
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
        public static (int _dice1, int _dice2) Roll()//might be changed on Unity
        {
            SetDiceState(DiceState.Rolling);
            
            var rollings = rand.Next((int)shuffleRange.X, (int)shuffleRange.Y);
            var delay = rollings / shuffleSpeed;
            
            for (int i = 0; i < rollings; i++)
            {
                Thread.Sleep((int)(delay * EasingFunctions.OutQuart(i / (float)rollings)* 1000));

                
                _dice1 = rand.Next(1, 7);
                _dice2 = rand.Next(1, 7);
                //Console.Write(EasingFunctions.OutQuart(i / (float)rollings).ToString("0.00") + " ");
                OnDiceShuffle?.Invoke(_dice1, _dice2);

            }
            
            
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