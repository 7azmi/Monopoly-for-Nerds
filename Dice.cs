using System.Numerics;
using Kryz.Tweening;
using MonopolyTerminal.Interfaces;
using static MonopolyTerminal.Monopoly.Engine;
namespace MonopolyTerminal;

public partial class Monopoly
{
    public static class Dice
    {
        private static int _dice1;
        private static int _dice2;
        private static int _doubles = 0;

        static Random rand = new ();
        private static Vector2 _shuffleRange = new (2, 5);
        private static float _shuffleSpeed = 1000f;//500f;//best value :)
        
        public static int SumDice() => _dice1 + _dice2;
        
        public static (int _dice1, int _dice2) Roll()//might be changed on Unity
        {
            WhoseTurn.State &= ~PlayerState.ReadyForRolling;
            
            var rollings = rand.Next((int)_shuffleRange.X, (int)_shuffleRange.Y);
            var delay = rollings / _shuffleSpeed;
            
            for (var i = 0; i < rollings; i++)
            {
                Thread.Sleep( (int)(delay * EasingFunctions.OutQuart(i / (float)rollings)* 1000));
                
                _dice1 = rand.Next(1, 7);
                _dice2 = rand.Next(1, 7);
                //Console.Write(EasingFunctions.OutQuart(i / (float)rollings).ToString("0.00") + " ");
                OnDiceShuffle?.Invoke(_dice1, _dice2);
            }
            if(IsDouble()) IncreaseDoubleCounter();
            
            Human.Terminal.Log("rolled " + SumDice());//could be temp

            OnDiceRolled?.Invoke(GetDoubles());

            return (_dice1, _dice2);
        }
        
        public static bool PlayerCanRollAgain() => _dice1 ==_dice2 && _doubles <3 && _doubles != 0;//don't remove last condition
        public static bool PlayerCanRoll() => WhoseTurn.State.HasFlag(PlayerState.ReadyForRolling);//must actually
        public static bool IsDouble() => _dice1 ==_dice2;
        public static void IncreaseDoubleCounter() => _doubles++;
        public static void ResetDoubleCounter() => _doubles = 0;
        public static int GetDoubles() => _doubles;
    }
}