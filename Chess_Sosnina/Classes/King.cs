using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_Sosnina.Classes
{
    public class King
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Select = false;
        public bool Black = false;
        public Grid Figure { get; set; }

        public King(int X, int Y, bool Black)
        {
            this.X = X;
            this.Y = Y;
            this.Black = Black;
        }


        public void SelectFigure(object sender, MouseButtonEventArgs e)
        {
            MainWindow.init.OnSelectKing(this);

            //выбран - снимаем выделение
            if (this.Select)
            {
                if (this.Black)
                    this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/King (black).png")));
                else
                    this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/King.png")));

                this.Select = false;


                MainWindow.init.ClearHighlights();
            }
            //не выбран - выделяем
            else
            {
                this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/King (select).png")));

                this.Select = true;


                ShowHighlights();
            }
        }

        public void Transform(int newX, int newY)
        {
            int deltaX = Math.Abs(this.X - newX);
            int deltaY = Math.Abs(this.Y - newY);

            if (deltaX <= 1 && deltaY <= 1 && (deltaX + deltaY) > 0)
            {
                //бьем врага если есть
                Pawn enemy = MainWindow.init.Pawns.Find(p => p.X == newX && p.Y == newY && p.Black != this.Black);
                if (enemy != null)
                {
                    MainWindow.init.gameBoard.Children.Remove(enemy.Figure);
                    MainWindow.init.Pawns.Remove(enemy);
                }

                //передвигаем
                Grid.SetColumn(this.Figure, newX);
                Grid.SetRow(this.Figure, newY);
                this.X = newX;
                this.Y = newY;

                //шах
                bool isCheck = false;
                foreach (Pawn p in MainWindow.init.Pawns)
                {
                    if (p.Black != this.Black && Math.Abs(p.X - newX) == 1)
                    {
                        if (!this.Black && p.Y == newY + 1) isCheck = true;
                        if (this.Black && p.Y == newY - 1) isCheck = true;
                    }
                }

                if (isCheck)
                {
                    MessageBox.Show("шаах");
                }
            }

            SelectFigure(null, null);
        }





        public void ShowHighlights()
        {
            MainWindow.init.ClearHighlights(); //чистим

            //8 клеток вокруг
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue; //под собой скип

                    int newX = this.X + dx;
                    int newY = this.Y + dy;

                    //в пределах доски
                    if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
                    {
                        //ищем врага на этой клетке
                        Pawn enemyPawn = MainWindow.init.Pawns.Find(p => p.X == newX && p.Y == newY && p.Black != this.Black);
                        King enemyKing = MainWindow.init.Kings.Find(k => k.X == newX && k.Y == newY && k.Black != this.Black);

                        if (enemyPawn != null || enemyKing != null)
                            MainWindow.init.HighlightTile(newX, newY, Brushes.LightCoral); //красный
                        else
                            MainWindow.init.HighlightTile(newX, newY, Brushes.LightGreen); //зеленый
                    }
                }
            }
        }


    }
}
