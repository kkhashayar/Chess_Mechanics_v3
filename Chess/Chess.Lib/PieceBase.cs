using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Lib
{
    public class PieceBase
    {
        // All pieces as a string
        public List<string> allPieces = new List<string> { "P", "N", "B", "R", "Q", "K", "p", "n", "b", "r", "q", "k" ,""}; //Empty piece
        public List<string> whitePieces = new List<string> { "P", "N", "B", "R", "Q", "K" };
        public List<string> blackPieces = new List<string> { "p", "n", "b", "r", "q", "k" };
        public List<decimal> AllValues = new List<Decimal> { 1, 3, 3.5m, 5, 9, 9999, 1, 3, 3.5m, 5, 9, 9999, 0 };

        public int[][] AllLegalMoves = new int[][]
           {
                new int[] {-8,-16,-7,-9}, // white Pawn
                new int[] {-6,-10,-15,-17,+6,+10,+15,+17 }, // White Knight
                new int[] {-9, -7, 9, 7}, // white Bishop
                new int[] {-1, -8, +1, +8}, // White Rook
                new int[] {-10, 1, 10, -1, -9, -11, 11, 9}, // White Queen --> Not in use
                new int[] {-1, -8, -2, +1, +8, +2, -9, -7, +9, +7}, // White king 
                new int[] {+8,+16,+7,+9}, // will be changed
                new int[] {-6,-10,-15,-17,+6,+10,+15,+17 }, // Knight
                new int[] {+8, +16, +7, +9}, // Black pawn
                new int[] {-1, -8, +1, +8}, //  Black Rook
                new int[] {-10, 1, 10, -1, -9, -11, 11, 9},// Black Queen --> Not in use
                new int[] {-1, -8, -2, +1, +8, +2, -9, -7, +9, +7}, // Black king
           };

        public int GetPieceIndex(string piece)
        {
            return allPieces.IndexOf(piece);
        }

        public decimal GetPieceValue(int index)
        {
            return AllValues[index]; 
        }
        public List<int> GetLegalMoves(int index)
        {
            return AllLegalMoves[index].ToList();
        }
        //var pieceLegalMoves = pieceBase.AllLegalMoves[pieceIndex];
    }
}

 