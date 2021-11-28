﻿using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Grafer2.CustomControls
{
    /// <summary>
    /// Interaction logic for CoordinateSystem.xaml
    /// </summary>
    public partial class CoordinateSystem : Canvas
    {
        public CoordinateSystem()
        {
            InitializeComponent();
        }

        private enum Direction
        {
            X,
            Y
        }

        private int defaultElementsCount;


        public void Create()
        {
            DrawAxes();
            DrawGrid();
            DrawNumbers();

            defaultElementsCount = Children.Count;
        }

        public void RemoveFunctions()
        {
            Children.RemoveRange(defaultElementsCount, Children.Count - defaultElementsCount);
        }

        private void DrawGrid()
        {
            DrawGridLines(Direction.X, Width);
            DrawGridLines(Direction.Y, Height);
        }

        private void DrawNumbers()
        {
            DrawGridNumbers(Direction.X, Width);
            DrawGridNumbers(Direction.Y, Height);
        }

        private void DrawGridNumbers(Direction direction, double size)
        {
            for (int i = 100; i < size / 2; i += 100)
            {
                switch (direction)
                {
                    case Direction.X:
                        {
                            Children.Add(NewNumber(-i / 100, (size / 2) - i - 7, Height / 2 + 10));
                            Children.Add(NewNumber(i / 100, (size / 2) + i - 3, Height / 2 + 10));
                            break;
                        }

                    case Direction.Y:
                        {
                            Children.Add(NewNumber(i / 100, Width / 2 + 16, (size / 2) - i - 7));
                            Children.Add(NewNumber(-i / 100, Width / 2 + 10, (size / 2) + i - 7));
                            break;
                        }
                }
            }

            if (direction == Direction.X)
            {
                Children.Add(NewNumber(0, Width / 2 + 16, Height / 2 + 10));
            }

        }

        private static TextBlock NewNumber(int value, double x, double y)
        {
            TextBlock number = DefaultTextBlock(value.ToString());

            number.RenderTransform = new TranslateTransform()
            {
                X = x,
                Y = y
            };

            return number;
        }

        private static TextBlock DefaultTextBlock(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontFamily = new FontFamily("Arial"),
                FontSize = 13
            };
        }

        private void DrawGridLines(Direction direction, double size)
        {
            SolidColorBrush brush = new(Color.FromArgb(75, 0, 0, 0));
            for (int i = 100; i < size / 2; i += 100)
            {
                switch (direction)
                {
                    case Direction.X:
                        {
                            Children.Add(NewLine(direction, lineY: (size / 2) - i, brushes: brush));
                            Children.Add(NewLine(direction, lineY: (size / 2) + i, brushes: brush));
                            break;
                        }

                    case Direction.Y:
                        {
                            Children.Add(NewLine(direction, lineX: (size / 2) - i, brushes: brush));
                            Children.Add(NewLine(direction, lineX: (size / 2) + i, brushes: brush));
                            break;
                        }
                }
            }
        }

        private void DrawAxes()
        {
            Line axisX = NewLine(Direction.X, lineY: Height / 2, strokeThickness: 1.5);

            Line axisY = NewLine(Direction.Y, lineX: Width / 2, strokeThickness: 1.5);

            Children.Add(axisX);
            Children.Add(axisY);
        }

        private Line NewLine(Direction direction, double lineX = 0, double lineY = 0, SolidColorBrush? brushes = null, double strokeThickness = 1)
        {
            Line line = new()
            {
                X1 = (direction == Direction.X) ? 0 : lineX,
                Y1 = (direction == Direction.X) ? lineY : 0,
                X2 = (direction == Direction.X) ? Width : lineX,
                Y2 = (direction == Direction.X) ? lineY : Height,
                Stroke = brushes ?? Brushes.Black,
                StrokeThickness = strokeThickness,
            };

            line.SnapsToDevicePixels = true;

            return line;
        }
    }
}