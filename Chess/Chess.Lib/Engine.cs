
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading;
namespace Chess.Lib
{
    public class Engine
    {
        private readonly Board _board;
        private readonly Piece _piece;
        public PieceBase pieceBase = new PieceBase();


        public Engine(MoveObject moveObject, Board board, Piece piece)
        {
            _board = board;
            _piece = piece;

        }

        public string inputMove { get; set; }
        public List<Piece> Position = new List<Piece>();
        public string? PieceColor { get; set; }

        public int Turn { get; set; } = 0;  // 0 = white 1 = black
        public List<string> GameHistory { get; set; } = new List<string>();
        public string LastMovedPiece { get; set; }
        public bool PawnFirstMove = true; // Gets the value from fen string, currently is true for test

        public List<string> Path = new List<string>();
        // Castle rights
        public bool WhiteKingCastle { get; set; }
        public bool WhiteQueenCastle { get; set; }
        public bool BlackKingCastle { get; set; }
        public bool BlackQueenCastle { get; set; }

        public int PlayerTurn { get; set; }
        public char BishopSymbol { get; set; }

        // Assigning pieces after parsing the Fen string

        public bool IsAPiece(MoveObject moveObject)
        {
            if (moveObject.SourcePiece != ".") return true;
            return false;
        }

        public bool IsOnBoard(MoveObject moveObject)
        {
            var inspect_MoveObject = moveObject;
            if (moveObject.EndIndex < 0 || moveObject.EndIndex >= _board.board.Count) // 64
            {
                return false;
            }
            return true;
        }

        public bool IsNotSameColor(MoveObject moveObject)
        {
            if (pieceBase.whitePieces.Contains(moveObject.SourcePiece) && pieceBase.whitePieces.Contains(moveObject.TargetPiece))
                return false;
            if (pieceBase.blackPieces.Contains(moveObject.SourcePiece) && pieceBase.blackPieces.Contains(moveObject.TargetPiece))
                return false;

            return true;
        }

        // Piece rules
        public bool IsLegalPieceMove(MoveObject moveObject)
        {
            var currentPiece = moveObject.SourcePiece;
            switch (currentPiece)
            {
                case "N" or "n":
                    if (GetKnight(moveObject)) //Done
                    {
                        return true;
                    }
                    return false;
                case "R" or "r":
                    if (GetRook(moveObject)) //Done
                    {
                        return true;
                    }
                    return false;
                case "B" or "b":
                    if (GetBishop(moveObject)) //Done
                    {
                        return true;
                    }
                    return false;
                case "Q" or "q":
                    if (GetQueen(moveObject)) //Done
                    {
                        return true;
                    }
                    return false;
                // Kings
                case "K":
                    if (GetWhiteKing(moveObject))
                    {
                        return true;
                    }
                    return false;

                case "k":
                    if (GetBlackKing(moveObject))
                    {
                        return true;
                    }
                    return false;
                // Pawns 
                case "P":
                    if (GetWhitePawn(moveObject)) //Done 
                    {
                        return true;
                    }
                    return false;
                case "p":
                    if (GetBlackPawn(moveObject)) //Done 
                    {
                        return true;
                    }
                    return false;

                default:
                    break;
            }
            return true;
        }

        public bool IsKingNotInCheck(MoveObject moveObject)
        {
            return true;
        }

