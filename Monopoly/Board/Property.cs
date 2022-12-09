namespace Monopoly;

public partial class Monopoly
{
    public static partial class Board
    {
        public abstract class Property : Place
        {
            protected Property(int index, int price) : base(index)
            {
                Price = price;
            }

            private bool _mortgaged;
            private Player _owner;
            protected int Price;
            public bool Owned => GetOwner() != null;
            public int MortgageValue => Price / 2;
            public int UnmortgageValue => (int)((float)MortgageValue * 1.1f); //that's what it's all about


            public override void Land()
            {
                base.Land();

                LandOnProperty();
            }

            private void LandOnProperty()
            {
                if (Owned)
                {
                    if (GetOwner() != WhoseTurn)
                    {
                        if (!IsMortgaged()) RequestRental();
                        else Engine.OnLandingCompleted.Invoke(this);
                    }
                    else Engine.OnLandingOnMyProperty?.Invoke(WhoseTurn, this); //home sweet home
                }
                else Engine.OnLandingOnUnownedProperty?.Invoke(this);
            }

            private void RequestRental() => Engine.OnRentalDue?.Invoke(this, WhoseTurn, GetRentalValue());
            public void SetOwner(Player newOwner) => _owner = newOwner;
            public Player GetOwner() => _owner;
            public bool HasOwner => _owner != null;
            public int GetPrice() => Price;
            public bool IsMortgaged() => _mortgaged;
            public bool CanBeMortgaged() => !IsMortgaged();

            public void Mortgage()
            {
                _mortgaged = true;
            }

            public bool CanBeUnmortgaged(Player player) => player.HasEnoughMoney(UnmortgageValue) && IsMortgaged();
            public abstract int GetRentalValue();

            public void Unmortgage()
            {
                _mortgaged = false;
            }
        }
    }
}