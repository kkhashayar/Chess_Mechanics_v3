using Chess.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Tests.PieceMovementTests
{
    
    public class KnightTests
    {
      
        [Test]
        public void GetKnight_On_GivenMove_Returns_True()
        {
            // Arrange 
            var moveObject = new MoveObject(); 
            var board = new Board();    
            var piece = new Piece();
            var pieceStructure = new PieceBase();  
            var sut = new Engine(moveObject, board, piece);

            piece.Name = "N";

            int startsquare = 0;
            int endsquare = 0;

            // Act 



            // Assert
        }
    }
}