        // Checks for legality of the move
        public bool IsLegalMove(MoveObject moveObject)
        {
            // TODO Legal moves 

            // Board check
            if (IsOnBoard(moveObject))
            {
                //Piece check
                if (IsAPiece(moveObject) == true && IsNotSameColor(moveObject) == true)
                {
                    // Piece rules check
                    if (IsLegalPieceMove(moveObject) == true)
                    {
                        // King is in check check --> TODO after generating moves, for now pass
                        if (IsKingNotInCheck(moveObject) == true)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
            return false;
        }

        // TODO: Where to call this method to be less expensive, and need to have above conditions to prevent calaling it
        // If Castle rights are already false 
        public void SetCastleRules(MoveObject moveObject)
        {
            if (moveObject.SourcePiece == "K")
            {
                WhiteKingCastle = false;
                WhiteQueenCastle = false;
            }
            if (moveObject.SourcePiece == "k")
            {
                BlackKingCastle = false;
                BlackQueenCastle = false;
            }
            if (moveObject.SourcePiece == "R" && moveObject.StartIndex == 63)
            {
                WhiteKingCastle = false;
            }
            else if (moveObject.SourcePiece == "R" && moveObject.StartIndex == 56)
            {
                WhiteQueenCastle = false;
            }
            else if (moveObject.SourcePiece == "r" && moveObject.StartIndex == 7)
            {
                BlackKingCastle = false;
            }
            else if (moveObject.SourcePiece == "r" && moveObject.StartIndex == 0)
            {
                BlackQueenCastle = false;
            }
        }

        // Basic function, adding the last move to GameHistory, Note: This is not a notation  
        public void AddToHistory(MoveObject moveObject)
        {
            // Adding only last piece moved to history for now 
            GameHistory.Add(moveObject.SourcePiece);
        }


        public void ShowBoard()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;
                    if (_board.board[square] != ".")
                    {
                        // Dejavu Sans Mono
                        var piece = _board.board[square];
                        var pieceIndex = _piece.GetPieceIndex(piece);
                        var pieceUnicode = _piece.Unicodes[pieceIndex];
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.Write(pieceUnicode + "  ");
                    }
                    if (_board.board[square] == ".")
                    {
                        Console.Write(_board.board[square] + "  ");
                    }
                }
                Console.WriteLine(_board.ranks[rank]);
            }
            Console.WriteLine("\nA  B  C  D  E  F  G  H");
            Console.WriteLine();

            Console.WriteLine("************** History *************");

            Console.WriteLine("\n************************************");

            Console.WriteLine("************** Position Info ******");
            Console.WriteLine($"W King Castle = {WhiteKingCastle}");
            Console.WriteLine($"W Queen Castle = {WhiteQueenCastle}");
            Console.WriteLine($"B King Castle = {BlackKingCastle}");
            Console.WriteLine($"B Queen Castle = {BlackQueenCastle}");
            Console.WriteLine($"Turn = {Turn}");
            Console.WriteLine($"Player Turn = {PlayerTurn}");

        }


        /*
         
                    "8", "8", "8", "8", "8", "8", "8", "8",
                    "7", "7", "7", "7", "7", "7", "7", "7",
                    "6", "6", "6", "6", "6", "6", "6", "6",
                    "5", "5", "5", "5", "5", "5", "5", "5",
                    "4", "4", "4", "4", "4", "4", "4", "4",
                    "3", "3", "3", "3", "3", "3", "3", "3",
                    "2", "2", "2", "2", "2", "2", "2", "2",
                    "1", "1", "1", "1", "1", "1", "1", "1"
              
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8"
                
         */

