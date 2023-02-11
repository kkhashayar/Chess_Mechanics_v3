using Chess.Lib;

MoveObject moveObject = new MoveObject();
Board board = new Board();
Piece piece = new Piece();

Engine chessEngine = new Engine(moveObject, board, piece);

Fen fen = new Fen(chessEngine, board, piece);


fen.FenReader("n6n/8/8/8/8/8/8/N6N w - - 0 1"); 

chessEngine.Run();



