using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using MonopolyTerminal;
using MonopolyTerminal.Enums;
using MonopolyTerminal.Helpers;
using MonopolyTerminal.Human;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Engine;
using static MonopolyTerminal.Monopoly.Board;
using static System.Console;
using static MonopolyTerminal.Printer;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.Monopoly.Auction;

//Console.WriteLine((1 + 39) % 40);
PrintBoard();

LateInit();

ConnectGameLogicWithCommands();//terminal thing

var m = new Monopoly(new Terminal(), new GameSettings(
    new []
    {
        new Player("nGAGEOnline", ConsoleColor.Red, 1500, new Shooter()),
        new Player("HuHu", ConsoleColor.Yellow, 1500, new Shooter()),
        new Player("Shooter", ConsoleColor.Blue, 1500, new Shooter()),
        //new Player("Ahmed", ConsoleColor.Cyan, 1500, true),
        //new Monopoly.Player("Ahmed", ConsoleColor.Green, 1500, true)
    }, startingPoint: 15));

MonopolyTerminal.Human.Terminal.Log("Finished");
CursorVisible = true;
ReadKey();
ReadKey();


//////////////////////////////////////////////////////////////////////////////////////////

void ConnectGameLogicWithCommands()
{
    OnPlayerTurn += player => player.GetInput().OnTurn();
    OnDiceReadyForRolling += () => WhoseTurn.GetInput().OnDiceReady();
    OnPlayerTurnWhileInJail += (_) => WhoseTurn.GetInput().OnInJail();
    OnPlayerInDebt += (player) => WhoseTurn.GetInput().OnDoesNotHaveEnoughMoney();
    OnLandingOnUnownedProperty += (property) => WhoseTurn.GetInput().OnBuyOrBid(property);
    OnTurnCompleted += () => WhoseTurn.GetInput().OnTurnCompleted();
    OnOffering += (offeror, offeree, offer) => offeree.GetInput().OnReceiveDeal(offeror, offeree, offer);
}

    #region Extract functions later

    //     void AskPlayerToGetOutOrStay(Player prisoner)