        public bool GetKnight(MoveObject moveObject)
        {
            Piece newPiece = new Piece(moveObject.SourcePiece);
            var dif = moveObject.GetDifference();
            if (newPiece.LegalMoves.Contains(moveObject.GetDifference()))
            {
                // Set Logic here
                var startRank = _board.GetRank(moveObject.StartIndex);
                var startFile = _board.GetFile(moveObject.StartIndex);
                var endRank = _board.GetFile(moveObject.EndIndex);
                var endFile = _board.GetFile(moveObject.EndIndex);


                var difRank = endRank - startRank;
                var difFile = endFile - startFile;

                if (difRank <= -6 || difFile <= -6 || difRank >= 6 || difFile >= 6) return false;

                return true;

            }

            return false;
        }
        public bool GetRook(MoveObject moveObject)
        {
            Path.Clear();
            Piece newPiece = new Piece(moveObject.SourcePiece);

            moveObject.BoardStartSquare = _board.GetCoordinates(moveObject.StartIndex);
            moveObject.BoardEndSquare = _board.GetCoordinates(moveObject.EndIndex);

            // Move on files
            if (moveObject.BoardStartSquare.Substring(0, 1) == moveObject.BoardEndSquare.Substring(0, 1))
            {
                if (moveObject.StartIndex > moveObject.EndIndex)
                {
                    // step is -8 --> rook moving up 
                    for (int i = 1; i < 8; i++)
                    {
                        int tempPath = moveObject.StartIndex - (i * 8);
                        if (tempPath > moveObject.EndIndex)
                        {
                            Path.Add(_board.board[tempPath]);
                        }
                        if (tempPath == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (Path.All(x => x == ".") || Path.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                if (moveObject.StartIndex < moveObject.EndIndex)
                {
                    // step is +8 --> rook moving down
                    for (int i = 1; i < 8; i++)
                    {
                        int tempPath = moveObject.StartIndex + (i * 8);
                        if (tempPath < moveObject.EndIndex)
                        {
                            Path.Add(_board.board[tempPath]);
                        }
                        if (tempPath == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (Path.All(x => x == ".") || Path.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            // Moving on ranks
            else if (moveObject.BoardStartSquare.Substring(1, 1) == moveObject.BoardEndSquare.Substring(1, 1))
            {
                // step is -1 --> rook moving left
                if (moveObject.StartIndex > moveObject.EndIndex)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        int tempPath = moveObject.StartIndex - (i * 1);
                        if (tempPath > moveObject.EndIndex)
                        {
                            Path.Add(_board.board[tempPath]);
                        }
                        if (tempPath == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (Path.All(x => x == ".") || Path.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                // step is 1 --> rook moving right 
                if (moveObject.StartIndex < moveObject.EndIndex)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        int tempPath = moveObject.StartIndex + (i * 1); // TODO: check for refactoring here
                        if (tempPath < moveObject.EndIndex)
                        {
                            Path.Add(_board.board[tempPath]);
                        }
                        if (tempPath == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (Path.All(x => x == ".") || Path.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public bool GetBishop(MoveObject moveObject)
        {
            int difference = moveObject.GetDifference();
            // Check if the bishop is moving on a diagonal
            if (difference % 7 != 0 && difference % 9 != 0)
            {
                return false;
            }

            // Check if the bishop is moving along the same diagonal
            int startRank = moveObject.StartIndex / 8;
            int startFile = moveObject.StartIndex % 8;
            int endRank = moveObject.EndIndex / 8;
            int endFile = moveObject.EndIndex % 8;
            if (Math.Abs(startRank - endRank) != Math.Abs(startFile - endFile))
            {
                return false;
            }

            // Check if the bishop can move to the end position by checking if
            // there is a clear diagonal path between the start and end positions
            int direction = 1;
            if (moveObject.StartIndex > moveObject.EndIndex)
            {
                direction = -1;
            }

            int step = 7;
            if (difference % 9 == 0)
            {
                step = 9;
            }

            for (int i = 1; i < difference / step; i++)
            {
                int tempPath = moveObject.StartIndex + direction * i * step;
                if (tempPath != moveObject.EndIndex && _board.board[tempPath] != ".")
                {
                    return false;
                }
            }

            return true;
        }

        public bool GetQueen(MoveObject moveObject)
        {
            moveObject.BoardStartSquare = _board.GetCoordinates(moveObject.StartIndex);
            moveObject.BoardEndSquare = _board.GetCoordinates(moveObject.EndIndex);

            if (moveObject.BoardStartSquare.Substring(0, 1) == moveObject.BoardEndSquare.Substring(0, 1) || // Files
               moveObject.BoardStartSquare.Substring(1, 1) == moveObject.BoardEndSquare.Substring(1, 1)) // Ranks 
            {
                if (GetRook(moveObject))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (GetBishop(moveObject))
                {
                    return true;
                }
                return false;
            }
        }

        public bool GetWhitePawn(MoveObject moveObject)
        {
            Piece pawn = new Piece(moveObject.SourcePiece);

            var row = _board.GetCoordinates(moveObject.StartIndex)[1].ToString();

            if (pawn.LegalMoves.Contains(moveObject.GetDifference() * -1))
            {
                // First 2 square move
                if (moveObject.GetDifference() * (-1) == -16 && row == "2" && _board.board[moveObject.EndIndex] == ".")
                {
                    return true;
                }
                // one square move
                else if (moveObject.GetDifference() * (-1) == -8 && _board.board[moveObject.EndIndex] == ".")
                {
                    return true;
                }
                // Capture right
                else if (moveObject.GetDifference() * (-1) == -9 && _board.board[moveObject.EndIndex] != "." && !_piece.whitePieces.Contains(_board.board[moveObject.EndIndex]))
                {
                    return true;
                }
                // Capture left 
                else if (moveObject.GetDifference() * (-1) == -7 && _board.board[moveObject.EndIndex] != "." && !_piece.whitePieces.Contains(_board.board[moveObject.EndIndex]))
                {
                    return true;
                }
                // Right En passant
                if (moveObject.GetDifference() * (-1) == -7)
                {
                    // 
                    // TODO: Condition if last pawn move was it's first move have to be set in game history, now is always true
                    // TODO: Might add to properties // looks like no need to check right/left gap 
                    var tempStartIndex = moveObject.StartIndex;
                    var tempEndIndex = moveObject.EndIndex;
                    //var leftGap = tempStartIndex; 
                    //var rightGap = tempEndIndex;

                    var lastMove = GameHistory.LastOrDefault();
                    LastMovedPiece = lastMove[0].ToString();

                    if (_board.board[moveObject.EndIndex] == "." && LastMovedPiece == "p" && PawnFirstMove == true)
                    {
                        var targetObject = _board.board[moveObject.StartIndex + 1];
                        var targetObjectIndex = moveObject.StartIndex + 1;
                        if (targetObject == "p")
                        {
                            _board.board[targetObjectIndex] = ".";
                            return true;
                        }
                        return false;
                    }

                }
                // Left En passant 
                if (moveObject.GetDifference() * (-1) == -9)
                {
                    // 
                    // TODO: Condition if last pawn move was it's first move have to be set in game history, now is always true
                    // TODO: Might add to properties // looks like no need to check right/left gap 
                    var tempStartIndex = moveObject.StartIndex;
                    var tempEndIndex = moveObject.EndIndex;


                    var lastMove = GameHistory.LastOrDefault();
                    LastMovedPiece = lastMove[0].ToString();

                    if (_board.board[moveObject.EndIndex] == "." && LastMovedPiece == "p" && PawnFirstMove == true)
                    {
                        var targetObject = _board.board[moveObject.StartIndex - 1];
                        var targetObjectIndex = moveObject.StartIndex - 1;

                        if (_board.board[moveObject.StartIndex - 1] == "p")
                        {
                            _board.board[targetObjectIndex] = ".";
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }

            return false;
        }

        public bool GetBlackPawn(MoveObject moveObject)
        {
            Piece pawn = new Piece(moveObject.SourcePiece);
            var row = _board.GetCoordinates(moveObject.StartIndex)[1].ToString();

            if (pawn.LegalMoves.Contains(moveObject.GetDifference()))
            {
                if (moveObject.GetDifference() == 16 && row == "7" && _board.board[moveObject.EndIndex] == ".")
                {
                    return true;
                }
                else if (moveObject.GetDifference() == 8 && _board.board[moveObject.EndIndex] == ".")
                {
                    return true;
                }
                else if (moveObject.GetDifference() == 9 && _board.board[moveObject.EndIndex] != "." && !_piece.blackPieces.Contains(_board.board[moveObject.EndIndex]))
                {
                    return true;
                }
                else if (moveObject.GetDifference() == 7 && _board.board[moveObject.EndIndex] != "." && !_piece.blackPieces.Contains(_board.board[moveObject.EndIndex]))
                {
                    return true;
                }

                // Right En passant
                if (moveObject.GetDifference() == 9)
                {
                    // 
                    // TODO: Condition if last pawn move was it's first move have to be set in game history, now is always true
                    // TODO: Might add to properties // looks like no need to check right/left gap 
                    var tempStartIndex = moveObject.StartIndex;
                    var tempEndIndex = moveObject.EndIndex;
                    //var leftGap = tempStartIndex; 
                    //var rightGap = tempEndIndex;

                    var lastMove = GameHistory.LastOrDefault();
                    LastMovedPiece = lastMove[0].ToString();

                    if (_board.board[moveObject.EndIndex] == "." && LastMovedPiece == "P" && PawnFirstMove == true)
                    {
                        var targetObject = _board.board[moveObject.StartIndex + 1];
                        var targetObjectIndex = moveObject.StartIndex + 1;
                        if (targetObject == "P")
                        {
                            _board.board[targetObjectIndex] = ".";
                            return true;
                        }
                        return false;
                    }

                }
                // Left En passant 
                if (moveObject.GetDifference() == 7)
                {
                    // 
                    // TODO: Condition if last pawn move was it's first move have to be set in game history, now is always true
                    // TODO: Might add to properties // looks like no need to check right/left gap 
                    var tempStartIndex = moveObject.StartIndex;
                    var tempEndIndex = moveObject.EndIndex;


                    var lastMove = GameHistory.LastOrDefault();
                    LastMovedPiece = lastMove[0].ToString();

                    if (_board.board[moveObject.EndIndex] == "." && LastMovedPiece == "P" && PawnFirstMove == true)
                    {
                        var targetObject = _board.board[moveObject.StartIndex - 1];
                        var targetObjectIndex = moveObject.StartIndex - 1;

                        if (_board.board[moveObject.StartIndex - 1] == "P")
                        {
                            _board.board[targetObjectIndex] = ".";
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }

            return false;
        }

        public bool GetWhiteKing(MoveObject moveObject)
        {
            Piece whiteKing = new Piece(moveObject.SourcePiece);
            var dif = moveObject.GetDifferenceOnKingMove();

            if ((moveObject.GetDifferenceOnKingMove() != -2 || moveObject.GetDifferenceOnKingMove() == 2)
                && whiteKing.LegalMoves.Contains(moveObject.GetDifferenceOnKingMove())
                && !_piece.whitePieces.Contains(_board.board[moveObject.EndIndex])
                && _board.board[moveObject.EndIndex] == ".")
            {
                WhiteQueenCastle = false;
                WhiteKingCastle = false;
                return true;
            }

            if (moveObject.GetDifferenceOnKingMove() == -2
                        && WhiteQueenCastle == true
                        && _board.board[moveObject.StartIndex + 1].ToString() == "."
                        && _board.board[moveObject.StartIndex + 2].ToString() == "."
                        && _board.board[63] == "R")
            {
                _board.board[63] = ".";
                _board.board[61] = "R";
                WhiteKingCastle = false;
                WhiteQueenCastle = false;
                return true;
            }
            else if (moveObject.GetDifferenceOnKingMove() == +2
                        && WhiteKingCastle == true
                        && _board.board[moveObject.StartIndex - 1].ToString() == "."
                        && _board.board[moveObject.StartIndex - 2].ToString() == "."
                        && _board.board[moveObject.StartIndex - 3].ToString() == "."
                        && _board.board[56] == "R")
            {
                _board.board[56] = ".";
                _board.board[59] = "R";
                WhiteQueenCastle = false;
                WhiteKingCastle = false;
                return true;
            }
            return false;
        }


        public bool GetBlackKing(MoveObject moveObject)
        {
            Piece blackKing = new Piece(moveObject.SourcePiece);
            if ((moveObject.GetDifferenceOnKingMove() != -2 || moveObject.GetDifferenceOnKingMove() == 2)
                && blackKing.LegalMoves.Contains(moveObject.GetDifferenceOnKingMove())
                && !_piece.whitePieces.Contains(_board.board[moveObject.EndIndex])
                && _board.board[moveObject.EndIndex] == ".")
            {
                WhiteQueenCastle = false;
                WhiteKingCastle = false;
                return true;
            }

            if (moveObject.GetDifferenceOnKingMove() == -2
                        && BlackQueenCastle == true
                        && _board.board[moveObject.StartIndex + 1].ToString() == "."
                        && _board.board[moveObject.StartIndex + 2].ToString() == "."

                        && _board.board[7] == "r")
            {
                _board.board[7] = ".";
                _board.board[5] = "r";
                BlackKingCastle = false;
                BlackQueenCastle = false;
                return true;
            }
            else if (moveObject.GetDifferenceOnKingMove() == +2
                        && BlackKingCastle == true
                        && _board.board[moveObject.StartIndex - 1].ToString() == "."
                        && _board.board[moveObject.StartIndex - 2].ToString() == "."
                        && _board.board[moveObject.StartIndex - 3].ToString() == "."
                        && _board.board[0] == "r")
            {
                _board.board[0] = ".";
                _board.board[3] = "r";
                BlackKingCastle = false;
                BlackQueenCastle = false;
                return true;
            }
            return false;
        }

        public bool IsKingInCheck(MoveObject moveObject)
        {
            return false;
        }

        // Getting actual input from user or machine, evantually this will move to UI 
        public void GetMove()
        {
            Console.Write("\nMove: ");
            var inputMove = Console.ReadLine();
            if (inputMove != null)
            {
                try
                {
                    int startSquare = _board.GetIndex(inputMove.Substring(0, 2));
                    int endSquare = _board.GetIndex(inputMove.Substring(2, 2));
                    if (inputMove.Count() == 4
                        && _board.coordinates.Contains(inputMove.Substring(0, 2))
                        && _board.coordinates.Contains(inputMove.Substring(2, 2))
                        && _board.board[startSquare] != ".")
                    {
                        MoveObject newMove = new MoveObject
                        {
                            StartIndex = startSquare,
                            EndIndex = endSquare,
                            SourcePiece = _board.board[startSquare],
                            TargetPiece = _board.board[endSquare],
                        };
                        if (IsLegalMove(newMove))
                        {
                            MakeMove(newMove); // Actual move process 
                        }
                    }
                }
                catch (System.ArgumentOutOfRangeException) { }
            }
        }

        // Changes the position on boardbased on given move, 
        public void MakeMove(MoveObject moveObject)
        {
            if(IsLegalMove(moveObject))
            {
                if (Turn == 0 && _piece.whitePieces.Contains(moveObject.SourcePiece))
                {
                    _board.board[moveObject.EndIndex] = moveObject.SourcePiece;
                    _board.board[moveObject.StartIndex] = ".";
                    AddToHistory(moveObject);

                    Turn = 1;

                    Console.Beep(700, 200);
                    ShowBoard();
                }

                else if (Turn == 1 && _piece.blackPieces.Contains(moveObject.SourcePiece))
                {
                    _board.board[moveObject.EndIndex] = moveObject.SourcePiece;
                    _board.board[moveObject.StartIndex] = ".";
                    AddToHistory(moveObject);

                    Turn = 0;

                    Console.Beep(500, 200);
                    ShowBoard();
                }
            }
            
        }

        /// <summary>
        /// ///////////////////////////// Random move generator //////////////////////////////////////
        /// </summary>
        
        public MoveObject GenerateKnightMove()
        {
            var moveObject = new MoveObject();
            var random = new Random();

            // Finding a first Knight on the board with a right color / Turn 
            var knightPosition = -1;
            char knightSymbol;
            
            if (Turn == 1) knightSymbol = 'n';
            else knightSymbol = 'N';

            for (int i = 0; i < _board.board.Count; i++)
            {
                if (_board.board[i] == knightSymbol.ToString())
                {
                    var piece = new Piece(knightSymbol.ToString());
                    moveObject.StartIndex = i;
                    var dif = piece.LegalMoves[random.Next(piece.LegalMoves.Count)];
                    moveObject.EndIndex = (moveObject.StartIndex + dif);
                    moveObject.SourcePiece = piece.Name;
                }
            }
            return moveObject;
        }
        //public MoveObject GenerateWhitekKnightMove()
        //{
        //    var moveObject = new MoveObject();
        //    var random = new Random();
        //    for (int i = 0; i < _board.board.Count; i++)
        //    {
        //        if (_board.board[i] == "N")
        //        {
        //            var piece = new Piece("N");
        //            moveObject.StartIndex = i;
        //            var dif = piece.LegalMoves[random.Next(piece.LegalMoves.Count)];
        //            moveObject.EndIndex = (moveObject.StartIndex + dif);
        //            moveObject.SourcePiece = piece.Name;
        //        }
        //    }
        //    return moveObject;
        //}

        public MoveObject GenerateBishopeMove()
        {
            var moveObject = new MoveObject();
            var random = new Random();

            // Find the first bishop on the board with the correct color
            var bishopPosition = -1;
            char bishopSymbol;
            if (Turn == 1)
            {
                bishopSymbol = 'b';
            }
            else
            {
                bishopSymbol = 'B';
            }

            for (int i = 0; i < _board.board.Count; i++)
            {
                if (_board.board[i] == bishopSymbol.ToString())
                {
                    bishopPosition = i;
                    break;
                }
            }

            // If no bishop was found, exit the code for now 
            if (bishopPosition == -1)
            {
                Environment.Exit(0);
            }

            // Generate a random move that only moves the bishop in a single diagonal
            int direction = random.Next(2) == 0 ? 1 : -1;
            int step = random.Next(1, 8);
            var endPosition = bishopPosition + direction * step * (step % 2 == 0 ? 7 : 9);

            moveObject.StartIndex = bishopPosition;
            moveObject.EndIndex = endPosition;
            moveObject.SourcePiece = bishopSymbol.ToString();

            return moveObject;
        }
        /// <summary>
        /// ///////////////////////////// End of Random move generator //////////////////////////////////////
        /// </summary>
        

        // Testing 
        public void GetRandomFunction()
        {
            var GeneratorMethods = new List<Func<MoveObject>>
            {
                () => GenerateKnightMove(),
                () => GenerateBishopeMove(),
            };
    
        }


        public async void Run()
        {
            bool running = true;
            while (running)
            {

                ShowBoard();
                if (Turn == 0)
                {
                    //GetMove();

                    //var move = GenerateWhitekKnightMove(); // 8/3n4/8/8/8/8/3N4/8 w - - 0 1
                    
                    var move = GenerateBishopeMove(); // 8/3b4/8/8/8/8/3B4/8 w - - 0 1
                    MakeMove(move);

                    Thread.Sleep(50);
                }
                else if (Turn == 1)
                {

                    //GetMove();

                    //var move = GenerateBlackKnightMove();
                    var move = GenerateBishopeMove();

                    MakeMove(move);

                    Thread.Sleep(50);
                }
            }
        }
    }
}

/*
   "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
   "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
   "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
   "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
   "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
   "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
   "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
   "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"


    {-6,-10,-15,-17,+6,+10,+15,+17 } removed -6,+6
    new int[] {-10,-15,-17,-6, +6,+10,+15,+17 }, // White Knight

          1     2     3     4     5     6     7     8 
     
     1   "00", "01", "02", "03", "04", "05", "06", "07", 
     2   "08", "09", "10", "11", "12", "13", "14", "15",
     3   "16", "17", "18", "19", "20", "21", "22", "23",
     4   "24", "25", "26", "27", "28", "29", "30", "31",
     5   "32", "33", "34", "35", "36", "37", "38", "39",
     6   "40", "41", "42", "43", "44", "45", "46", "47",
     7   "48", "49", "50", "51", "52", "53", "54", "55",
     8   "56", "57", "58", "59", "60", "61", "62", "63"

 */


/*
 * TOODO:
 * What are the absolute bare essentials your main objects need?
 * 
 * For instance, your game "engine", what does the engine actually need to do?
 */


/*
 * 1: Scan the board
 * 2: For each piece generate all possible moves
 * 3: If in possible move Can hit the king will break the loop and return true
 * 4: else moves to another piece 
 * 
 * for first stage will scan startsquare and endsquare of kings move 
 * for second stage will scan castling, and will add more flags on castle rights 
 */
