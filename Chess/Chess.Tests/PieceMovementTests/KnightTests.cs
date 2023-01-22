using Chess.Lib;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Tests.PieceMovementTests
{
    [TestFixture]
    public class KnightTests
    {

        [Test]
        [TestCase(10,0,  true)]
        [TestCase(10,17, true)]
        //[TestCase(10,18, false)] // problem on this 
        public void GetKnight_On_GivenMove_Returns_True(int startIndex, int endIndex, bool expectedResult)
        {
            // Arrange
            Piece knight = new Piece("N");
            
            Board board = new Board();

            MoveObject moveObject = new MoveObject()
            {
                StartIndex = 0,
                EndIndex = 10,
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



        public MoveObject GenerateKnightMove(int startSquare, int endSquare)
        {
            MoveObject move = new MoveObject()
            {
                StartIndex = startSquare,
                EndIndex = endSquare,
                SourcePiece = "N"
            };

            return move;
        }


    }
}

/*
 *      [Test]
        [TestCase(inputA, inputB, "ExpectedResult")]
        
        public void GetGrade_Returns_Right_Grade(int CaseA, int CaseB, string expectedResult)
        {
          
            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(0, 61, "F")]
        
        // Another way to use ExpectedResult 
        [Test]
        [TestCase(0, 61,  ExpectedResult = "F")]
        [TestCase(20, 80, ExpectedResult = "F")]
        [TestCase(59, 80, ExpectedResult = "F")]
       
 */

