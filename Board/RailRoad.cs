namespace MonopolyTerminal;

public partial class Monopoly
{
    public static partial class Board
    {
        public class Railroad : Property
        {
            public Railroad(int index, string name, int price) : base(index, price)
            {
                Name = name;
            }

            public override int GetRentalValue()
            {
                var rental = 25;
                for (var i = 5; i <= 35; i += 10)
                {
                    var railroad = _places[i] as Railroad;
                    if (railroad.GetOwner() == GetOwner() && railroad != this)
                    {
                        rental *= 2;
                    }
                }

                return rental;
            }
        }
    }
}