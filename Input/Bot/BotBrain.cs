using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.TypingSimulator;
namespace MonopolyTerminal;

public partial class Monopoly
{
    public abstract class BotBrain
    {
        protected readonly string STAYINJAIL = "stay"; 
        protected readonly string GETOUTOFJAIL = "out"; 
        protected readonly string ROLLDICE = "roll"; 
        protected readonly string PAYRENT = "pay";
        protected readonly string FOLD = "fold";
        //todo ....
        public abstract string OnBotTurn();
        public abstract string OnBotInJail();
        public abstract string OnBotReceivesDeal();
        public abstract string OnBotDoesNotHaveEnoughMoney();
        public abstract string Declare();
        public abstract string Quit(); //bot never quits


        public abstract string GetOutOfJailOrStay(Player bot);

        public abstract string RollDice();


        public string DivestOrDeclareBankruptcy()
        {
            throw new NotImplementedException();
        }

        public string PayRent() => TypeOutText(PAYRENT);//what else?

        public abstract string BuyOrBid(Player player, Board.Property property);

        public abstract string BidOrFold(Player bidder, int mostBid);

        public abstract string RollAgain(int getDoubles);

        public string EndTurnOrManagePropertiesOrSetOffer(Player player)
        {
            return TypeOutText("end");
        }

        public string AcceptOrRefuseOffer(Player offeror, Player offeree, Player.SetOffer.Offer offer)
        {
            //todo oh shit
            return TypeOutText("decline");
        }
    }

    public class DumbBot : BotBrain
    {
        public override string OnBotTurn()
        {
            return "";
        }

        public override string OnBotInJail()
        {
            throw new NotImplementedException();
        }

        public override string OnBotReceivesDeal()
        {
            throw new NotImplementedException();
        }

        public override string OnBotDoesNotHaveEnoughMoney()
        {
            throw new NotImplementedException();
        }

        public override string Declare()
        {
            throw new NotImplementedException();
        }

        public override string Quit()
        {
            //I can't use that
            return "";
        }

        //Randomly decides whether to get 'out' or 'stay' if there is money
        public override string GetOutOfJailOrStay(Player bot)
        {
            string command;
            
            if (bot.HasEnoughMoney(50)) command = new Random().Next(2) == 1 ? GETOUTOFJAIL : STAYINJAIL;
            else command = STAYINJAIL;
            
            return TypeOutText(command);
        }

        //roll every time
        public override string RollDice()
        {
            return TypeOutText(ROLLDICE);
        }

        public override string BuyOrBid(Player player, Property property)
        {
            return "buy";
        }

        public override string BidOrFold(Player bidder, int mostBid)
        {
            var property = WhoseTurn.GetCurrentOccupation() as Board.Property;//must be
            var newBid = mostBid + new Random().Next(5, 75);
            
            if (mostBid >= property.GetPrice() || !bidder.HasEnoughMoney(newBid))
                return TypeOutText(FOLD);

            return TypeOutText(newBid.ToString());
        }

        public override string RollAgain(int getDoubles) => TypeOutText(ROLLDICE);
    }
}