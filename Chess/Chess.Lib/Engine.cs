

using System.Linq;
using System.Net.NetworkInformation;

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
        public string? Turn { get; set; } = "white";
        public List<string> GameHistory { get; set; } = new List<string>();
        public string LastMovedPiece { get; set; }
        public bool PawnFirstMove = true; // Gets the value from fen string, currently is true for test

        public List<string> Path = new List<string>();
        // Castle rights
        public bool WhiteKingCastle { get; set; }
        public bool WhiteQueenCastle { get; set; }
        public bool BlackKingCastle { get; set; }
        public bool BlackQueenCastle { get; set; }

        // Assigning pieces after parsing the Fen string


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
                            MakeMove(newMove);
                        }
                    }

                }
                catch (System.ArgumentOutOfRangeException) { }
            }
            GetMove();
        }


        public bool IsAPiece(MoveObject moveObject)
        {
            if (moveObject.SourcePiece != ".") return true;
            return false;
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
                    if (GetWhitePawn(moveObject))
                    {
                        return true;
                    }
                    return false;
                case "p":
                    if (GetBlackPawn(moveObject))
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
            if (IsAPiece(moveObject) == true && IsNotSameColor(moveObject) == true)
            {
                // Piece rules check
                if (IsLegalPieceMove(moveObject) == true)
                {
                    // King is in check check :D
                    if (IsKingNotInCheck(moveObject) == true)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        // Changes the position on boardbased on given move, 
        public void MakeMove(MoveObject moveObject)
        {
            _board.board[moveObject.EndIndex] = moveObject.SourcePiece;
            _board.board[moveObject.StartIndex] = ".";
            
            // TODO Implement --> Read / Write method to and from History

            ShowBoard();
        }

        public void ShowBoard()
        {
            Console.Clear();
            Console.WriteLine("***************************");

            Console.WriteLine();
            Console.WriteLine();
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;

                    Console.Write(_board.board[square] + "  ");
                }
                Console.WriteLine(_board.ranks[rank]);
            }
            Console.WriteLine("\nA  B  C  D  E  F  G  H");
            Console.WriteLine();

            Console.WriteLine("************** History *************");

            Console.WriteLine("\n************************************");

            Console.WriteLine("************** Position facts ******");
            Console.WriteLine($"W King Castle = {WhiteKingCastle}");
            Console.WriteLine($"W Queen Castle = {WhiteQueenCastle}");
            Console.WriteLine($"B King Castle = {BlackKingCastle}");
            Console.WriteLine($"B Queen Castle = {BlackQueenCastle}");
            Console.WriteLine("\n************************************");

        }
        public void Run()
        {
            ShowBoard();

            GetMove();
            Console.ReadKey();
        }

        public bool GetKnight(MoveObject moveObject)
        {
            Piece newPiece = new Piece(moveObject.SourcePiece);
            if (newPiece.LegalMoves.Contains(moveObject.GetDifference())) return true;
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
            Path.Clear();
            // Is move dividable by 9,7 --> diagonal next squares 
            // int result_of_9s = moveObject.GetDifference() % 9;
            // int result_of_7s = moveObject.GetDifference() % 7;


            if (moveObject.Difference % 9 == 0 || moveObject.Difference % 7 == 0
                && moveObject.BoardStartSquare.Substring(0, 1) != moveObject.BoardEndSquare.Substring(0, 1)
                && moveObject.BoardStartSquare.Substring(0, 2) != moveObject.BoardEndSquare.Substring(0, 2)// Preventing file crossing moves
                && moveObject.BoardStartSquare.Substring(1, 1) != moveObject.BoardEndSquare.Substring(1, 1)) // Preventing rank crossing moves
            {
                // +9 up right 
                // +7 up left
                if (moveObject.StartIndex > moveObject.EndIndex)
                {
                    if (moveObject.GetDifference() % 9 == 0)
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            int tempPath = moveObject.StartIndex - (9 * i);
                            if (tempPath >= moveObject.EndIndex && tempPath <= _board.board.Count)
                            {
                                if (tempPath != moveObject.EndIndex)
                                {
                                    Path.Add(_board.board[tempPath]);
                                }
                            }
                        }
                        if (Path.All(x => x == ".") || Path.Count == 0)
                        {
                            return true;
                        }
                    }
                    // +7 up left
                    if (moveObject.GetDifference() % 7 == 0)
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            int tempPat = moveObject.StartIndex - (7 * i);
                            if (tempPat >= moveObject.EndIndex && tempPat <= _board.board.Count)
                            {
                                if (tempPat != moveObject.EndIndex)
                                {
                                    Path.Add(_board.board[tempPat]);
                                }
                            }
                        }
                        if (Path.All(x => x == ".") || Path.Count == 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {   // -9 down right
                    if (moveObject.GetDifference() % 9 == 0)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            int tempPath = moveObject.StartIndex + (9 * i);
                            if (tempPath <= moveObject.EndIndex && tempPath <= _board.board.Count)
                            {
                                if (tempPath != moveObject.EndIndex)
                                {
                                    Path.Add(_board.board[tempPath]);
                                }
                            }
                        }
                        if (Path.All(x => x == ".") || Path.Count == 0)
                        {
                            return true;
                        }
                    }
                    // -7 down  left
                    if (moveObject.GetDifference() % 7 == 0)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            int tempPat = moveObject.StartIndex + (7 * i);
                            if (tempPat <= moveObject.EndIndex && tempPat <= _board.board.Count)
                            {
                                if (tempPat != moveObject.EndIndex)
                                {
                                    Path.Add(_board.board[tempPat]);
                                }
                            }
                        }
                        if (Path.All(x => x == ".") || Path.Count == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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
                // Left En passant 
                if(moveObject.GetDifference() * (-1) == -7 || moveObject.GetDifference() * (-1) == -9)
                {
                    // will add to properties 
                    var leftGap = moveObject.StartIndex += 15; // I think they should be -15 and -17 // to be implemented 
                    var rightGap = moveObject.EndIndex += 17;
                    if (_board.board[moveObject.EndIndex] == "." && LastMovedPiece == "p" && PawnFirstMove == true)
                    {
                        if (_board.board[moveObject.StartIndex + 1] != null && _board.board[moveObject.StartIndex + 1] == "p")
                        {
                            _board.board[moveObject.StartIndex + 1] = ".";
                            return true;
                        }
                        else if (_board.board[moveObject.StartIndex - 1] != null && _board.board[moveObject.StartIndex - 1] == "p")
                        {
                            _board.board[moveObject.StartIndex - 1] = ".";
                            return true;
                        }
                        return false;
                    }

                }
                // Right En passant 
                return false;
            }
            
            return false;
        }

        public bool GetBlackPawn(MoveObject moveObject)
        {
            Piece pawn = new Piece(moveObject.SourcePiece);

            //var leftGap = moveObject.StartIndex -= 15;
            //var rightGap = moveObject.EndIndex -= 17;
            
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
                
                return false;
            }

            return false;
        }

        public bool GetWhiteKing(MoveObject moveObject)
        {
            return false;
        }

        public bool GetBlackKing(MoveObject moveObject)
        {
            return false;
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
 */

