﻿using Chess.Lib;

MoveObject moveObject = new MoveObject();
Board board = new Board();
Piece piece = new Piece();

Engine chessEngine = new Engine(moveObject, board, piece);

Fen fen = new Fen(chessEngine, board, piece);


fen.FenReader("4k3/8/8/8/3n4/8/8/4K3 w - - 0 1"); 

chessEngine.Run();



