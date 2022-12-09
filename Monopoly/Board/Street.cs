namespace Monopoly;

public partial class Monopoly
{
    public static partial class Board
    {
        public class Street : Property
        {
            public Street(int index, string name, int price, int housePrice, int[] rental, int setIndex) : base(index,
                price)
            {
                Name = name;
                Price = price;
                _housePrice = housePrice;
                _rental = rental;
                _setIndex = setIndex;
            }

            private int _housePrice;
            private int _houses = 0;
            public bool NoHouses => _houses == 0;
            public bool MaxHouses => _houses == 5;
            public bool HasHouses => _houses > 0;
            public int HouseCount => _houses;

            private int _setIndex;
            public int SetIndex => _setIndex;

            public Street[] GetStreetSet()
            {
                var set = new List<Street>();
                foreach (var place in _places)
                {
                    if (place is Street s && s._setIndex == _setIndex) set.Add(s);
                }

                return set.ToArray();
            }

            private int[] _rental; //6
            public new bool CanBeMortgaged() => !IsMortgaged() && NoHouses;


            public int[] Rentals => _rental;

            public bool PlayerCanBuildHouseHere => GetOwner().HasEnoughMoney(_housePrice)
                                                   && IsCompleteSetProperty
                                                   && HouseCanBeBuiltOrderRule;

            public bool HouseCanBeBuiltOrderRule =>
                GetStreetSet().Any(street => street._houses > _houses && street != this)
                || GetStreetSet().All(street => street._houses == _houses);

            //public bool HouseOrder

            public bool IsCompleteSetProperty => GetStreetSet().All(s => s.Owned && s.GetOwner() == GetOwner());

            public override int GetRentalValue()
            {
                return _rental[_houses];
            }

            public void AddHouse() => _houses++; //yeah!
            public int GetHousePrice() => _housePrice;

            public void RemoveHouse() => _houses--;

            public Player GetSetOwner()
            {
                var set = GetStreetSet();
                Player owner = GetStreetSet().First().GetOwner();

                foreach (var street in set)
                {
                    if (street.GetOwner() == null || street.GetOwner() != owner) return null;
                }

                return owner;
            }
        }
    }
}