namespace Monopoly;

public partial class Monopoly
{
    public static partial class Board
    {
        public class Chance : Place
        {
            public Chance(int index) : base(index)
            {
                Name = "Chance";
            }

            public override void Land()
            {
                base.Land();

                Random rand = new Random();

                AdvanceToMayfair(WhoseTurn);
                //(WhoseTurn);
                return;
                
                int randomCard = rand.Next(3, 18);

                var result = randomCard switch  
                {  
                    3 => (Action<Player>) AdvanceToMayfair,
                    4 => (Action<Player>) TakeTripToMaryleboneStation,
                    5 => (Action<Player>) BuildingLoanMatures,
                    6 => (Action<Player>) AdvanceToTrafalgar,
                    7 => (Action<Player>) AdvanceToNearestUtility,
                    8 => (Action<Player>) GotoJail,
                    9 => (Action<Player>) AdvanceToGo,
                    10 => (Action<Player>) BankDividend,
                    11 => (Action<Player>) MakeGeneralRepairOnAllYourProperty,
                    12 => (Action<Player>) GetOutOfJailFreeCard,
                    13 => (Action<Player>) AdvanceToPallMall,
                    14 => (Action<Player>) ChairmanOfTheBoardPayment,
                    15 => (Action<Player>) GoBackThreeSpaces,
                    16 => (Action<Player>) SpeedingFine,
                    17 => (Action<Player>) AdvanceToNearestStation,
                    18 => (Action<Player>) AdvanceToNearestStation,
                };  
                
                result.Invoke(WhoseTurn);
                
                void AdvanceToMayfair(Player player)
                {
                    var path = new List<Place>();

                    Log("advance to 39");
                    var destination = 39;//Mayfair
                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    
                    var steps = (destination - i + 1 + 40) % 40;

                    for (var j = 1; j < steps; j++)
                    {
                        path.Add(GetPlace((i + j) % 40));
                    }


                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);

                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }

                void TakeTripToMaryleboneStation(Player player)
                {
                    List<Place> path = new List<Place>();

                    Log("advance to 15");
                    
                    var destination = 15;//Mayfair
                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    var steps = (destination - i + 1 + 40) % 40;

                    for (int j = i + 1; j < steps; j++)
                    {
                        path.Add(GetPlace(j % 40));
                    }
                    
                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);

                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }

                void BuildingLoanMatures(Player player)
                {
                    player.AddMoney(200);
                    Log($"Collect $200 Building loan matures");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void AdvanceToTrafalgar(Player player)
                {
                    List<Place> path = new List<Place>();

                    Log("advance to 24");
                    
                    var destination = 15;//Mayfair
                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    var steps = (destination - i + 1 + 40) % 40;

                    for (int j = i + 1; j < steps; j++)
                    {
                        path.Add(GetPlace(j % 40));
                    }

                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);
                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                    
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void AdvanceToNearestUtility(Player player)
                {
                    var i = WhoseTurn.GetCurrentOccupationByIndex();

                    List<Place> path = new List<Place>();

                    var electricity = 12;
                    var waterWorks = 28;

                    Log("Advance to nearest utility");
                    for (int j = i+1; j != electricity +1 & j != waterWorks +1; j++)
                    {
                        path.Add(GetPlace(j % 40));
                    }
                    
                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);
                    

                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }
                
                void IncomeTaxRefund(Player player)
                {
                    player.AddMoney(20);
                    Log($"Pay $20 Tax Refund");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void GotoJail(Player player)
                {
                    Log("Go directly to jail without collecting $200");
                    player.GoJail();
                }

                void AdvanceToGo(Player player)
                {
                    var i = WhoseTurn.GetCurrentOccupationByIndex();

                    Log("advance to go");
                    List<Place> path = new List<Place>(_places).Where(place => place.GetIndex() > i).ToList();
                    path.Add(GetPlace(0));//go
                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }
                
                void BankDividend(Player player)//collect
                {
                    player.AddMoney(50);
                    Log($"Bank pays you dividend of $50");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void MakeGeneralRepairOnAllYourProperty(Player player)
                {
                    //todo repair property 2
                    //25 for each house
                    //100 for each hotel
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void GetOutOfJailFreeCard(Player player)
                {
                    player.HasJailFreeCard = true;
                    Log($"{player.GetName()} has got a free card");
                    Engine.OnLandingCompleted?.Invoke( this);
                }

                void AdvanceToPallMall(Player player)
                {
                    List<Place> path = new List<Place>();

                    Log("advance to 11");
                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    var destination = 11;
                    var steps = (destination - i + 1 + 40) % 40;

                    for (int j = i + 1; j < steps; j++)
                    {
                        path.Add(GetPlace(j % 40));
                    }

                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);

                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }

                void ChairmanOfTheBoardPayment(Player player)
                {
                    Log("laaay laay laay laay");
                    //todo pay each player later...
                    //pay each player $50
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void GoBackThreeSpaces(Player player)//the worse
                {
                    List<Place> path = new List<Place>();

                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    
                    var distenation = i - 3;

                    for (int j = i - 1; j != distenation - 1; j--)
                    {
                        path.Add(GetPlace(j));
                    }

                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);

                    Thread.Sleep(500);
                    
                    Log("Go back three spaces");
                    player.Move(path.ToArray());
                }

                void SpeedingFine(Player player)
                {
                    player.AddMoney(15);
                    Log($"$15 speeding fine");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void AdvanceToNearestStation(Player player)
                {
                    var i = WhoseTurn.GetCurrentOccupationByIndex();

                    List<Place> path = new List<Place>();

                    var station1 = 5;
                    var station2 = 15;
                    var station3 = 25;
                    var station4 = 35;

                    for (int j = i+1;
                         j != station1 +1 & j != station2 +1 & j != station3 +1 & j != station4 +1;
                         j++)
                    {
                        path.Add(GetPlace(j % 40));
                    }

                    var road = "";
                    foreach (var place in path)
                    {
                        road += place.GetIndex() + " ";
                    }
                    
                    Log(road);
                    
                    Log("Advance to nearest station");
                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }
            }

            void Log(string line)
            {
                Platform.Log(line);
            }
        }
    }
}