using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class MyMap
    {
        private static List<int> directionRow = new List<int> { -1, 1, 0, 0 };
        private static List<int> directionColumn = new List<int> { 0, 0, 1, -1 };

        private MyCell[,] map;

        internal MyCell[,] Map { get => map; set => map = value; }

        public int NumberOfXLines => Map.GetLength(0);
        public int NumberOfYLines => Map.GetLength(1);

        public MyMap(int Xlength, int Ylength)
        {
            this.map = new MyCell[Xlength, Ylength];
        }

        public void AddCell(double X, double Y, Spot spot)
        {
            for (int i = 0; i < NumberOfYLines; i++)
            {
                if (X == Map[i, 0].X_coordinates)
                {
                    for (int j = 0; j < NumberOfXLines; j++)
                    {
                        if (Y == Map[i, j].Y_coordinates)
                        {
                            Map[i, j].Spot = spot;
                            break;
                        }
                    }
                }
            }
        }

        public bool End(int x, int y, MyCell test)
        {
            if (x == test.X && y == test.Y)
                return true;
            return false;
        }

        private int findRow(double xCoord)
        {
            for (int x = 0; x < NumberOfYLines; x++)
            {
                if (xCoord == Map[x, 0].X_coordinates)
                {
                    return x;
                }
            }
            return -1;
        }

        private int findColumn(double yCoord)
        {
            for (int y = 0; y < NumberOfYLines; y++)
            {
                if (yCoord == Map[0, y].Y_coordinates)
                {
                    return y;
                }
            }
            return -1;
        }

        //BFS
        public List<MyCell> createLine(double StartX, double StartY, double StopX, double StopY, int flag)
        {
            List<MyCell> shortest = new List<MyCell>();
            Queue<MyCell> queue = new Queue<MyCell>();
            MyCell[,] previous = new MyCell[NumberOfXLines, NumberOfYLines];

            int StartRow = findRow(StartX);
            int StopRow = findColumn(StopX);
            int StartColumn = findRow(StartY);
            int StopColumn = findColumn(StopY);

            MyCell startCell = new MyCell(StartX, StartY, StartRow, StartColumn);
            MyCell stopCell = new MyCell(StopX, StopY, StopRow, StopColumn);

            previous[StartRow, StartColumn] = startCell; //visited
            queue.Enqueue(startCell);    //to search

            bool complitePath = false;

            while (queue.Count > 0)
            {
                MyCell tempCell = queue.Dequeue();
                if (End(tempCell.X, tempCell.Y, stopCell))
                {
                    complitePath = true;
                    break;
                }
                for (int i = 0; i < 4; i++)
                {
                    int nextRow = tempCell.X + directionRow[i];
                    int nextColumn = tempCell.Y + directionColumn[i];
                    if (nextRow < 0 || nextColumn < 0 || nextRow >= NumberOfXLines || nextColumn >= NumberOfYLines)
                    {
                        continue;
                    }
                    if (previous[nextRow, nextColumn] != null)
                    {
                        continue;
                    }
                    if (!End(nextRow, nextColumn, stopCell) && (Map[nextRow, nextColumn].Spot != Spot.FREE) && flag == 0)
                    {
                        continue;
                    }
                    if (!End(nextRow, nextColumn, stopCell) && (Map[nextRow, nextColumn].Spot == Spot.NODE) && flag == 1)
                    {
                        continue;
                    }

                    queue.Enqueue(new MyCell(Map[nextRow, nextColumn].X_coordinates, Map[nextRow, nextColumn].Y_coordinates, nextRow, nextColumn));

                    previous[nextRow, nextColumn] = tempCell;
                }
            }
            if (complitePath)
            {
                shortest.Add(stopCell);
                MyCell prev = previous[stopCell.X, stopCell.Y];
                while (prev.X > 0 && !compareCell(prev, startCell))
                {
                    MarkSpace(prev.X, prev.Y);
                    shortest.Add(prev);
                    prev = previous[prev.X, prev.Y];
                }
                shortest.Add(prev);
            }

            return shortest;
        }

        private bool compareCell(MyCell first, MyCell sec)
        {
            return (first.X == sec.X && first.Y == sec.Y && first.X_coordinates == sec.X_coordinates && first.Y_coordinates == sec.Y_coordinates);
        }

        private void MarkSpace(int x, int y)
        {
            if (Map[x, y].Spot == Spot.FREE)
            {
                Map[x, y].Spot = Spot.LINE;
            }

        }

        public void SetMark(double xcoord, double ycoord, Spot current)
        {
            for (int x = 0; x < NumberOfXLines; x++)
            {
                if (xcoord == map[x, 0].X_coordinates)
                {
                    for (int y = 0; y < NumberOfYLines; y++)
                    {
                        if (ycoord == map[x, y].Y_coordinates)
                        {
                            if (map[x, y].Spot == Spot.NODE)
                                return;
                            if (map[x, y].Spot == Spot.LINE || map[x, y].Spot == Spot.FREE)
                                map[x, y].Spot = current;
                            else if (map[x, y].Spot != current)
                                map[x, y].Spot = Spot.LINE_X;
                            return;
                        }
                    }
                }
            }
        }

    }
}
