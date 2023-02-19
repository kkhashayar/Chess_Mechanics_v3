using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Lib
{
    public struct MoveObject
    {
        public MoveObject(){}

        public int StartIndex { get; set; } = 0;
        public int EndIndex { get; set; } = 0;
        public string SourcePiece { get; set; } = string.Empty;
        public string TargetPiece { get; set; } = string.Empty;
        // public bool IsLegal { get; set; } = false;
        public string BoardStartSquare { get; set; } = string.Empty;
        public string BoardEndSquare { get; set; } = string.Empty;

        public int GetDifferenceOnKingMove()
        {
            return StartIndex - EndIndex;
        }
        public int GetDifference()
        {
            if (StartIndex >= EndIndex)
            {
                return StartIndex - EndIndex;
            }
            else
            {
                return EndIndex - StartIndex;
            }
        }
    }

}
