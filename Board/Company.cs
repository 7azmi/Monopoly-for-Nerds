namespace MonopolyTerminal;

public partial class Monopoly
{
    public static partial class Board
    {
        public class Company : Property
        {
            public Company(int index, string name, int price) : base(index, price)
            {
                Name = name;
            }

            public override int GetRentalValue()
            {
                var elec = (Property)_places[12];
                var water = (Property)_places[28];

                if (elec.GetOwner() == water.GetOwner()) return Dice.SumDice() * 10;
                else return Dice.SumDice() * 4;
            }
        }
    }
}