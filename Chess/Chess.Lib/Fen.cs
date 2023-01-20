using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Lib
{
    public class Fen
    {
        private readonly Engine _engine;
        private readonly Board _board;
        private readonly Piece _piece;

        public Fen(Engine engine, Board board, Piece piece)
        {
            _board = board;
            _piece = piece;
            _engine = engine;
        }
       
        public void FenReader(string fen = "")
        {
            if(fen.Length == 0 || fen == null)
            {
                fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }
            string[] fenList = fen.Split(" ");
            string[] rows = fenList[0].Split("/");

            if (fenList[2].Contains("K"))
            {
                _engine.WhiteKingCastle = true;
            }
            if (fenList[2].Contains("Q"))
            {
                _engine.WhiteQueenCastle = true;
            }
            if (fenList[2].Contains("k"))
            {
                _engine.BlackKingCastle = true;
            }
            if (fenList[2].Contains("q"))
            {
                _engine.BlackQueenCastle = true;
            }
            _engine.Turn = fenList[1];

            int index = 0;
            foreach (var row in rows)
            {
                foreach (var square in row)
                {
                    if (char.IsDigit(square))
                    {
                        int emptySquare = int.Parse(square.ToString());
                        for (int i = 0; i < emptySquare; i++)
                        {
                            _board.board[index] = ".";
                            index++;
                        }
                    }
                    else
                    {
                        string piece = square.ToString();
                        string actualPiece;

                        actualPiece = piece;

                        _board.board[index] = actualPiece;
                        index++;
                    }
                }
            }
            SetupPosition();
        }
        public void SetupPosition()
        {

            for (int i = 0; i < _board.board.Count(); i++)
            {
                
                string positioinCoords = _board.GetCoordinates(i);
                var row = positioinCoords[1].ToString();
                if (_board.board[i] != ".")
                {
                    if (_board.board[i] == "N")
                    {
                        var _piece = new Piece()
                        {
                            Name = "N",
                            Value = 3
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "n")
                    {
                        var _piece = new Piece()
                        {
                            Name = "n",
                            Value = 3
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "R")
                    {
                        var _piece = new Piece()
                        {
                            Name = "R",
                            Value = 5
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "r")
                    {
                        var _piece = new Piece()
                        {
                            Name = "r",
                            Value = 5
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "B")
                    {
                        var _piece = new Piece()
                        {
                            Name = "B",
                            Value = 3.5m
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "b")
                    {
                        var _piece = new Piece()
                        {
                            Name = "b",
                            Value = 3.5m
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "Q")
                    {
                        var _piece = new Piece()
                        {
                            Name = "Q",
                            Value = 9
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "q")
                    {
                        var _piece = new Piece()
                        {
                            Name = "q",
                            Value = 9
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "P")
                    {
                        var _piece = new Piece()
                        {
                            Name = "P",
                            Value = 1
                        };
                        if (row == "2")
                        {
                            _piece.IsMoved = false;
                        }
                        else
                        {
                            _piece.IsMoved = true;
                        }

                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "p")
                    {
                        var _piece = new Piece()
                        {
                            Name = "p",
                            Value = 1
                        };
                        if (row == "7")
                        {
                            _piece.IsMoved = false;
                        }
                        else
                        {
                            _piece.IsMoved = true;
                        }
                        _engine.Position.Add(_piece);
                    }

                    else if (_board.board[i] == "K")
                    {
                        var _piece = new Piece()
                        {
                            Name = "K",
                            Value = 9999
                        };
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "k")
                    {
                        var _piece = new Piece()
                        {
                            Name = "k",
                            Value = 9999
                        };
                        _engine.Position.Add(_piece);
                    }
                }
            }
        }
    }
}
