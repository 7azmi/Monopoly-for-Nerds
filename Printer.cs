﻿namespace Monopoly;
using static Monopoly.Engine;
using static Monopoly.Board;
using static Monopoly;
using static Console;
using static Human.Terminal;

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

    Cards.Init();
    Terminal.ConsoleRenderer.InitDiceRenderer();
    
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
    OnPlayerTurn += player => PrintPlayersPanel(currentLeft, currentTop);
    OnBuyProperty += (property) => PrintPlayersPanel(currentLeft, currentTop);
    OnRentalPaid += (property, player, rental) => PrintPlayersPanel(currentLeft, currentTop);
    OnPlayerGetsOutOfJail += player => PrintPlayersPanel(currentLeft, currentTop);
    OnCloseAuction += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    OnPlayerMortgage += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    OnPlayerUnmortgage += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    OnPlayerBuyHouse += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    OnPlayerSellHouse += (player, property) => PrintPlayersPanel(currentLeft, currentTop);
    OnAcceptOffer += (_, _, _) => PrintPlayersPanel(currentLeft, currentTop);
    OnDeclareBankruptcy += _ => PrintPlayersPanel(currentLeft, currentTop);
    //todo still have work to do here
    
    static void PrintPlayersPanel(int cursorLeft, int cursorTop)
    {
        //Console.BackgroundColor = ConsoleColor.Black;
        var currentLeft = CursorLeft;
        var currentTop = CursorTop;
        var playersLength = Monopoly.GameSettings.Players.Length;

        for (var i = 0; i < AllPlayers.Length; i++)
        {
            CursorLeft = cursorLeft + 1;
            CursorTop = cursorTop + i;
            var name = AllPlayers[i].GetName();
            var money = AllPlayers[i].GetMoney().ToString();
            var jailState = AllPlayers[i].InJail ? "(In jail)" : "         ";
            var turnState = AllPlayers[i].MyTurn ? "(His Turn)" : "          ";
            var state = AllPlayers[i].IsBroke ? "*Broke*    " : turnState;
            var playerLine = $"{name} '{name[0]}' ${money.PadRight(8)} {state} {jailState}";

            PrintLine(playerLine);
            PrintLine("");
            
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
    
    Cards.GetCompanyCardInfo(GetPlace(12) as Company);
    
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

    //Log("|11-----|                                                                       |39-----|");
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

    /*
    CommandBarPosition = (ttt.Item1, ttt.Item2);
    CreateCommandBar();
    
    void CreateCommandBar()
    {
        CommandBarLogger();
    }*/
    
    StreamLinePosition = (CursorLeft, CursorTop);
    (int, int) tempLine = StreamLinePosition;

    //var topG = CursorTop;
    OnPlayerTurn += player => Clear(tempLine.Item2);

    Terminal.ConsoleRenderer.RenderDoge();
    
    void Clear(int cursorTop)
    {
        //var currentLeft = Console.CursorLeft;
        var currentTop = StreamLinePosition.Item2;

        for (var i = cursorTop; i <= currentTop; i++)
        {
            SetCursorPosition(0, i);
            ClearCurrentConsoleLine();
        }

        StreamLinePosition = (0, cursorTop);
        SetCursorPosition(CommandBarPosition.Item1, CommandBarPosition.Item2);

        void ClearCurrentConsoleLine()
        {
            var currentLineCursor = CursorTop;
            SetCursorPosition(0, CursorTop);
            PrintLine(new string(' ', WindowWidth)); 
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
        Write(names[index].PadRight(CellPadding));
        
        void SetPropertyConsoleColor(int index)
        {
            switch (index)
            {
                default:
                    ResetColor();
                    break;
            
                case 1:
                case 3:
                    BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case 6:
                case 8:
                case 9:
                    BackgroundColor = ConsoleColor.DarkCyan;
                    break;

                case 11:
                case 13:
                case 14:
                    BackgroundColor = ConsoleColor.DarkMagenta;
                    
                    break;
                case 16:
                case 18:
                case 19:
                    BackgroundColor = ConsoleColor.White;
                    break;
                
                case 21:
                case 23:
                case 24:
                    BackgroundColor = ConsoleColor.DarkRed;
                    break;
                
                case 26:
                case 27:
                case 29:
                    BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                
                case 31:
                case 32:
                case 34:
                    BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                
                case 37:
                case 39:
                    BackgroundColor = ConsoleColor.DarkBlue;
                    break;
            }
        }
    }

    void PropertyInfo(int index)//todo requires refactoring
    {
        
        
        var cell = index.ToString().PadRight(CellPadding-1);
        
        for (int i = 0; i < CellPadding-1; i++)
        {
            Write(cell[i] ==' ' ? "-": cell[i]);
        }

        var currentLeft = CursorLeft;
        var currentTop = CursorTop;
        
        if (GetPlace(index) is Property) OnDeclareBankruptcy += _ => WritePropertyState(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Property) OnPlayerMortgage += (player, property) => WritePropertyState(index, currentLeft, currentTop);
        if (GetPlace(index) is Property) OnPlayerUnmortgage += (player, property) => WritePropertyState(index, currentLeft, currentTop);
        if (GetPlace(index) is Street) OnPlayerBuyHouse += (player, street) => WritePropertyState(index, currentLeft, currentTop);
        if (GetPlace(index) is Street) OnPlayerSellHouse += (player, street) => WritePropertyState(index, currentLeft, currentTop);
        
        
        Write("-");
        
        void WriteHousesNumber(int index, int cursorLeft, int cursorTop)
        {
            var currentLeft = CursorLeft;
            var currentTop = CursorTop;

            CursorLeft = cursorLeft;
            CursorTop = cursorTop;
            
            
            
            var street = GetPlace(index) as Street;
            if (street.HasHouses)
            {
                var currentColor = BackgroundColor;
                BackgroundColor = ConsoleColor.DarkGreen;
                Write(street.HouseCount);
                BackgroundColor = currentColor;
            } else Write("-");
            
            CursorLeft = currentLeft;
            CursorTop = currentTop;
        }

        void WritePropertyState(int index, int cursorLeft, int cursorTop)
        {
            var currentLeft = CursorLeft;
            var currentTop = CursorTop;

            CursorLeft = cursorLeft;
            CursorTop = cursorTop;

            var property = GetPlace(index) as Property;

            if (property is Street street)
            {
                if (street.HasHouses)
                {
                    var currentColor = BackgroundColor;
                    BackgroundColor = ConsoleColor.DarkGreen;
                    Print(street.HouseCount.ToString());
                    BackgroundColor = currentColor;
                } 
                else if (street.IsMortgaged())
                {
                    var currentColor = BackgroundColor;
                    BackgroundColor = ConsoleColor.DarkRed;
                    Print("M");
                    BackgroundColor = currentColor;
                } else Print("-");
            }
            else if (property.IsMortgaged())
            {
                var currentColor = BackgroundColor;
                BackgroundColor = ConsoleColor.DarkRed;
                Print("M");
                BackgroundColor = currentColor;
            }else Print("-");
            
            CursorLeft = currentLeft;
            CursorTop = currentTop;
        }
    }
}

    private static (int, int)[] _cellsPos = new (int, int)[40];
    
    
    private static void PrintCell(int index)
    {
        /*
        var currentLeft = CursorLeft;
        var currentTop = CursorTop;
        */
        _cellsPos[index].Item1 = CursorLeft;
        _cellsPos[index].Item2 = CursorTop;

        //you maaaay refactor this
        OnAcceptOffer += (_, _, _) => PrintCell(index, _cellsPos[index].Item1, _cellsPos[index].Item2); 
        OnDeclareBankruptcy += _ => PrintCell(index, _cellsPos[index].Item1, _cellsPos[index].Item2);
        
        if (index == 39)//last one
        {
            OnStart += () =>
            {
                var sp = Monopoly.GameSettings.StartingPoint;
                PrintCell(sp, _cellsPos[sp].Item1, _cellsPos[sp].Item2);
            };
            
            OnMovingOnPlace += (place, player) =>
            {
                var j = (place.GetIndex() + 39) % 40;
                var i = place.GetIndex();
                var k = (place.GetIndex() + 1) % 40;
                PrintCell(j, _cellsPos[j].Item1, _cellsPos[j].Item2);
                PrintCell(i, _cellsPos[i].Item1, _cellsPos[i].Item2);
                PrintCell(k, _cellsPos[k].Item1, _cellsPos[k].Item2);
            };
            
            OnPlayerGetsToJail += player => PrintCell(10, _cellsPos[10].Item1, _cellsPos[10].Item2);
            OnPlayerGetsToJail += player => PrintCell(30, _cellsPos[30].Item1, _cellsPos[30].Item2);
            OnPlayerGetsOutOfJail += player => PrintCell(10, _cellsPos[10].Item1, _cellsPos[10].Item2);


            OnBuyProperty += (property) => PrintCell(property.GetIndex(), _cellsPos[property.GetIndex()].Item1,
                _cellsPos[property.GetIndex()].Item2);
            
            OnCloseAuction += (player, property) => PrintCell(property.GetIndex(), _cellsPos[property.GetIndex()].Item1,
                _cellsPos[property.GetIndex()].Item2);
        }
        
        /*
         OnMovingOnPlace += (place, player) => PrintCell(index, currentLeft, currentTop);
        if (GetPlace(index) is Jail)
            OnPlayerGetsOutOfJail += player => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Jail || GetPlace(index) is GotoJail)
            OnPlayerGetsToJail += player => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Property){}
        OnBuyProperty += (property) => PrintCell(index, currentLeft, currentTop);
        
        if (GetPlace(index) is Property){}
        OnCloseAuction += (player, property) => PrintCell(index, currentLeft, currentTop);

        if (GetPlace(index) is Property){}
        OnAcceptOffer += (_, _, _) => PrintCell(index, currentLeft, currentTop); 
        
        OnDeclareBankruptcy += _ => PrintCell(index, currentLeft, currentTop); */
        
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


        PrintSettlers(GetPlace(index));
        
        //ResetColor();
        
        CursorLeft = currentLeft;
        CursorTop = currentTop;
        
        //CursorVisible = true;
    }
    
    private static void PrintSettlers(Place place)
    {
        var settlers =  place.GetSettlers();
            

        for (var i = 0; i < CellPadding; i++)
        {
            
            if (settlers.Length > i) PrintPlayerCharacter(settlers[i]);
            else
            {

                if (place is Property property && property.HasOwner) BackgroundColor = property.GetOwner().GetLabel();
                else BackgroundColor = ConsoleColor.Black;

                Write(" ");
                BackgroundColor = ConsoleColor.DarkGreen;
            }

            //ForegroundColor = currentForeground;
        }

        void PrintPlayerCharacter(Player player)
        {
            // var currentForeground = ForegroundColor;//you always have to do this swap approach
            // var currentBackground = BackgroundColor;
            var f = ForegroundColor;
            ForegroundColor = player.GetLabel();
            BackgroundColor = player.InJail ? ConsoleColor.Gray : ConsoleColor.Black;
            
            Write(GetPlayerCharacter(player).ToString());
            ForegroundColor = f;
            BackgroundColor = ConsoleColor.DarkGreen;

            char GetPlayerCharacter(Player player) => player.GetName()[0];
        }
    }
    //public static void PrintLine(string log) => PrintLine(log);
}