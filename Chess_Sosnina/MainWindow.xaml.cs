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
using Chess_Sosnina.Classes;

namespace Chess_Sosnina
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<King> Kings = new List<King>();

        public List<Pawn> Pawns = new List<Pawn>();
        public static MainWindow init;



        public MainWindow()
        {
            InitializeComponent();
            init = this;

            for(int i = 0; i <= 7; i ++)
            {
                Pawns.Add(new Pawn(i, 1, false));
                Pawns.Add(new Pawn(i, 6, true));
            }

            Kings.Add(new King(4, 0, false)); 
            Kings.Add(new King(4, 7, true));

            CreateFigures();
        }

        public void CreateFigures()
        {
            foreach(Pawn Pawn in Pawns)
            {
                Pawn.Figure = new Grid()
                {
                    Width = 50,
                    Height = 50
                };
                if (Pawn.Black)
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                else
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));

                Grid.SetColumn(Pawn.Figure, Pawn.X);
                Grid.SetRow(Pawn.Figure, Pawn.Y);
                Pawn.Figure.MouseDown += Pawn.SelectFigure;
                gameBoard.Children.Add(Pawn.Figure);
            }

            foreach (King king in Kings)
            {
                king.Figure = new Grid()
                {
                    Width = 50,
                    Height = 50
                };

                if (king.Black)
                    king.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/King (black).png")));
                else
                    king.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/King.png")));

                Grid.SetColumn(king.Figure, king.X);
                Grid.SetRow(king.Figure, king.Y);
                king.Figure.MouseDown += king.SelectFigure;
                gameBoard.Children.Add(king.Figure);
            }
        }

        private void SelectTile(object sender, MouseButtonEventArgs e)
        {
            ClearHighlights();



            Grid Tile = sender as Grid;
            int X = Grid.GetColumn(Tile);
            int Y = Grid.GetRow(Tile);

            Pawn SelectPawn = Pawns.Find(x => x.Select == true);



            //ищем выдел. короля
            King SelectKing = Kings.Find(x => x.Select == true);



            if (SelectPawn != null)
            {
                SelectPawn.Transform(X, Y);
            }


            //если выбран двигаем 
            else if (SelectKing != null)
            {
                SelectKing.Transform(X, Y);
            }


        }

        public void OnSelect(Pawn pawn)
        {
            //убираем выделение с др пешек
            foreach (Pawn currentPawn in Pawns)
            {
                if (currentPawn != pawn && currentPawn.Select)
                    currentPawn.SelectFigure(null, null);
            }

            //убираем выделение с королей
            foreach (King currentKing in Kings)
            {
                // Тут мы сравниваем через object, чтобы не было ошибки типов
                if ((object)currentKing != (object)pawn && currentKing.Select)
                    currentKing.SelectFigure(null, null);
            }
        }

        //при выбранном короле
        public void OnSelectKing(King king)
        {
            //убираем выделение с пешек
            foreach (Pawn currentPawn in Pawns)
            {
                if ((object)currentPawn != (object)king && currentPawn.Select)
                    currentPawn.SelectFigure(null, null);
            }

            //убираем выделение с др королей
            foreach (King currentKing in Kings)
            {
                if (currentKing != king && currentKing.Select)
                    currentKing.SelectFigure(null, null);
            }
        }







        //сброс подсветки
        public void ClearHighlights()
        {
            foreach (var item in gameBoard.Children)
            {
                if (item is Grid cell)
                {
                    //клетка доски
                    if (cell.Background is SolidColorBrush)
                    {
                        int x = Grid.GetColumn(cell);
                        int y = Grid.GetRow(cell);

                        //возвращаем цвет
                        if ((x + y) % 2 == 0)
                            cell.Background = Brushes.White;
                        else
                            cell.Background = (Brush)new BrushConverter().ConvertFrom("#FF9A6400");
                    }
                }
            }
        }

        //закрашивание одной клетки
        public void HighlightTile(int x, int y, Brush color)
        {
            foreach (var item in gameBoard.Children)
            {
                if (item is Grid cell)
                {
                    //только клетки доски
                    if (cell.Background is SolidColorBrush)
                    {
                        if (Grid.GetColumn(cell) == x && Grid.GetRow(cell) == y)
                        {
                            cell.Background = color;
                            return;
                        }
                    }
                }
            }
        }
    }
}
