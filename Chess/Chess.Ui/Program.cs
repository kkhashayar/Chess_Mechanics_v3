using Chess.Lib;

MoveObject moveObject = new MoveObject();
Board board = new Board();
Piece piece = new Piece();

Engine chessEngine = new Engine(moveObject, board, piece);

Fen fen = new Fen(chessEngine, board, piece);


fen.FenReader("r2qk2r/p7/8/2n5/8/7P/8/R2QK2R w KQkq - 0 1"); 

chessEngine.Run();



