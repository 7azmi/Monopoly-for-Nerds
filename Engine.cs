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
            OnMovingOnPlace += MovingOnPlace;
            OnRentalDue += PayRental;
            OnDiceRolled += times =>
            {
                if (times > 2)
                {
                    Human.Terminal.Log("Player rolled double thrice, go to jail you cheater!");
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
        }

        //actions
        //start
        public static Action<Monopoly> OnInitialization;
        public static Action OnStart;
        
        //public static Action<Player> OnPlayerTurn;
        public static Action<Player> OnPlayerTurnWhileInJail;
        public static Action<Player> OnPlayerDecidesToStayInJail;
        public static Action OnDiceReadyForRolling;
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
        public static Action<Property>  OnStartAuction;
        public static Action<Player, Property> OnCloseAuction;
        public static Action<Player, Property, int> OnBid;
        
        public static Action<Player, Property> OnPlayerMortgage;
        public static Action<Player, Property> OnPlayerUnmortgage;
        public static Action<Player, Street> OnPlayerBuyHouse;
        public static Action<Player, Street> OnPlayerSellHouse;
        
        public static Action<Player> OnPlayerInDebt;
        
        
        public static Action<Player, Player, Player.SetOffer.Offer> OnOffering;
        public static Action<Player, Player, Player.SetOffer.Offer> OnAcceptOffer;
        public static Action<Player, Player> OnDeclineOffer;
        
        public static Action OnRollAgain;
        public static Action OnTurnCompleted;
        public static Action<Player> OnPlayerTurn;

        public static Action<Player> OnDeclareBankruptcy;
        public static Action<Player> OnDeclareWinner;


        public static void Init(Monopoly monopoly)
        {
            //OnStartAuction += property => StartAuction(property);
            OnInitialization?.Invoke(monopoly);
        }

        public static void LateInit()
        {
            OnLandingCompleted += _ =>  RollAgainOrFinishTurn();
            OnBuyProperty += _ => RollAgainOrFinishTurn();
            OnRentalPaid += (_, _, _) =>  RollAgainOrFinishTurn();
            OnLandingOnMyProperty += (_, _) => RollAgainOrFinishTurn();
            OnPlayerDecidesToStayInJail += _ => RollAgainOrFinishTurn();
            OnCloseAuction += (_, _) => RollAgainOrFinishTurn();
            OnAcceptOffer += (_, _, _) => RollAgainOrFinishTurn();
            OnDeclineOffer += (_, _) => RollAgainOrFinishTurn();
            OnDeclareWinner += DeclareWinner;
            OnDiceReadyForRolling += () => WhoseTurn.State |= PlayerState.ReadyForRolling;
        }

        private static void RollAgainOrFinishTurn()
        {
            Thread.Sleep(500);
            if (!WhoseTurn.InJail && PlayerCanRollAgain())
            {
                OnDiceReadyForRolling.Invoke();
            }
            else
            {
                if(!WhoseTurn.HasEnoughMoney(0)) OnPlayerInDebt.Invoke(WhoseTurn);
                else OnTurnCompleted.Invoke();
            }
        }

        private static void DeclareWinner(Player player)
        {
            Human.Terminal.Log($"{player.GetName()} ruthlessly smashed all his opponents and became the boss capital! ");
            Human.Terminal.Log($"No pity for losers. lol ");
            Thread.Sleep(5000);
            //Human.Terminal.Log($"re open  ");
        }
        
        private static async void StartAuction(Property property)
        {

        }

        public static void Start()
        {
            foreach (var p in ActivePlayers)
                p.SetStartingOccupation(GameSettings.StartingPoint);

            OnStart?.Invoke();//don't tell me why

            OnDiceReadyForRolling?.Invoke(); //SetDiceState(DiceState.ReadyForRolling);
        }
        
        private static void MovingOnPlace(Place place, Player player)
        {
            if (place is Board.Go go)
            {
                player.GetSalary(200);
            }
        }

        private static void PayRental(Property property, Player victim, int rental)
        {
            Human.Terminal.Log($"you landed on {property.GetName()}");
            new Player.PayRent(victim, property).Execute();
        }
        
        public static void SwitchNextPlayerTurn()
        {
            ResetDoubleCounter();
            
            NextPlayer();
            
            OnPlayerTurn?.Invoke(WhoseTurn);
            if (WhoseTurn.InJail) OnPlayerTurnWhileInJail?.Invoke(WhoseTurn);//window Jail.AskPlayerToGetOutOrStayInJail(); //window
            else OnDiceReadyForRolling?.Invoke(); //SetDiceState(DiceState.ReadyForRolling);
            
            void NextPlayer()
            {
                if (WhoseTurn == ActivePlayers.Last())
                {
                    WhoseTurn.State &= ~PlayerState.MyTurn;
                    WhoseTurn = ActivePlayers.First();
                    WhoseTurn.State |= PlayerState.MyTurn; 
                }
                else
                {
                    for (var i = 0; i < ActivePlayers.Count-1; i++)
                    {
                        if (WhoseTurn == ActivePlayers[i])
                        {
                            WhoseTurn.State &= ~PlayerState.MyTurn;
                            WhoseTurn = ActivePlayers[i + 1];
                            WhoseTurn.State |= PlayerState.MyTurn;
                            break;
                        }
                    }
                }
            }
        }
    }
}