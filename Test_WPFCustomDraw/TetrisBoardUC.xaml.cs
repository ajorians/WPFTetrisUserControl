using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test_WPFCustomDraw
{
   /// <summary>
   /// Interaction logic for TetrisBoardUC.xaml
   /// </summary>
   public partial class TetrisBoardUC : UserControl
   {
      protected TetrisBoard m_Board;
      protected int m_nPieceSize = 0;
      protected System.Windows.Threading.DispatcherTimer dispatcherTimer;
      protected bool m_bPaused = false;
      public TetrisBoardUC()
      {
         InitializeComponent();
         NewGame();
      }

      private void NewGame()
      {
         m_bPaused = false;
         m_Board = new TetrisBoard( 10, 20 );
         if( dispatcherTimer != null )
         {
            dispatcherTimer.Stop();
         }
         dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
         dispatcherTimer.Tick += new EventHandler( TimeEvent_Elapsed );
         dispatcherTimer.Interval = new TimeSpan( 0, 0, 0, 0, 500 );
         dispatcherTimer.Start();
      }

      private void TimeEvent_Elapsed( object sender, EventArgs e )
      {
         if( !m_bPaused )
            m_Board.Step();
         this.InvalidateVisual();
      }

      protected override void OnRenderSizeChanged( SizeChangedInfo sizeInfo )
      {
         base.OnRenderSizeChanged( sizeInfo );

         int nPieceWidth = (int)sizeInfo.NewSize.Width / m_Board.GetWidth();
         int nPieceHeight = (int)sizeInfo.NewSize.Height / m_Board.GetHeight();

         int nMinPieceSize = Math.Min(nPieceWidth, nPieceHeight);
         m_nPieceSize = nMinPieceSize;
         this.InvalidateVisual();
      }
      protected override void OnRender( DrawingContext drawingContext )
      {
         base.OnRender( drawingContext );

         drawingContext.DrawRectangle( Brushes.Black, new Pen(), new Rect( 0d, 0d, m_Board.GetWidth() * m_nPieceSize, m_Board.GetHeight() * m_nPieceSize) );
         for(int x = 0; x<m_Board.GetWidth(); x++ )
         {
            for(int y = 0; y<m_Board.GetHeight(); y++ )
            {
               TetrisPieceColor eColor = m_Board.GetPieceAt( x, y );
               if( eColor != TetrisPieceColor.NoPiece )
                  DrawPiece( drawingContext, x, y, eColor );
            }
         }
      }

      protected void DrawPiece( DrawingContext drawingContext, int x, int y, TetrisPieceColor tetrisPieceColor )
      {
         if ( m_nPieceSize <= 0 )
            return;

         int nX = x * m_nPieceSize;
         int nY = ((m_Board.GetHeight()-1) * m_nPieceSize) - y * m_nPieceSize;
         Brush b;
         switch(tetrisPieceColor)
         {
            default:
            case TetrisPieceColor.Red:
               b = Brushes.Red;
               break;
            case TetrisPieceColor.Yellow:
               b = Brushes.Yellow;
               break;
            case TetrisPieceColor.Purple:
               b = Brushes.Purple;
               break;
            case TetrisPieceColor.LtGray:
               b = Brushes.LightGray;
               break;
            case TetrisPieceColor.LtBlue:
               b = Brushes.LightBlue;
               break;
            case TetrisPieceColor.Green:
               b = Brushes.Green;
               break;
            case TetrisPieceColor.Blue:
               b = Brushes.Blue;
               break;
         }
         drawingContext.DrawRectangle( b, new Pen( Brushes.Black, 1d ), new Rect( nX, nY, m_nPieceSize, m_nPieceSize ) );
      }

      protected override void OnKeyDown( KeyEventArgs e )
      {
         base.OnKeyDown( e );
         switch(e.Key)
         {
            case Key.Left:
               m_Board.MovePiece( TetrisMoveDirection.Left );
               break;
            case Key.Right:
               m_Board.MovePiece( TetrisMoveDirection.Right );
               break;
            case Key.Up:
               m_Board.MovePiece( TetrisMoveDirection.Flip );
               break;
            case Key.Down:
               m_Board.MovePiece( TetrisMoveDirection.Drop );
               break;
            case Key.F2:
               NewGame();
               break;
            case Key.F3:
            case Key.Pause:
               m_bPaused = !m_bPaused;
               break;
         }
         this.InvalidateVisual();
      }
   }
}
