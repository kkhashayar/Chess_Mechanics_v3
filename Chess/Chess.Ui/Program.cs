using Chess.Lib;

MoveObject moveObject = new MoveObject();
Board board = new Board();
Piece piece = new Piece();

Engine chessEngine = new Engine(moveObject, board, piece);

Fen fen = new Fen(chessEngine, board, piece);


//fen.FenReader("8/3b4/8/8/8/8/3B4/8 w - - 0 1");



fen.FenReader("5b2/8/8/8/8/8/8/2B5 w - - 0 1");

chessEngine.Run();



