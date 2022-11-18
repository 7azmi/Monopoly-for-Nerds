namespace MonopolyTerminal;

public partial class Monopoly
{
    public static partial class Board
    {
        public class Chest : Place
        {
            public Chest(int index) : base(index)
            {
                Name = "Chest";
            }

            public override void Land()
            {
                base.Land();

                Random rand = new Random();

                int randomCard = rand.Next(3, 18);

                var result = randomCard switch  
                {  
                    3 => (Action<Player>) GetJailFreeCard,
                    4 => (Action<Player>) PayDoctorFees,
                    5 => (Action<Player>) HolidayFund,
                    6 => (Action<Player>) LifeInsuranceMatures,
                    7 => (Action<Player>) SchoolFees,
                    8 => (Action<Player>) IncomeTaxRefund,
                    9 => (Action<Player>) HospitalFees,
                    10 => (Action<Player>)  GotoJail,
                    11 => (Action<Player>)  ConsultancyFee,
                    12 => (Action<Player>)  BirthDayGift,
                    13 => (Action<Player>)  Inheritance,
                    14 => (Action<Player>)  StockMarket,
                    15 => (Action<Player>)  Prize,
                    16 => (Action<Player>)  RepairService,
                    17 => (Action<Player>)  AdvanceToGo,
                    18 => (Action<Player>)  BankError,
                };  
                
                result.Invoke(WhoseTurn);
                

                void GetJailFreeCard(Player player)
                {
                    player.HasJailFreeCard = true;
                    Console.WriteLine($"{player.GetName()} has got a free card");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void PayDoctorFees(Player player)
                {
                    player.SpendMoney(50);
                    Console.WriteLine($"$50 Doctor fees");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void HolidayFund(Player player)
                {
                    player.AddMoney(100);
                    Console.WriteLine($"Collect $50 Holiday Fund Matures");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void LifeInsuranceMatures(Player player)
                {
                    player.AddMoney(100);
                    Console.WriteLine($"Collect $50 Holiday Fund Matures");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void SchoolFees(Player player)
                {
                    player.SpendMoney(50);
                    Console.WriteLine($"Pay $50 school fees");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void IncomeTaxRefund(Player player)
                {
                    player.AddMoney(20);
                    Console.WriteLine($"Pay $20 Tax Refund");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void HospitalFees(Player player)
                {
                    player.SpendMoney(100);
                    Console.WriteLine($"Pay $100 hospital fees");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void GotoJail(Player player)
                {
                    player.GoJail();
                    Console.WriteLine("Go directly to jail without collecting $200");
                }

                void ConsultancyFee(Player player)//collect
                {
                    player.AddMoney(25);
                    Console.WriteLine($"recieve $25 consultancy fee");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void BirthDayGift(Player player)
                {
                    //no need for the sake of skiping technical issues
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void Inheritance(Player player)
                {
                    player.AddMoney(100);
                    Console.WriteLine($"You inherit $100 from your grandma");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void StockMarket(Player player)
                {
                    player.AddMoney(50);
                    Console.WriteLine($"Collect $50 from the stock market");
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void Prize(Player player)
                {
                    player.AddMoney(10);
                    Console.WriteLine($"Collect $10 as a prize for some reason");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
                
                void RepairService(Player player)//the worse
                {
                    //todo repair service fee
                    Engine.OnLandingCompleted?.Invoke(this);
                }

                void AdvanceToGo(Player player)
                {
                    var i = WhoseTurn.GetCurrentOccupationByIndex();

                    List<Place> path = new List<Place>(_places).Where(place => place.GetIndex() > i).ToList();
                    path.Add(GetPlace(0));//go

                    foreach (var place in path)
                    {
                        //Console.Write(place.GetIndex() +" ");
                    }

                    
                    Thread.Sleep(500);
                    
                    Console.WriteLine("advance to go");
                    player.Move(path.ToArray());
                }

                void BankError(Player player)
                {
                    player.AddMoney(200);
                    Console.WriteLine($"Bank error in your favor. Collect $200");
                    Engine.OnLandingCompleted?.Invoke(this);
                }
            }
        }
    }
}