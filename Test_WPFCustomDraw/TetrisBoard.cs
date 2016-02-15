using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_WPFCustomDraw
{
   public enum TetrisPieceColor
   {
      NoPiece,
      Red,
      Yellow,
      Purple,
      LtGray,
      LtBlue,
      Green,
      Blue
   }
   public enum TetrisMoveDirection
   {
      Left,
      Right,
      Flip,
      Drop
   }
   //public enum TetrisPieceFlip
   //{
   //   Normal,
   //   Flip90,
   //   Flip180,
   //   Flip270
   //}
   public class TetrisPiece
   {
      protected TetrisBoard m_Board;
      protected TetrisPieceColor m_eColor;
      protected int m_nX, m_nY;
      //protected TetrisPieceFlip m_eFlip;
      protected bool[,] m_PieceBoard;
      public TetrisPiece( TetrisBoard board, TetrisPieceColor eColor )
      {
         m_Board = board;
         m_eColor = eColor;
         m_nX = board.GetWidth() / 2;
         m_nY = board.GetHeight();
         //m_eFlip = TetrisPieceFlip.Normal;
         m_PieceBoard = new bool[4, 4];
         for(int x=0; x<4; x++ )
         {
            for(int y=0; y<4; y++ )
            {
               m_PieceBoard[x, y] = false;
            }
         }
         if ( m_eColor == TetrisPieceColor.Red )
         {
            m_PieceBoard[0, 0] = true;
            m_PieceBoard[1, 0] = true;
            m_PieceBoard[2, 0] = true;
            m_PieceBoard[3, 0] = true;
         }
         else if( m_eColor == TetrisPieceColor.Yellow )
         {
            m_PieceBoard[0, 0] = true;
            m_PieceBoard[1, 0] = true;
            m_PieceBoard[2, 0] = true;
            m_PieceBoard[2, 1] = true;
         }
         else if ( m_eColor == TetrisPieceColor.Purple )
         {
            m_PieceBoard[1, 1] = true;
            m_PieceBoard[1, 0] = true;
            m_PieceBoard[2, 0] = true;
            m_PieceBoard[3, 0] = true;
         }
         else if ( m_eColor == TetrisPieceColor.LtGray )
         {
            m_PieceBoard[1, 0] = true;
            m_PieceBoard[1, 1] = true;
            m_PieceBoard[2, 0] = true;
            m_PieceBoard[3, 0] = true;
         }
         else if ( m_eColor == TetrisPieceColor.LtBlue )
         {
            m_PieceBoard[0, 0] = true;
            m_PieceBoard[0, 1] = true;
            m_PieceBoard[1, 0] = true;
            m_PieceBoard[1, 1] = true;
         }
         else if( m_eColor == TetrisPieceColor.Green )
         {
            m_PieceBoard[3, 0] = true;
            m_PieceBoard[2, 0] = true;
            m_PieceBoard[2, 1] = true;
            m_PieceBoard[1, 1] = true;
         }
         else
         {
            m_PieceBoard[0, 0] = true;
            m_PieceBoard[1, 0] = true;
            m_PieceBoard[1, 1] = true;
            m_PieceBoard[2, 1] = true;
         }
      }

      public bool HasPieceAt(int nX, int nY)
      {
         if ( nX < 0 || nX >= m_Board.GetWidth() || nY < 0 || nY >= m_Board.GetHeight() )
            return false;

         if( nX >= m_nX && nX < m_nX+4 && nY >= m_nY && nY < m_nY+4 )
         {
            int nOffsetX = nX - m_nX;
            int nOffsetY = nY - m_nY;
            if ( nOffsetX < 0 || nOffsetX >= 4 || nOffsetY < 0 || nOffsetY >= 4 )
               return false;

            if ( m_PieceBoard[nOffsetX, nOffsetY] == true )
               return true;
         }

         return false;
      }

      public TetrisPieceColor GetPieceColor()
      {
         return m_eColor;
      }

      // Return true if piece is still in play
      // Return false when it is now out of play
      public bool Step()
      {
         if ( m_nY <= 0 )
            return false;

         //Check to see if can move down
         for(int x=0; x<4; x++ )
         {
            for(int y=0; y<4; y++ )
            {
               if( m_PieceBoard[x,y] == true )
               {
                  if ( m_Board.GetBoardPieceAt( m_nX + x, m_nY + y - 1 ) != TetrisPieceColor.NoPiece )
                     return false;
               }
            }
         };

         m_nY--;
         return true;
      }

      public void Move( TetrisMoveDirection eDirection )
      {
         switch(eDirection)
         {
            case TetrisMoveDirection.Left:
               AttemptMoveLeft();
               break;
            case TetrisMoveDirection.Right:
               AttemptMoveRight();
               break;
            case TetrisMoveDirection.Flip:
               AttemptFlip();
               break;
            case TetrisMoveDirection.Drop:
               DropPiece();
               break;
         }
      }

      private void AttemptMoveLeft()
      {
         for ( int x = 0; x < 4; x++ )
         {
            for ( int y = 0; y < 4; y++ )
            {
               if ( m_PieceBoard[x, y] == true )
               {
                  int nNewX = m_nX + x - 1;
                  if ( nNewX < 0 || nNewX >= m_Board.GetWidth() )
                     return;

                  if ( m_Board.GetBoardPieceAt( nNewX, m_nY + y ) != TetrisPieceColor.NoPiece )
                     return;
               }
            }
         }
         m_nX--;
      }

      private void AttemptMoveRight()
      {
         for ( int x = 0; x < 4; x++ )
         {
            for ( int y = 0; y < 4; y++ )
            {
               if ( m_PieceBoard[x, y] == true )
               {
                  int nNewX = m_nX + x + 1;
                  if ( nNewX < 0 || nNewX >= m_Board.GetWidth() )
                     return;

                  if ( m_Board.GetBoardPieceAt( nNewX, m_nY + y ) != TetrisPieceColor.NoPiece )
                     return;
               }
            }
         }
         m_nX++;
      }

      private void DropPiece()
      {
         while( Step() )
         {

         }
      }

      private void AttemptFlip()
      {
         for ( int x = 0; x < 4; x++ )
         {
            for ( int y = 0; y < 4; y++ )
            {
               if ( m_PieceBoard[x, y] == true )
               {
                  int nNewX = m_nX + y;
                  int nNewY = m_nY + x;
                  if ( nNewX < 0 || nNewX >= m_Board.GetWidth() || nNewY < 0 )
                     return;

                  if ( m_Board.GetBoardPieceAt( nNewX, nNewY ) != TetrisPieceColor.NoPiece )
                     return;
               }
            }
         }

         bool[,] bArray = new bool[4,4];

         for ( int x = 0; x < 4; x++ )
         {
            for ( int y = 0; y < 4; y++ )
            {
               bArray[y, x] = m_PieceBoard[x, y];
            }
         }
         m_PieceBoard = bArray;
      }

      public void ApplyYourSelfToBoard()
      {
         for ( int x = 0; x < 4; x++ )
         {
            for ( int y = 0; y < 4; y++ )
            {
               if ( m_PieceBoard[x, y] == true )
               {
                  m_Board.SetPieceAt( m_nX + x, m_nY + y, m_eColor );
               }
            }
         }
      }
   }
   public class TetrisBoard
   {
      protected int m_nWidth, m_nHeight;
      protected TetrisPieceColor[,] m_Board;
      protected TetrisPiece m_Piece = null;
      public TetrisBoard(int nWidth, int nHeight)
      {
         m_nWidth = nWidth;
         m_nHeight = nHeight;
         m_Board = new TetrisPieceColor[nWidth, nHeight];
         for(int x=0; x<m_nWidth; x++ )
         {
            for(int y=0; y<m_nHeight; y++ )
            {
               m_Board[x, y] = TetrisPieceColor.NoPiece;
            }
         }
      }

      public TetrisPieceColor GetBoardPieceAt( int nX, int nY )
      {
         if ( nX < 0 || nX >= m_nWidth || nY < 0 || nY >= m_nHeight )
            return TetrisPieceColor.NoPiece;

         TetrisPieceColor eColor = m_Board[nX, nY];
         return eColor;
      }

      public TetrisPieceColor GetPieceAt(int nX, int nY)
      {
         TetrisPieceColor eColor = GetBoardPieceAt(nX, nY);
         if( eColor == TetrisPieceColor.NoPiece && m_Piece != null )
         {
            if ( m_Piece.HasPieceAt( nX, nY ) )
               eColor = m_Piece.GetPieceColor();
         }
         return eColor;
      }

      public void Step()
      {
         if( m_Piece == null )
         {
            Random r = new Random();
            TetrisPieceColor eColor = ( TetrisPieceColor)r.Next((int)TetrisPieceColor.Red, (int)TetrisPieceColor.Blue );
            m_Piece = new TetrisPiece(this, eColor);
         }
         else
         {
            if( !m_Piece.Step() )
            {
               //Add to board
               m_Piece.ApplyYourSelfToBoard();
               m_Piece = null;

               //Check Rows to see if can clear anything
               DoClearRowCheck();
            }
         }
      }

      private void DoClearRowCheck()
      {
         for(int y=m_nHeight-1; y-->0; )
         {
            bool bFullRow = true;
            for(int x = 0; x<m_nWidth; x++ )
            {
               if( m_Board[x, y] == TetrisPieceColor.NoPiece )
               {
                  bFullRow = false;
                  break;
               }
            }

            if( bFullRow )
            {
               ClearRowAndDrop( y );
            }
         }
      }

      private void ClearRowAndDrop( int y )
      {
         if ( y < 0 || y >= m_nHeight )
            return;

         for(int x=0; x<m_nWidth; x++ )
         {
            for(int nY = y; nY < m_nHeight-1; nY++ )
            {
               m_Board[x, nY] = m_Board[x, nY + 1];
            }
            m_Board[x, m_nHeight - 1] = TetrisPieceColor.NoPiece;
         }
      }

      public void MovePiece(TetrisMoveDirection eDirection)
      {
         if ( m_Piece == null )
            return;
         m_Piece.Move( eDirection );
      }

      public int GetHeight()
      {
         return m_nHeight;
      }

      public int GetWidth()
      {
         return m_nWidth;
      }

      public void SetPieceAt( int nX, int nY, TetrisPieceColor eColor )
      {
         if ( nX < 0 || nX >= m_nWidth || nY < 0 || nY >= m_nHeight )
            return;

         m_Board[nX, nY] = eColor;
      }
   }
}
