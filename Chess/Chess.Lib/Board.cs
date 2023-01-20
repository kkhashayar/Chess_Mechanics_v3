﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Lib
{
    public class Board
    {
        public int[] ranks = new int[8] { 8, 7, 6, 5, 4, 3, 2, 1 };

        public List<string> board = new List<string>
                {
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", ".",
                   ".", ".", ".", ".", ".", ".", ".", "."
                };

        public List<string> coordinates = new List<string>
                {
                    "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
                    "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
                    "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
                    "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
                    "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
                    "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
                    "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
                    "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"
                };

        public string GetCoordinates(int squareIndex)
        {
            return coordinates[squareIndex];
        }

        public int GetIndex(string squareCoordinate)
        {
            if (IsValidCoordinate(squareCoordinate))
            {
                return coordinates.IndexOf(squareCoordinate);
            }
            return 65;
        }

        public bool IsValidCoordinate(string squareName)
        {
            if (coordinates.Contains(squareName)) return true;
            return false;
        }

        public bool IsValidIndex(int squareIndex)
        {
            if (squareIndex >= 0 && squareIndex <= 63) return true;
            return false;
        }
    }
}
