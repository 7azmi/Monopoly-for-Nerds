using MonopolyTerminal.Interfaces;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Dice;
namespace MonopolyTerminal;

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
            OnRentalDue += PayRental;
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
        public static Action<int> OnDiceReadyForRolling;
        public static Action<int, int> OnDiceShuffle;
        public static Action<int> OnDiceRolled;
        
        public static Action<Place, Player> OnMovingOnPlace;

        public static Action<Place> OnLanding;

        public static Action<Player> OnPlayerGetsToJail;

        public static Action<Player> OnPlayerGetsOutOfJail;

        public static Action<Player ,Property> OnLandingOnMyProperty;
        
        public static Action<Place> OnLandingCompleted; //could be temp

        public static Action<Property> OnLandingOnUnownedProperty;

        public static Action<Property> OnBuyProperty;
        
        public static Action<Property, Player, int> OnRentalDue;
        public static Action<Property, Player, int> OnRentalPaid;
        public static Action<Property>  OnAuction;
        public static Action<Player, Property> OnCloseAuction;
        public static Action<Player, Property, int> OnBid;
        
        public static Action<Player, Property> OnPlayerMortgage;
        public static Action<Player, Property> OnPlayerUnmortgage;
        public static Action<Player, Street> OnPlayerBuyHouse;
        public static Action<Player, Street> OnPlayerSellHouse;
        
        public static Action<Player, int> OnPlayerInDebt;
        
        
        public static Action<Player, Player, Player.SetOffer.Offer> OnOffering;
        public static Action<Player, Player, Player.SetOffer.Offer> OnAcceptOffer;
        public static Action<Player, Player> OnDeclineOffer;
        
        public static Action OnRollAgain;
        public static Action OnTurnCompleted;
        public static Action<Player> OnPlayerTurn;


        public static void Init(Monopoly monopoly)
        {
            OnInitialization?.Invoke(monopoly);
        }

        public static void LateInit()
        {
            OnLandingCompleted += _ =>  RollAgainOrFinishTurn();
            OnBuyProperty += _ => RollAgainOrFinishTurn();
        }

        private static void RollAgainOrFinishTurn()
        {
            if (!WhoseTurn.InJail && PlayerCanRollAgain())
            {
                OnDiceReadyForRolling.Invoke(GetDoubles());
            }
            else
            {
                OnTurnCompleted.Invoke();
            }
        }

        public static void Start()
        {
            foreach (var p in ActivePlayers)
                p.SetStartingOccupation(15);

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
        static void LandingOnUnownedProperty(Property property)
        {
            
        }
        static void PayRental(Property property, Player victim, int rental)
        {
            Console.WriteLine($"you landed on {property.GetName()}");
            new Player.PayRent(victim, property).Execute();
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
            
            OnPlayerTurn?.Invoke(WhoseTurn);

            if (WhoseTurn.InJail) OnPlayerTurnWhileInJail?.Invoke(WhoseTurn);//window Jail.AskPlayerToGetOutOrStayInJail(); //window
            else SetDiceState(DiceState.ReadyForRolling);
            
            void NextPlayer()
            {
                if (WhoseTurn == ActivePlayers.Last())
                    WhoseTurn = ActivePlayers.First();
                else
                {
                    for (var i = 0; i < ActivePlayers.Count-1; i++)
                    {
                        if (WhoseTurn == ActivePlayers[i])
                        {
                            WhoseTurn.PlayerState &= ~PlayerState.MyTurn;
                            WhoseTurn = ActivePlayers[i + 1];
                            WhoseTurn.PlayerState |= PlayerState.MyTurn;
                            break;
                        }
                    }
                }
               
            }
        }
    }
}