namespace Monopoly_for_Nerds;
using static Monopoly.Engine;
using static Monopoly.Board;
using static Monopoly;
using static Console;

public static class Printer
{
    private const int CellPadding = 7;

    public static void PrintBoard()
{

    string[] names =
    {
        "Go",
        "Old Ken",
        "Comm Ch",
        "white r",
        "Inc Tax",
        "King RR",
        "Angel I",
        "Chance",
        "Euston",
        "Penton",
        "Jail",
        "Pall Ml",
        "Elec",
        "Whitehl",
        "Nrth Av",
        "Mary St",
        "Bow St ",
        "Comm Ch",
        "Marl St",
        "Vine St",
        "Parking",
        "Strand",
        "Chance",
        "Fleet S",
        "Traf Sq",
        "Fench S",
        "Leic Sq",
        "Coventy",
        "Water",
        "Pcdly",
        "Bonk",
        "Reent",
        "Oxford",
        "Comm Ch",
        "J Bond",
        "Lvrpl S",
        "Chance",
        "Park L",
        "Spr Tax",
        "Mayfair"
    };

    #region Print Board

    SetWindowSize(140, 50);
    
    //Console.WriteLine("|iyfurtp|Strand |Chance |Fleet S|Traf Sq|Fench S|Leic Sq|Covent |Water  |Pcdly  |Bonk   |");
    for (int i = 20; i <= 30; i++)
    {
        Border();
        PlaceName(i);
    } 
    Border();

    var currentLeft = CursorLeft;
    var currentTop = CursorTop;

    OnInitialization += monopoly => PrintPlayersPanel(currentLeft, currentTop);
    OnNextPlayerTurn += player => PrintPlayersPanel(currentLeft, currentTop);
    OnBuyProperty += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    OnRentalPaid += (property, player, rental) => PrintPlayersPanel(currentLeft, currentTop);
    OnPlayerGetsOutOfJail += player => PrintPlayersPanel(currentLeft, currentTop);
    OnCloseAuction += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    //todo still have work to do here
    
    static void PrintPlayersPanel(int cursorLeft, int cursorTop)
    {
        var currentLeft = CursorLeft;
        var currentTop = CursorTop;
        
        
        for (int i = 0; i < ActivePlayers.Count; i++)
        {
            CursorLeft = cursorLeft + 1;
            CursorTop = cursorTop + i;
            
            var name = ActivePlayers[i].GetName();
            var money = ActivePlayers[i].GetMoney().ToString();
            var jailState = ActivePlayers[i].InJail ? "(In jail)" : "         ";
            var turnState = ActivePlayers[i].MyTurn ? "(His Turn)" : "          ";
            var playerLine = $"{name} '{name[0]}' ${money.PadRight(8)} {turnState} {jailState}";

            WriteLine(playerLine);
            WriteLine();
        }
        CursorLeft = currentLeft;
        CursorTop = currentTop;
    }
    
    WriteLine();

    //Console.WriteLine("|       |       |       |       |       |       |       |       |       |       |       |");
    for (int i = 20; i <= 30; i++)
    {
        Border();
        
        PrintCell(i);
    } 
    Border();
    WriteLine("");
    
    //Console.WriteLine("|20-----|21-----|22-----|23-----|24-----|25-----|26-----|27-----|28-----|29-----|30-----|");
    for (int i = 20; i <= 30; i++)
    {
        Border();
        PropertyInfo(i);
    } 
    Border();
    WriteLine();

    for (int left = 19, right =31; left >= 11 && right <= 39; left--, right++)//left(19, 11), right(31, 39)
    {
        //Console.WriteLine("|Vine St|                                                                       |Regent |");
        Border(); PlaceName(left); Border();
        Write("                                                                       ");//body
        Border(); PlaceName(right); Border();
        WriteLine();

        //Console.WriteLine("|       |                                                                       |       |");
        Border(); PrintCell(left); Border();
        Write("                                                                       ");//body
        Border(); PrintCell(right); Border();
        WriteLine();
    
        //Console.WriteLine("|19-----|                                                                       |31-----|");
        Border(); PropertyInfo(left); Border();
        Write("                                                                       ");//body
        Border(); PropertyInfo(right); Border();
        WriteLine();
    }

    //remove later
    #region Looped

    /*
    //Console.WriteLine("|Vine St|                                                                       |Regent |");
    Border(); PlaceName(19); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(31); Border();
    Console.WriteLine();

    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(19); Border();
    Console.Write("                                                                       ");
    Border(); Cell(31); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|19-----|                                                                       |31-----|");
    Border(); PropertyInfo(19); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(31); Border();
    Console.WriteLine();

    //Console.WriteLine("|Marl St|                                                                       |Oxford |");
    Border(); PlaceName(18); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(32); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(18); Border();
    Console.Write("                                                                       ");
    Border(); Cell(32); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|18-----|                                                                       |32-----|");
    Border(); PropertyInfo(18); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(32); Border();
    Console.WriteLine();

    //Console.WriteLine("|Comm Ch|                                                                       |Comm Ch|");
    Border(); PlaceName(17); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(33); Border();
    Console.WriteLine();

    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(17); Border();
    Console.Write("                                                                       ");
    Border(); Cell(33); Border();
    Console.WriteLine();


    //Console.WriteLine("|17-----|                                                                       |33-----|");
    Border(); PropertyInfo(17); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(33); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|Bow St.|                                                                       |J Bond |");
    Border(); PlaceName(16); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(34); Border();
    Console.WriteLine();

    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(16); Border();
    Console.Write("                                                                       ");
    Border(); Cell(34); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|16-----|                                                                       |34-----|");
    Border(); PropertyInfo(16); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(34); Border();
    Console.WriteLine();
    
    
    //Console.WriteLine("|Mary St|                                                                       |Lvrpl S|");
    Border(); PlaceName(15); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(35); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(15); Border();
    Console.Write("                                                                       ");
    Border(); Cell(35); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|15-----|                                                                       |35-----|");
    Border(); PropertyInfo(15); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(35); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|Nrth Av|                                                                       |Chance |");
    Border(); PlaceName(14); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(36); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(14); Border();
    Console.Write("                                                                       ");
    Border(); Cell(36); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|14-----|                                                                       |36-----|");
    Border(); PropertyInfo(14); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(36); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|Whitehl|                                                                       |Park Ln|");
    Border(); PlaceName(13); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(37); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(13); Border();
    Console.Write("                                                                       ");
    Border(); Cell(37); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|13-----|                                                                       |37-----|");
    Border(); PropertyInfo(13); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(37); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|Electrc|                                                                       |Spr Tax|");
    Border(); PlaceName(12); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(38); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(12); Border();
    Console.Write("                                                                       ");
    Border(); Cell(38); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|12-----|                                                                       |38-----|");
    Border(); PropertyInfo(12); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(38); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|Pall Ml|                                                                       |Mayfair|");
    Border(); PlaceName(11); Border();
    Console.Write("                                                                       ");
    Border(); PlaceName(39); Border();
    Console.WriteLine();
    
    //Console.WriteLine("|       |                                                                       |       |");
    Border(); Cell(11); Border();
    Console.Write("                                                                       ");
    Border(); Cell(39); Border();
    Console.WriteLine();

    //Console.WriteLine("|11-----|                                                                       |39-----|");
    Border(); PropertyInfo(11); Border();
    Console.Write("                                                                       ");
    Border(); PropertyInfo(39); Border();
    Console.WriteLine();
*/
    #endregion

    //Console.WriteLine("|Jail   |Penton |Euston |Chance |Angel I|King RR|Inc Tax|white r|Comm ch|Kent Rd|Go     |");
    for (int i = 10; i >= 0; i--)
    {
        Border();
        PlaceName(i);
    } 
    Border();
    WriteLine();
    
    //Console.WriteLine("|       |       |       |       |       |       |       |       |       |       |       |");
    for (int i = 10; i >= 0; i--)
    {
        Border();
        PrintCell(i);
    } 
    Border();
    WriteLine();
    
    //Console.WriteLine("|10-----|9------|8------|7------|6------|5------|4------|3------|2------|1------|0------|");
    for (int i = 10; i >= 0; i--)
    {
        Border();
        PropertyInfo(i);
    } 
    Border();
    WriteLine();
    
    //var currentLeft = Console.CursorLeft; //0
    var currentTop2 = CursorTop;

    OnNextPlayerTurn += player => Clear(currentTop2);

    void Clear(int cursorTop)
    {
        //var currentLeft = Console.CursorLeft;
        var currentTop = CursorTop;

        for (var i = cursorTop; i <= currentTop; i++)
        {
            SetCursorPosition(0, i);
            ClearCurrentConsoleLine();
        } 
        
        SetCursorPosition(0, cursorTop);

        void ClearCurrentConsoleLine()
        {
            var currentLineCursor = CursorTop;
            SetCursorPosition(0, CursorTop);
            Write(new string(' ', WindowWidth)); 
            SetCursorPosition(0, currentLineCursor);
        }
    }
    
    #endregion

    void Border()
    {
        ResetColor();
        Write("|");
    }

    void PlaceName(int index)
    {
        SetPropertyConsoleColor(index);
        Console.Write(names[index].PadRight(CellPadding));
        
        void SetPropertyConsoleColor(int index)
        {
            switch (index)
            {
                default:
                    Console.ResetColor();
                    break;
            
                case 1:
                case 3:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case 6:
                case 8:
                case 9:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;

                case 11:
                case 13:
                case 14:
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    
                    break;
                case 16:
                case 18:
                case 19:
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                
                case 21:
                case 23:
                case 24:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                
                case 26:
                case 27:
                case 29:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                
                case 31:
                case 32:
                case 34:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                
                case 37:
                case 39:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
            }
        }
    }

    void PropertyInfo(int index)
    {
        if(index <10)
            Write(index+"------");
        else
            Write(index+"-----");
    }
}

