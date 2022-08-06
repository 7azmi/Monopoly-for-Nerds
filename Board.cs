namespace Monopoly_for_Nerds;

public partial class Monopoly
{
    private static class Board
    {
        private static Place[] _places = new Place[]
        {
            new Go(),
            new Street(60, 50, new[] {2, 10, 30, 90, 160, 250}),
            new Chest(),
            new Street(60, 50, new[] {4, 20, 60, 180, 320, 450}),
            new Tax(),
            new Railroad(),
            new Street(100, 50, new[] {6, 30, 90, 270, 400, 550}),
            new Chance(),
            new Street(100, 50, new[] {6, 30, 90, 270, 400, 550}),
            new Street(120, 50, new[] {8, 40, 100, 300, 450, 600}),
            new Jail(),
            new Street(140, 100, new[] {10, 50, 150, 450, 625, 750}),
            new Company(),
            new Street(140, 100, new[] {10, 50, 150, 450, 625, 750}),
            new Street(160, 100, new[] {12, 60, 180, 500, 700, 900}),
            new Railroad(),
            new Street(180, 100, new[] {14, 70, 200, 550, 750, 950}),
            new Chest(),
            new Street(180, 100, new[] {14, 70, 200, 550, 750, 950}),
            new Street(200, 100, new[] {16, 80, 220, 600, 800, 1000}),
            new FreeParking(),
            new Street(220, 150, new[] {18, 90, 250, 700, 875, 1050}),
            new Chance(),
            new Street(220, 150, new[] {18, 90, 250, 700, 875, 1050}),
            new Street(240, 150, new[] {20, 100, 300, 750, 925, 1100}),
            new Railroad(),
            new Street(260, 150, new[] {22, 110, 330, 800, 975, 1150}),
            new Street(260, 150, new[] {22, 110, 330, 800, 975, 1150}),
            new Company(),
            new Street(280, 150, new[] {24, 120, 360, 850, 1025, 1200}),
            new GotoJail(),
            new Street(300, 200, new[] {26, 130, 390, 900, 1100, 1275}),
            new Street(300, 200, new[] {26, 130, 390, 900, 1100, 1275}), //Oxford
            new Chest(),
            new Street(320, 200, new[] {28, 150, 450, 1000, 1200, 1400}),
            new Railroad(),
            new Chance(),
            new Street(350, 200, new[] {35, 175, 500, 1100, 1300, 1500}),
            new Tax(),
            new Street(400, 200, new[] {50, 200, 600, 1400, 1700, 2000})
        };

        internal abstract class Place
        {
            public virtual void Land()
            {
                OnLand?.Invoke();
            }
        }

        internal abstract class Property : Place
        {
            public bool Mortgaged { get; set; }
            public Player Owner { get; set; }
            public bool Owned => Owner != null;

            public override void Land()
            {
                base.Land();
            }

            public virtual void OnProperty()
            {
                if (Owned)
                {
                    if (Owner != WhoseTurn)
                        Rent();
                }
                else OnBuyOrBid?.Invoke();
            }

            public abstract void Rent();
        }

        private class Go : Place
        {
            public override void Land()
            {
                WhoseTurn.GetSalary();
                base.Land();
            }
        }

        private class Street : Property
        {
            private int price;
            private int housePrice;
            public int houses = 0;
            public int[] rental; //6

            public Street(int price, int housePrice, int[] rental)
            {
                this.price = price;
                this.housePrice = housePrice;
                this.rental = rental;
            }

            public override void Rent() => Monopoly.OnRental?.Invoke(Owner, rental[houses]);
        }

        private class Chest : Place
        {
        }

        private class Tax : Place
        {
        }

        private class Railroad : Property
        {
            public override void Rent()
            {
                OnRental?.Invoke(Owner, RailRoadRental());

                int RailRoadRental()
                {
                    int rental = 25;
                    for (int i = 5; i <= 35; i += 5)
                    {
                        var railroad = (Railroad) Board._places[i];
                        if (railroad.Owner == Owner && railroad != this)
                        {
                            rental *= 2;
                        }
                    }

                    return rental;
                }
            }
        }

        private class Chance : Place
        {
        }

        private class Jail : Place
        {
        }

        private class Company : Property
        {
            public override void Rent()
            {
                OnRental?.Invoke(Owner, GetCompanyRental());

                int GetCompanyRental()
                {
                    var elec = (Property) Board._places[12];
                    var water = (Property) Board._places[28];

                    if (elec.Owner == water.Owner) return Dice.SumDice() * 10;
                    else return Dice.SumDice() * 4;
                }
            }
        }

        private class FreeParking : Place
        {
            //if you feel useless in life, just remember this parking place
        }

        private class GotoJail : Place
        {
            public override void Land()
            {
                base.Land();

                OnGoToJail?.Invoke(WhoseTurn);
            }
        }
    }
}