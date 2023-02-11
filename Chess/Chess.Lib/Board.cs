using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Lib
{
    public class Board
    {
        
        public int[] ranks = new int[8] { 8, 7, 6, 5, 4, 3, 2, 1 };
        public int[] files = new int[8] { 8, 7, 6, 5, 4, 3, 2, 1 };

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

        public List<string> ranksBoard = new List<string>
                {
                    "8", "8", "8", "8", "8", "8", "8", "8",
                    "7", "7", "7", "7", "7", "7", "7", "7",
                    "6", "6", "6", "6", "6", "6", "6", "6",
                    "5", "5", "5", "5", "5", "5", "5", "5",
                    "4", "4", "4", "4", "4", "4", "4", "4",
                    "3", "3", "3", "3", "3", "3", "3", "3",
                    "2", "2", "2", "2", "2", "2", "2", "2",
                    "1", "1", "1", "1", "1", "1", "1", "1"
                };

        public List<string> filesBoard = new List<string>
                {
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8",
                    "1", "2", "3", "4", "5", "6", "7", "8"
                };

        public int GetRank(int squareIndex)
        {
            var rank = ranksBoard[squareIndex];
            var _rank = rank.ToString();
            return Int32.Parse(_rank);  
        }
        public int GetFile(int squareIndex)
        {
            var file = filesBoard[squareIndex];
            var _file = file.ToString(); 
            return Int32.Parse(file); 
        }

        public int GetRankOrFileDifference(int rankOrFile, int rankOrFileTarget)
        {
            return rankOrFile - rankOrFileTarget;
           
        }


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

/*
 * 
   "00", "01", "02", "03", "04", "05", "06", "07",
   "08", "09", "10", "11", "12", "13", "14", "15",
   "16", "17", "18", "19", "20", "21", "22", "23",
   "24", "25", "26", "27", "28", "29", "30", "31",
   "32", "33", "34", "35", "36", "37", "38", "39",
   "40", "41", "42", "43", "44", "45", "46", "47",
   "48", "49", "50", "51", "52", "53", "54", "55",
   "56", "57", "58", "59", "60", "61", "62", "63"
   

    Knight start = 0 ends -> +10 or +17 or -10 or -17 

 */
