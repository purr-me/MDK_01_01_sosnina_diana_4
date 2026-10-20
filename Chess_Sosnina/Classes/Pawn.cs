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
            //проверка выбранного короля
            King selectedKing = MainWindow.init.Kings.Find(k => k.Select == true);
            if (selectedKing != null && selectedKing.Black != this.Black)
            {
                //если выбран и он вражеский
                int deltaX = Math.Abs(selectedKing.X - this.X);
                int deltaY = Math.Abs(selectedKing.Y - this.Y);

                //если стоит рядом то рубит пешку
                if (deltaX <= 1 && deltaY <= 1 && (deltaX + deltaY) > 0)
                {
                    //убираем пешку
                    MainWindow.init.gameBoard.Children.Remove(this.Figure);
                    MainWindow.init.Pawns.Remove(this);

                    //занмаем место пешки
                    Grid.SetColumn(selectedKing.Figure, this.X);
                    Grid.SetRow(selectedKing.Figure, this.Y);
                    selectedKing.X = this.X;
                    selectedKing.Y = this.Y;

                    selectedKing.SelectFigure(null, null);
                    return; 
                }
            }



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


                MainWindow.init.ClearHighlights();
            }
            else
            {
                Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (select).png")));
                Select = true;


                ShowHighlights();
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






        public void ShowHighlights()
        {
            MainWindow.init.ClearHighlights();
            int dir = this.Black ? -1 : 1; //черные вверх (-1) белые вниз (+1)
            int nextY = this.Y + dir;

            //шаг вперед если пусто
            if (nextY >= 0 && nextY < 8)
            {
                Pawn p1 = MainWindow.init.Pawns.Find(p => p.X == this.X && p.Y == nextY);
                King k1 = MainWindow.init.Kings.Find(k => k.X == this.X && k.Y == nextY);

                if (p1 == null && k1 == null) //путсая клетка
                {
                    MainWindow.init.HighlightTile(this.X, nextY, Brushes.LightGreen);

                    //первый ход (2 клетки)
                    if ((this.Black && this.Y == 6) || (!this.Black && this.Y == 1))
                    {
                        int nextY2 = this.Y + (dir * 2);
                        Pawn p2 = MainWindow.init.Pawns.Find(p => p.X == this.X && p.Y == nextY2);
                        King k2 = MainWindow.init.Kings.Find(k => k.X == this.X && k.Y == nextY2);

                        if (p2 == null && k2 == null)
                            MainWindow.init.HighlightTile(this.X, nextY2, Brushes.LightGreen);
                    }
                }
            }

            //атака по диагонали если есть враг
            int[] dxs = { -1, 1 };
            foreach (int dx in dxs)
            {
                int attackX = this.X + dx;
                if (attackX >= 0 && attackX < 8 && nextY >= 0 && nextY < 8)
                {
                    Pawn enemyPawn = MainWindow.init.Pawns.Find(p => p.X == attackX && p.Y == nextY && p.Black != this.Black);
                    King enemyKing = MainWindow.init.Kings.Find(k => k.X == attackX && k.Y == nextY && k.Black != this.Black);

                    if (enemyPawn != null || enemyKing != null)
                        MainWindow.init.HighlightTile(attackX, nextY, Brushes.LightCoral);
                }
            }
        }



    }
}
