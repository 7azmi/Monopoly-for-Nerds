namespace Monopoly_for_Nerds;
using static Monopoly.Board;
using static Monopoly.Dice;
public partial class Monopoly
{
    //Engine Actions controls what commands to perform according to game rules.
    //Control your UI through these actions.
    public static class Engine
    { 
        static Engine()
        {
            //OnInitialization += Init;
            //OnStart += Start;
            //OnPlayerTurn += PlayerTurn;
            OnPlayerTurnWhileInJail += PlayerTurnWhileInJail;
            OnMovingOnPlace += MovingOnPlace;
            OnLandingOnUnownedProperty += LandingOnUnownedProperty;
            OnRentalDue += LandingOnOwnedProperty;
            OnAuction += Auction;
            //OnDiceReadyForRolling += DiceReadyForRolling;
            OnDiceRolled += times =>
            {
                if (times > 2)
                {
                    Console.WriteLine("Player rolled double thrice, go to jail you cheater!");
                    WhoseTurn.GoJail();
                }
                else
                {
                    WhoseTurn.Move(GetPath(SumDice()));

                    Place[] GetPath(int steps)
                    {
                        List<Place> properties = new List<Place>();

                        var i = WhoseTurn.GetCurrentOccupationByIndex();
                        
                        while (steps-- > 0)
                        {
                            //i++;

                            if (++i > 39) i = 0;
                            
                            properties.Add(GetPlace(i));
                            //or maybe properties.Add(GetPlace(++i % 40));
                            //steps--;
                        }
                        return properties.ToArray();
                    }
                }
            };
            //OnNextPlayerTurn += NextPlayerTurn;

        }

        //actions
        //start
        public static Action<Monopoly> OnInitialization;
        public static Action OnStart;
        
        //public static Action<Player> OnPlayerTurn;
        public static Action<Player> OnPlayerTurnWhileInJail;
        public static Action<Player> OnPlayerDecidesToStayInJail;
        public static Action<int, Player> OnDiceReadyForRolling;
        public static Action<int> OnDiceRolled;
        
        public static Action<Place, Player> OnMovingOnPlace;

        public static Action<Place> OnLanding;

        public static Action<Player> OnPlayerGetsToJail;
        public static Action<Player> OnPlayerGetsOutOfJail;

        public static Action<Player ,Property> OnLandingOnMyProperty;
        
        public static Action<Player ,Place> OnLandingCompleted; //could be temp

        public static Action<Player, Property> OnLandingOnUnownedProperty;
        public static Action<Player, Property> OnBuyProperty;
        public static Action<Property, Player, int> OnRentalDue;
        public static Action<Property, Player, int> OnRentalPaid;
        public static Action<Property> OnAuction;
        public static Action<Player, Property> OnCloseAuction;
        public static Action<Player, Property, int> OnBid;
        public static Action<Player, Player> OnSetDeal;
        public static Action OnSwitchTurnOrRollAgain;
        public static Action<Player> OnNextPlayerTurn;


        public static void Init(Monopoly monopoly)
        {
            OnInitialization?.Invoke(monopoly);
        }
        public static void Start()
        {
            foreach (var p in ActivePlayers)
                p.SetStartingOccupation(23);

            OnStart?.Invoke();//don't tell me why

            SetDiceState(DiceState.ReadyForRolling);
        }
        //private static void DiceReadyForRolling(int times, Player whoseTurn)
        //{
        //    //SetDiceState(DiceState.ReadyForRolling);
        //}
        private static void PlayerTurnWhileInJail(Player bonk)
        {
            
        }

        private static void MovingOnPlace(Place place, Player player)
        {
            if (place is Board.Go go)
            {
                player.GetSalary(200);
            }
        }
        static void LandingOnUnownedProperty(Player player, Property property)
        {
            
        }
        static void LandingOnOwnedProperty(Property property, Player victim, int rental)
        {
            
        }
        static void Auction(Property onSale)
        {
            
        }

        //public static void SwitchTurnOrRollAgain()
        //{
        //    if (PlayerCanRollAgain()) SetDiceState(DiceState.ReadyForRolling);
        //    else SwitchNextPlayerTurn();
        //    
        //    OnSwitchTurnOrRollAgain?.Invoke();
        //}
        //SetDeal
        
        public static void SwitchNextPlayerTurn()
        {
            ResetDoubleCounter();
            
            NextPlayer();
            
            OnNextPlayerTurn?.Invoke(WhoseTurn);

            if (WhoseTurn.InJail) OnPlayerTurnWhileInJail?.Invoke(WhoseTurn);//window Jail.AskPlayerToGetOutOrStayInJail(); //window
            else SetDiceState(DiceState.ReadyForRolling);
            
            void NextPlayer()
            {
                if (WhoseTurn == ActivePlayers.Last())
                    WhoseTurn = ActivePlayers.First();
                else
                {
                    for (int i = 0; i < ActivePlayers.Count-1; i++)
                    {
                        if (WhoseTurn == ActivePlayers[i])
                        {
                            WhoseTurn = ActivePlayers[i + 1];
                            break;
                        }
                    }
                }
               
            }
        }
    }
}