    private static void PrintCell(int index)
    {
        var currentLeft = CursorLeft;
        var currentTop = CursorTop;

        OnStart += () => PrintCell(index, currentLeft, currentTop);
        OnMovingOnPlace += (place, player) => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Jail)
            OnPlayerGetsOutOfJail += player => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Jail || GetPlace(index) is GotoJail)
            OnPlayerGetsToJail += player => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Property){}
        OnBuyProperty += (player, property) => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Property){}
        OnCloseAuction += (player, property) => PrintCell(index, currentLeft, currentTop);

        Write("       ");
        
        //PrintCell(index, currentLeft, currentTop);//there is no active players at this point
        //that's why we're involving OnStart upt here
    }

    private static void PrintCell(int index, int cursorLeft, int cursorTop)
    {
        CursorVisible = false;
        
        var currentLeft = CursorLeft;
        var currentTop = CursorTop;

        CursorLeft = cursorLeft;
        CursorTop = cursorTop;

        if (GetPlace(index) is Property property)
            if (property.HasOwner) BackgroundColor = property.GetOwner().GetLabel();

        PrintSettlers(GetPlace(index));
        
        ResetColor();
        
        CursorLeft = currentLeft;
        CursorTop = currentTop;
        
        CursorVisible = true;
    }
    
    private static void PrintSettlers(Place place)
    {
        var settlers =  place.GetSettlers();
            
        for (var i = 0; i < CellPadding; i++)
        {
            if (settlers.Length > i) PrintPlayerCharacter(settlers[i]);
            else Write(" ");
        }
        
        void PrintPlayerCharacter(Player player)
        {
            var currentForeground = ForegroundColor;//you always have to do this swap approach
            var currentBackground = BackgroundColor;
            
            ForegroundColor = player.GetLabel();
            BackgroundColor = player.InJail ? ConsoleColor.Gray : ConsoleColor.Black;
            
            Write(GetPlayerCharacter(player));
            
            ForegroundColor = currentForeground;
            BackgroundColor = currentBackground;
            
            char GetPlayerCharacter(Player player) => player.GetName()[0];
        }
    }
    

public static void Log(string log) => WriteLine(log);
}