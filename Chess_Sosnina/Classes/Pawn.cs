using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_Sosnina.Classes
{
    public class Pawn
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public bool Select = false;
        public bool Black = false;
        public Grid Figure { get; set; }
        public Pawn(int X, int Y, bool Black)
        {
            this.X = X;
            this.Y = Y; 
            this.Black = Black;
        }

        public void SelectFigure(object sender, MouseButtonEventArgs e)
        {
            Pawn SelectPawn = MainWindow.init.Pawns.Find(x => x.Select == true);
            if (SelectPawn != null)
            {
                if (Black && Y - 1 == SelectPawn.Y && (X == SelectPawn.X - 1 || X == SelectPawn.X + 1) ||
                    !Black && Y + 1 == SelectPawn.Y && (X == SelectPawn.X - 1 || X == SelectPawn.X + 1)) {

                    MainWindow.init.gameBoard.Children.Remove(Figure);
                    Grid.SetColumn(SelectPawn.Figure, X);
                    Grid.SetRow(SelectPawn.Figure, Y);

                    SelectPawn.X = X;
                    SelectPawn.Y = Y;

                    SelectPawn.SelectFigure(null, null);
                    return;
                }
            }

            MainWindow.init.OnSelect(this);
            if(Select)
            {
                if (Black)
                    Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                else
                    Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));
                Select = false;
            }
            else
            {
                Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (select).png")));
                Select = true;
            }
        }
    
        
        public void Transform(int X, int Y)
        {
            if (X != this.X)
            {
                SelectFigure(null, null);
                return;
            }

            if(Black && ((this.Y == 6 && this.Y - 2 == Y) || this.Y - 1 == Y) ||
                !Black && ((this.Y == 1 && this.Y + 2 == Y) || this.Y + 1 == Y))
            {
                Grid.SetColumn(Figure, X);
                Grid.SetRow(Figure, Y);

                this.X = X;
                this.Y = Y;


            }
            SelectFigure(null, null);
        }
    
    }
}
