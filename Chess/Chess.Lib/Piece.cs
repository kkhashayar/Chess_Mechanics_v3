
namespace Chess.Lib
{
    public class Piece : PieceBase
    {
        public Piece() { }
        public Piece(string name)
        {
            Name = name;
            Index = GetPieceIndex(Name);
            Value = GetPieceValue(Index);
            LegalMoves = AllLegalMoves[Index].ToList();
        }
        public string? Name { get; set; }
        public int Index { get; set; }
        public decimal? Value { get; set; }
        public List<int> LegalMoves { get; set; } 
        public bool? IsMoved { get; set; }


    }
}