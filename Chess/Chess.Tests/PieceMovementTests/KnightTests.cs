using Chess.Lib;
using NUnit.Framework.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Tests.PieceMovementTests
{
    [TestFixture]
    public class KnightTests
    {
        // {-6,-10,-15,-17,+6,+10,+15,+17 }, // Knight Legal moves
        [Test]
        [TestCase(0, 10, true)] // StartIndex, EndIndex, expected return 
        [TestCase(0, 17, true)]
        [TestCase(0, 18, false)]
        public void GetKnight_On_GivenMove_Returns_True(int startIndex, int endIndex, bool expectedResult)
        {
            // Arrange
            Piece knight = new Piece("N");

            Board board = new Board();

            MoveObject moveObject = new MoveObject()
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                SourcePiece = knight.Name,
            };

            moveObject.GetDifference();

            Engine engine = new Engine(moveObject, board, knight);

            var sut = engine.GetKnight(moveObject);

            // Act
            var result = sut;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        ///
        public int GenerateKnightMove(int startPositionIndex)
        {

            List<int> moves = new List<int>();
            List<int> KnightMoves = new List<int> { -6, -10, -15, -17, +6, +10, +15, +17 };

            var random = new Random();

            int startIndex = startPositionIndex;

            int move = startIndex += random.Next(KnightMoves.Count);

            return move;
        }


    }
}

/*
  new int[] {-8,-16,-7,-9}, // white Pawn
  new int[] {-6,-10,-15,-17,+6,+10,+15,+17 }, // White Knight
  new int[] {-9, -7, 9, 7}, // white Bishop
  new int[] {-1, -8, +1, +8}, // White Rook
  new int[] {-10, 1, 10, -1, -9, -11, 11, 9}, // White Queen --> Not in use
  new int[] {-1, -8, -2, +1, +8, +2, -9, -7, +9, +7}, // White king 
  new int[] {+8,+16,+7,+9}, // will be changed
  new int[] {-6,-10,-15,-17,+6,+10,+15,+17 }, // Black Knight
  new int[] {+8, +16, +7, +9}, // Black pawn
  new int[] {-1, -8, +1, +8}, //  Black Rook
  new int[] {-10, 1, 10, -1, -9, -11, 11, 9},// Black Queen --> Not in use
  new int[] {-1, -8, -2, +1, +8, +2, -9, -7, +9, +7}, // Black king
       
 */

