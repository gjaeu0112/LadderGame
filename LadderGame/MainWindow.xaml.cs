using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LadderGame
{
    public partial class MainWindow : Window
    {
        int PeopleNum;
        int LadderTimes;
        List<int[]> LadderResultSaver;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            ClearLadderGameResult();
            StartLadderGame(int.Parse(PeopleNumberBox.Text.ToString()));
        }
        private void ClearLadderGameResult()
        {
            ButtonDatas.Clear();
            StartPoint.Children.Clear();
            EndPoint.Children.Clear();
            LadderDrawingPaper.Children.Clear();
        }
        private void StartLadderGame(int peopleNum)
        {
            Random random = new Random();
            int[] arr = new int[peopleNum];
            PeopleNum = peopleNum;

            for (int i = 0; i < peopleNum; i++)
            {
                arr[i] = i;
                StartOrEndPointGenerate(true, i);
                DrawVerticalLine(i, 0, LadderDrawingPaper.ActualHeight, Brushes.Black);
            }

            List<int[]> ladderResultSaver = new List<int[]> { arr };
            int ladderTimes = random.Next(peopleNum, peopleNum * peopleNum);
            LadderTimes = ladderTimes;
            for (int i = 0; i < ladderTimes; i++)
            {
                int[] brr = new int[peopleNum];
                if (i == 0)
                {
                    for (int k = 0; k < peopleNum; k++)
                    {
                        brr[k] = arr[k];
                    }
                }
                else
                {
                    for (int k = 0; k < peopleNum; k++)
                    {
                        brr[k] = ladderResultSaver[i][k];
                    }
                }
                int randomNumber = random.Next();
                int randomDirection = random.Next() % 2;
                int swapIndex = randomNumber % peopleNum;

                if (swapIndex == 0)
                {
                    randomDirection = 0;
                }

                if (swapIndex >= peopleNum - 1)
                {
                    randomDirection = 1;
                }

                if ((swapIndex == 0) || (randomDirection == 0 && swapIndex < peopleNum - 1))
                {
                    int num_swap = brr[swapIndex];
                    brr[swapIndex] = brr[swapIndex + 1];
                    brr[swapIndex + 1] = num_swap;
                }
                else
                {
                    int num_swap = brr[swapIndex];
                    brr[swapIndex] = brr[swapIndex - 1];
                    brr[swapIndex - 1] = num_swap;
                }
                Console.WriteLine("swap point = {0}, To = {1}", swapIndex + 1, randomDirection == 0 ? swapIndex + 2 : swapIndex);
                DrawHorizontalLine(swapIndex, i, randomDirection, Brushes.Black);

                ladderResultSaver.Add(brr);
            }

            for (int i = 0; i < peopleNum; i++)
            {
                //Console.WriteLine("arr[{0}] = {1}", i, arr[i]);
                StartOrEndPointGenerate(false, ladderResultSaver[ladderTimes][i]);
            }
            LadderResultSaver = ladderResultSaver;
        }

        //private void TracePath(int startIndex)
        //{
        //    if (LadderResultSaver is null)
        //    {
        //        throw new ArgumentNullException(nameof(LadderResultSaver));
        //    }

        //    int j = 0;
        //    double length = RootGrid.ActualWidth / PeopleNum;
        //    double height = LadderTracingPaper.ActualHeight / LadderTimes;
        //    SolidColorBrush solidColorBrush = (SolidColorBrush)ButtonDatas[startIndex].color;
        //    DrawVerticalLine(startIndex, 0, solidColorBrush, false);
        //    TracingPathStart.StartPoint.Offset(length * (startIndex + 0.5), height * 0.5);

        //    foreach (int[] currentArr in LadderResultSaver)
        //    {
        //        if (currentArr[startIndex] == LadderResultSaver[j + 1][startIndex])
        //        {
        //            DrawVerticalLine(startIndex, height * j, solidColorBrush, true);
        //            DrawVerticalLine(startIndex, height * (j + 1), solidColorBrush, false);
        //        }
        //        else if (startIndex < PeopleNum - 1 && currentArr[startIndex] == LadderResultSaver[j + 1][startIndex + 1])
        //        {
        //            DrawHorizontalLine(startIndex, j, 0, solidColorBrush, true);
        //            startIndex += 1;
        //            DrawVerticalLine(startIndex, height * (j + 0.5), solidColorBrush, true);
        //        }
        //        else if (startIndex > 0 && currentArr[startIndex] == LadderResultSaver[j + 1][startIndex - 1])
        //        {
        //            DrawHorizontalLine(startIndex, j, 1, solidColorBrush, true);
        //            startIndex -= 1;
        //            DrawVerticalLine(startIndex, height * (j + 0.5), solidColorBrush, true);
        //        }
        //        else return;
        //        j++;
        //        if (LadderResultSaver.Count == j + 1) break;
        //    }
        //}

        private void TracePath(int startIndex)
        {
            if (LadderResultSaver is null)
            {
                throw new ArgumentNullException(nameof(LadderResultSaver));
            }

            double length = RootGrid.ActualWidth / PeopleNum;
            double height = LadderTracingPaper.ActualHeight / LadderTimes;
            TracingPath.Stroke = (SolidColorBrush)ButtonDatas[startIndex].color;
            TracingPath.StrokeThickness = 4;

            TracingPathStart.StartPoint = new Point(length * (startIndex + 0.5), 0);

            int j = 0;
            foreach (int[] currentArr in LadderResultSaver)
            {
                if (currentArr[startIndex] == LadderResultSaver[j + 1][startIndex]) // 위치가 변하지 않아, 수직으로 내려갈 때
                {
                    TracingPathStart.Segments.Add(new LineSegment(new Point(length * (startIndex + 0.5), height * (j + 0.5)), true));
                }
                else if (startIndex < PeopleNum - 1 && currentArr[startIndex] == LadderResultSaver[j + 1][startIndex + 1]) // 우측으로 이동했을 때
                {
                    startIndex += 1;
                    TracingPathStart.Segments.Add(new LineSegment(new Point(length * (startIndex + 0.5), height * (j - 0.5)), true)); //오른쪽으로 먼저 꺾고
                    TracingPathStart.Segments.Add(new LineSegment(new Point(length * (startIndex + 0.5), height * (j + 0.5)), true)); //아래로 한칸 내려가기
                }
                else if (startIndex > 0 && currentArr[startIndex] == LadderResultSaver[j + 1][startIndex - 1]) // 좌측으로 이동했을 떄
                {
                    startIndex -= 1;
                    TracingPathStart.Segments.Add(new LineSegment(new Point(length * (startIndex + 0.5), height * (j - 0.5)), true)); //오른쪽으로 먼저 꺾고
                    TracingPathStart.Segments.Add(new LineSegment(new Point(length * (startIndex + 0.5), height * (j + 0.5)), true)); //아래로 한칸 내려가기
                }
                else return;
                j++;
                if (LadderResultSaver.Count == j + 1)
                {
                    TracingPathStart.Segments.Add(new LineSegment(new Point(length * (startIndex + 0.5), height * (j + 1)), true)); //아래로 반칸 내려가기
                    break;
                }
            }
        }


        private void DrawHorizontalLine(int timesX, int timesY, int direction, Brush brushColor)
        {
            double length = RootGrid.ActualWidth / PeopleNum;
            double height = LadderDrawingPaper.ActualHeight / LadderTimes;
            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line
            {
                X1 = length * (timesX + 0.5),
                X2 = direction == 0 ? length * (timesX + 1 + 0.5) : length * (timesX - 1 + 0.5),
                Y1 = height * (timesY + 0.5),
                Y2 = height * (timesY + 0.5),
                Stroke = brushColor,
                StrokeThickness = 2
            };
            LadderDrawingPaper.Children.Add(line);
        }
        private void DrawVerticalLine(int timesX, double startY, double timesY, Brush brushColor)
        {
            double length = RootGrid.ActualWidth / PeopleNum;
            double height = LadderDrawingPaper.ActualHeight;
            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line
            {
                X1 = length * (timesX + 0.5),
                X2 = length * (timesX + 0.5) - 1,
                Y1 = startY,
                Y2 = startY + height * timesY,
                Stroke = brushColor,
                StrokeThickness = 2
            };
            LadderDrawingPaper.Children.Add(line);
        }
        //private void DrawHorizontalLine(int xIndex, int yIndex, int direction, Brush brushColor, bool mod)
        //{
        //    double length = RootGrid.ActualWidth / PeopleNum;
        //    double height = LadderTracingPaper.ActualHeight / LadderTimes;
        //    System.Windows.Shapes.Line line = new System.Windows.Shapes.Line
        //    {
        //        X1 = length * (xIndex + 0.5),
        //        X2 = length * (direction == 0 ? xIndex + 1 + 0.5 : xIndex - 1 + 0.5),
        //        Y1 = height * (yIndex + 0.5),
        //        Y2 = height * (yIndex + 0.5),
        //        Stroke = brushColor,
        //        StrokeThickness = 4
        //    };
        //    LadderTracingPaper.Children.Add(line);
        //}
        //private void DrawVerticalLine(int xIndex, double startY, Brush brushColor, bool mod)
        //{
        //    double length = RootGrid.ActualWidth / PeopleNum;
        //    double height = LadderTracingPaper.ActualHeight / LadderTimes;
        //    System.Windows.Shapes.Line line = new System.Windows.Shapes.Line
        //    {
        //        X1 = length * (xIndex + 0.5),
        //        X2 = length * (xIndex + 0.5),
        //        Y1 = startY,
        //        Y2 = startY + (mod ? height : height * 0.5),
        //        Stroke = brushColor,
        //        StrokeThickness = 4
        //    };
        //    LadderTracingPaper.Children.Add(line);
        //}

        public List<ButtonData> ButtonDatas = new List<ButtonData>();
        Random rnd = new Random();
        private void StartOrEndPointGenerate(bool mod, int num)
        {
            Button button = new Button();
            button.Click += Button_Click;
            button.Content = (num + 1).ToString();
            Thickness thickness = new Thickness(1, 1, 1, 1);
            button.Margin = thickness;
            int idx = int.Parse(button.Content.ToString()) - 1;
            int col1 = rnd.Next(256);
            int col2 = rnd.Next(256);
            int col3 = rnd.Next(256);
            ButtonData data = new ButtonData()
            {
                name = "",
                index = idx,
                color = new SolidColorBrush(Color.FromArgb((byte)col1, (byte)col2, (byte)col3, 00)),
            };
            ButtonDatas.Add(data);
            button.DataContext = data;
            if (mod)
            {
                StartPoint.Children.Add(button);
            }
            else
            {
                EndPoint.Children.Add(button);
            }
        }

        public class ButtonData
        {
            public string name;
            public int index;
            public Brush color;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonData temp = (sender as Button).DataContext as ButtonData;
            LadderTracingPaper.Children.Clear();
            TracePath(temp.index);
        }

        private void ClearTrace_Click(object sender, RoutedEventArgs e)
        {
//            LadderTracingPaper.Children.Clear();
        }

    }
}
