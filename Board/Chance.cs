namespace MonopolyTerminal;

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

                //AdvanceToNearestUtility(WhoseTurn);
                GoBackThreeSpaces(WhoseTurn);
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
                    List<Place> path = new List<Place>();

                    var i = WhoseTurn.GetCurrentOccupationByIndex();//mustn't and won't be 39 by any chance :)
                    
                    var distenation = 39;//Mayfair

                    for (int j = i + 1; j != distenation + 1; j = (j + 1) % 40)
                    {
                        path.Add(GetPlace(j));
                    }

                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() +" ");
                    }

                    
                    Thread.Sleep(500);
                    
                    Console.WriteLine("advance to 39");
                    player.Move(path.ToArray());
                }

                void TakeTripToMaryleboneStation(Player player)
                {
                    List<Place> path = new List<Place>();

                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    
                    var distenation = 15;// Marylebone station

                    for (int j = i + 1; j != distenation + 1; j = (j + 1) % 40)
                    {
                        path.Add(GetPlace(j));
                    }

                    
                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() +" ");
                    }

                    
                    Thread.Sleep(500);
                    
                    Console.WriteLine("advance to 15");
                    player.Move(path.ToArray());
                }

                void BuildingLoanMatures(Player player)
                {
                    player.AddMoney(200);
                    Console.WriteLine($"Collect $200 Building loan matures");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void AdvanceToTrafalgar(Player player)
                {
                    List<Place> path = new List<Place>();

                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    
                    var distenation = 24;//Trafalgar

                    for (int j = i + 1; j != distenation + 1; j = (j + 1) % 40)
                    {
                        path.Add(GetPlace(j));
                    }

                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() +" ");
                    }

                    
                    Thread.Sleep(500);
                    
                    Console.WriteLine("advance to 24");
                    player.Move(path.ToArray());
                    
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void AdvanceToNearestUtility(Player player)
                {
                    var i = WhoseTurn.GetCurrentOccupationByIndex();

                    List<Place> path = new List<Place>();

                    var electricity = 12;
                    var waterWorks = 28;

                    for (int j = i+1; j != electricity +1 & j != waterWorks +1; j = (j+1) %40)
                    {
                        path.Add(GetPlace(j));
                    }
                    
                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() +" ");
                    }
                    
                    Console.WriteLine("Advance to nearest utility");

                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }
                
                void IncomeTaxRefund(Player player)
                {
                    player.AddMoney(20);
                    Console.WriteLine($"Pay $20 Tax Refund");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void GotoJail(Player player)
                {
                    player.GoJail();
                    Console.WriteLine("Go directly to jail without collecting $200");
                }

                void AdvanceToGo(Player player)
                {
                    var i = WhoseTurn.GetCurrentOccupationByIndex();

                    List<Place> path = new List<Place>(_places).Where(place => place.GetIndex() > i).ToList();
                    path.Add(GetPlace(0));//go
                    
                    Thread.Sleep(500);
                    
                    Console.WriteLine("advance to go");
                    player.Move(path.ToArray());
                }
                
                void BankDividend(Player player)//collect
                {
                    player.AddMoney(50);
                    Console.WriteLine($"Bank pays you dividend of $50");
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
                    Console.WriteLine($"{player.GetName()} has got a free card");
                    Engine.OnLandingCompleted?.Invoke( this);
                }

                void AdvanceToPallMall(Player player)
                {
                    List<Place> path = new List<Place>();

                    var i = WhoseTurn.GetCurrentOccupationByIndex();
                    
                    var distenation = 11;

                    for (int j = i + 1; j != distenation + 1; j = (j + 1) % 40)
                    {
                        path.Add(GetPlace(j));
                    }

                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() +" ");
                    }

                    Thread.Sleep(500);
                    
                    Console.WriteLine("advance to 24");
                    player.Move(path.ToArray());
                }

                void ChairmanOfTheBoardPayment(Player player)
                {
                    //todo pay each player later...
                    //pay each player $50
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

                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() +" ");
                    }

                    Thread.Sleep(500);
                    
                    Console.WriteLine("Go back three spaces");
                    player.Move(path.ToArray());
                }

                void SpeedingFine(Player player)
                {
                    player.AddMoney(15);
                    Console.WriteLine($"$15 speeding fine");
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
                         j = (j+1) %40)
                    {
                        path.Add(GetPlace(j));
                    }
                    
                    foreach (var place in path)
                    {
                        Console.Write(place.GetIndex() + " ");
                    }
                    
                    Console.WriteLine("Advance to nearest station");

                    
                    Thread.Sleep(500);
                    
                    player.Move(path.ToArray());
                }
            }
        }
    }
}