//     {
//         //i.input.OnTurn.Invoke();
//         Log("you are in jail, would you like to get 'out' for $50, or 'stay' this time?");
//
//         string line;
//
//         tryAgain:
//         
//         line = !prisoner.IsBot ? ReadLine() : prisoner.GetInput().GetOutOfJailOrStay(prisoner);
//
//         if (line.Contains("out"))
//         {
//             var getOut = new Player.GetOutOfJail(prisoner);
//             if(!getOut.IsLegal()) goto tryAgain;
//             getOut.Execute(); 
//         }
//         else if (line.Contains("stay"))
//         {
//             var stay = new Player.StayInJail(prisoner);
//             if (!stay.IsLegal()) goto tryAgain;
//             stay.Execute(); 
//         } 
//         else goto tryAgain;
//     }
//     void AskPlayerToRoll(int times, Player whoseTurn)
//     {
//         Log("'roll' the dice");
//
//         string line;
//         do line = !whoseTurn.IsBot ? ReadLine() :whoseTurn.GetInput().RollDice();
//         while (!line.Contains("roll"));
//
//         new Player.RollDice(whoseTurn).Execute();
//     }
//     //process here is duplicated but it's fine for the sake of not repeating logs
//     void AskPlayerToDivestForDuePaymentOrDeclareBankruptcy(Player victim)
//     {
//         string line;
//
//         Log("'mortgage ##' or 'sell house ##' To proceed, or you can simply 'declare bankruptcy' :')");
//
//         tryAgain:
//         
//         line = !victim.IsBot ? ReadLine() : victim.GetInput().DivestOrDeclareBankruptcy();
//         if (line.Contains("declare bankruptcy")) 
//             new Player.DeclareBankruptcy(victim).Execute();
//         else if(TryExecuteDivestPropertyCommand(victim, line) && victim.HasEnoughMoney(amount)) 
//             new Player.PayRent(victim, property).Execute();
//         else 
//             goto tryAgain;
//
//     }
//     
//     void AskPlayerToBuyPropertyOrPutItOnAuction(Player player, Property property)
//     {
//         Log($"you landed on unowned property. you have to either 'buy' {property.GetName()} for {property.GetPrice()} or 'bid' for auction");
//         
//         string line;
//
//         tryAgain:
//         line = !player.IsBot ? ReadLine() : player.GetInput().BuyOrBid(player, property);
//
//         if (line.Contains("buy"))
//         {
//             var buyProperty = new Player.BuyProperty(player, property);
//             if (!buyProperty.IsLegal()) goto tryAgain;
//             buyProperty.Execute();
//         } 
//         else if (line.Contains("bid"))
//         {
//             new Player.OpenAuction(property).Execute();
//         } 
//         else goto tryAgain;
//     }
//     void AskPlayersToBid(Property property)//roll
//     {
//         Log($"bid '##' for {property.GetName()}, or 'fold'");
//
//         List<Player> bidders  = new List<Player>(ActivePlayers);
//         bidders.Reverse();//for a reversed for loop
//         
//         string line;
//         while(true)//I can't read it again but it functions as I remember.
//         {
//             if (bidders.Count == 1) break;
//             
//             for (var i = bidders.Count - 1; i >= 0; i--)
//             {
//                 if (bidders.Count == 1 && MostBidder != null) break;
//                 tryAgain:
//                 
//                 Write($"{bidders[i].GetName()}: ");
//                 line = !bidders[i].IsBot ? ReadLine() : bidders[i].GetInput().BidOrFold(bidders[i], MostBid);
//
//                 if (line.Contains("fold"))
//                     bidders.RemoveAt(i);
//                 else if (!TryExecuteBidCommand(bidders[i], line)) goto tryAgain;
//             }
//         }
//         if(bidders.IsNullOrEmpty()) Bank.CloseAuction(null, property);
//         else bidders.First().CloseAuction(property);
//     }
//     void AskPlayerToProceedPlaying()//todo refactor this 
//     {
//         var player = WhoseTurn;
//         
//         if(!player.HasEnoughMoney(0))
//         {
//             var debt = (int) MathF.Abs(player.GetMoney());
//             Log($"You need to normalize your balance: ${debt} before proceeding");
//             OnPlayerInDebt.Invoke(player, debt);//nightmare
//         }
//         
//         //roll again or end turn, you can also manage your properties and set deals
//         if (!player.InJail && Dice.PlayerCanRollAgain())//bad logic but seems to work for now
//         {
//             string line;
//             Log($"you got double {(Dice.GetDoubles() == 2 ? "twice": "")}, 'roll' again");
//             tryAgain1:
//
//             do line = !player.IsBot ? ReadLine() : player.GetInput().RollAgain(Dice.GetDoubles());
//             while (!line.Contains("roll"));
//             new Player.RollDice(player).Execute();
//         }
//         else
//         {
//             string line;
//             Log("You may manage your properties, set an 'offer', or 'end' your turn");
//             tryAgain2:
//             line = !player.IsBot ? ReadLine() : player.GetInput().EndTurnOrManagePropertiesOrSetOffer(player);
//
//             if (line.Contains("end")) new Player.EndTurn(player).Execute();
//             else if (line.Contains("offer"))//todo can be refactored
//             {
//                 TryExecuteSetOfferCommand(WhoseTurn, line);
//                 goto tryAgain2;
//             }
//             else
//             {
//                 TryExecuteManagePropertyCommand(player, line);
//                 goto tryAgain2;
//             }
//         }
//     }
//     void AskPlayerToAcceptOrDeclineOffer(Player offeror, Player offeree, Player.SetOffer.Offer offer)
//     {
//         string line;
//         Log(offeror.GetName() + "offers you:");
//         Log(offer.offerInfo());
//         Log("you should either 'accept' or 'decline' this deal");
//         tryAgain:
//         
//         line = !offeree.IsBot ? ReadLine() : offeree.GetInput().AcceptOrRefuseOffer(offeror, offeree, offer);
//
//         if(line.Contains("accept")) new Player.AcceptOffer(offeree, offeror, offer).Execute();
//         else if(line.Contains("decline"))new Player.DeclineOffer(offeree, offeror).Execute();
//         else goto tryAgain;
//     }
// }
//
// bool TryExecuteSetOfferCommand(Player player, string line)//offer 12 and 39 and $90 for 5 and 25 and 19 
// {
//     var isLegalCommand = TryGetSetOfferCommand(player, line, out var command);
//     if (isLegalCommand) command.Execute();
//
//     WriteLine("legal? " + isLegalCommand);
//     return isLegalCommand;
//
//     bool TryGetSetOfferCommand(Player whoseTurn, string line, out Command command)
//     {
//         command = null;
//         
//         
//         if(!line.Contains("for")) {WriteLine("you must include 'for' to differentiate between deal sides"); return false;}
//
//         var commandParts = line.Split(new string[] {"for"}, 2, StringSplitOptions.None);
//
//         var isTHereOfferedMoney = TryExtractMoneyDigits(commandParts[0], out var offeredMoney);
//         var isThereRequestedMoney = TryExtractMoneyDigits(commandParts[1], out var requestedMoney);
//         var isThereOfferedProperties = TryExtractProperties(commandParts[0], out var offeredProperties);
//         var isThereRequestedProperties = TryExtractProperties(commandParts[1], out var requestedProperties);
//         var isThereOpponentName = TryExtractOpponentName(line, out var player);
//
//         WriteLine("offeree: "+isThereOpponentName+" " +(player !=null? player.GetName():""));
//         WriteLine("money offered: " + isTHereOfferedMoney + " " + offeredMoney);
//         WriteLine("money requested: " +isThereRequestedMoney+" " + requestedMoney);
//         Write("properties offered: " +isThereOfferedProperties + " ");
//
//         foreach (var p in offeredProperties)
//             Write(p.GetName() + " ");
//         WriteLine();
//         
//         Write("properties requested: " +isThereRequestedProperties+ " ");
//         foreach (var p in requestedProperties)
//             Write(p.GetName() + " ");
//         WriteLine();
//         
//         
//         command = new Player.SetOffer(player, new SetOffer.Offer(offeredProperties, requestedProperties, offeredMoney, requestedMoney));
//
//         return command.IsLegal();
//         
//
//         bool TryExtractMoneyDigits(string line, out int money)
//         {
//             money = 0;  if (!line.Contains('$')) return false;
//
//             var dollarIndex = line.IndexOf('$');
//
//             var amount = "";
//             while (true)
//             {
//                 if (++dollarIndex > line.Length-1 || !char.IsDigit(line[dollarIndex])) break;
//                 amount += line[dollarIndex];
//             }
//
//             money = int.Parse(amount != ""? amount:"0");
//             
//             return money > 0;
//         }
//
//         bool TryExtractProperties(string line, out Property[] properties)
//         {
//             var propertiesIndexes = line.Split(' ').ToList();
//
//             propertiesIndexes.RemoveAll(s => s.Any(c => !char.IsDigit(c)));
//             propertiesIndexes = propertiesIndexes.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
//
//             List<Property> propertyList = new List<Property>();
//             
//             foreach (var index in propertiesIndexes)
//                 if(TryToGetProperty(int.Parse(index), out Property p)) propertyList.Add(p);
//
//
//                 properties = propertyList.ToArray();
//                 
//                 return !(properties == null ||properties.Length ==0);
//         }
//
//         bool TryExtractOpponentName(string line, out Player player)
//         {
//             player = null;
//             var regex = new Regex(" ([A-Za-z]) ");//" D " 
//
//             var match = regex.Match(line);
//             if (match.Success) player = GetPlayer(match.Value[1]);
//             
//             WriteLine(match.Value);
//             
//                 return player != null;
//                 
//                 Player GetPlayer(char letter) =>
//                 ActivePlayers.FirstOrDefault(p => p.GetName()[0] == letter && p != whoseTurn);
//         }
//     }
// }
// bool TryExecuteBidCommand(Player player, string line)
// {
//     var isLegalCommand = TryGetBidCommand(player, line, out var command);
//     if (isLegalCommand) command.Execute();
//
//     return isLegalCommand;
//
//     bool TryGetBidCommand(Player bidder, string line, out Command command)
//     {
//         command = null;
//         
//         if (!TryToGetDigits(line, out var bid)) return false;
//
//         command = new Bid(bidder, bid);
//
//         return command != null ? command.IsLegal() : false;
//     }
// }

    #endregion