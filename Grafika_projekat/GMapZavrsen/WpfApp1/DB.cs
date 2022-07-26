using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using WpfApp1.Model;

namespace WpfApp1
{
    public class DB
    {
        private static List<PowerEntity> powerEntities;

        private List<SubstationEntity> substationEntities = new List<SubstationEntity>();
        private List<NodeEntity> nodeEntities = new List<NodeEntity>();
        private List<SwitchEntity> switchEntities = new List<SwitchEntity>();
        private List<LineEntity> lineEntities = new List<LineEntity>();

        private static HashSet<(double, double)> usedCoords = new HashSet<(double, double)>();

        private double xMin, yMin;
        private double xToScale, yToScale;
        private MyMap myMap;
        private double size = 10;

        public DB()
        {
            powerEntities = new List<PowerEntity>();
        }

        public static List<PowerEntity> PowerEntities { get => powerEntities; set => powerEntities = value; }

        public void InititalizeDataBase(XmlDocument document)
        {
            foreach (XmlNode node in document.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity"))
            {
                ToLatLon(double.Parse(node.SelectSingleNode("X").InnerText),
                    double.Parse(node.SelectSingleNode("Y").InnerText),
                    34, out double x, out double y);
                substationEntities.Add(new SubstationEntity()
                {
                    X = x,
                    Y = y,
                    Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                    Name = node.SelectSingleNode("Name").InnerText
                });
            }

            foreach (XmlNode node in document.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity"))
            {
                ToLatLon(double.Parse(node.SelectSingleNode("X").InnerText),
                    double.Parse(node.SelectSingleNode("Y").InnerText),
                    34, out double x, out double y);
                nodeEntities.Add(new NodeEntity()
                {
                    X = x,
                    Y = y,
                    Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                    Name = node.SelectSingleNode("Name").InnerText
                });
            }

            foreach (XmlNode node in document.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity"))
            {
                ToLatLon(double.Parse(node.SelectSingleNode("X").InnerText),
                    double.Parse(node.SelectSingleNode("Y").InnerText),
                    34, out double x, out double y);
                switchEntities.Add(new SwitchEntity()
                {
                    X = x,
                    Y = y,
                    Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                    Name = node.SelectSingleNode("Name").InnerText,
                    Status = node.SelectSingleNode("Status").InnerText
                });
            }

            foreach (XmlNode node in document.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity"))
            {
                var line = new LineEntity()
                {
                    Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                    Name = node.SelectSingleNode("Name").InnerText,
                    IsUnderground = bool.Parse(node.SelectSingleNode("IsUnderground").InnerText),
                    R = float.Parse(node.SelectSingleNode("R").InnerText),
                    ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText,
                    LineType = node.SelectSingleNode("LineType").InnerText,
                    ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText),
                    FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText),
                    SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText)
                };

                int exist = 0;
                foreach (LineEntity entity in lineEntities)
                {
                    if (entity.FirstEnd == line.SecondEnd && entity.SecondEnd == line.FirstEnd)
                        exist = 1;
                }
                if (exist == 0)
                    lineEntities.Add(line);
            }
        }

        #region functions

        public void SetToCanvasScale(double width, double height)
        {
            xMin = Math.Min(Math.Min(substationEntities.Min((item) => item.X), nodeEntities.Min((item) => item.X)), switchEntities.Min((item) => item.X));
            yMin = Math.Min(Math.Min(substationEntities.Min((item) => item.Y), nodeEntities.Min((item) => item.Y)), switchEntities.Min((item) => item.Y));

            double xMax = Math.Max(Math.Max(substationEntities.Max((item) => item.X), nodeEntities.Max((item) => item.X)), switchEntities.Max((item) => item.X));
            double yMax = Math.Max(Math.Max(substationEntities.Max((item) => item.Y), nodeEntities.Max((item) => item.Y)), switchEntities.Max((item) => item.Y));

            xToScale = (width / 2) / (xMax - xMin);  //po x
            yToScale = (height / 2) / (yMax - yMin);  //po y

            myMap = new MyMap(401, 401);

            for (int x = 0; x <= 400; x++)
            {
                for (int y = 0; y <= 400; y++)
                {
                    myMap.Map[x, y] = new MyCell(x * size, y * size, Spot.FREE, x, y, null); //posto je canvas 4000x4000 mapa je 400x400 posto je jedno polje 10x10
                }
            }

        }

        public static (double x, double y) FindClosestXY(double x, double y, double size)
        {
            if (!usedCoords.Contains((x, y)))
            {
                usedCoords.Add((x, y));
                return (x * 2, y * 2);
            }

            double newX = x - size;
            newX = (newX < 0) ? newX+size : newX;
            double newY = y - size;
            newY = (newY < 0) ? newX+size : newY;

            while (usedCoords.Contains((newX, newY)))
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (!usedCoords.Contains((newX, newY)))
                        {
                            goto WhileExit;
                        }
                        newY += size;
                    }
                    if (!usedCoords.Contains((newX, newY)))
                    {
                        goto WhileExit;
                    }
                    newX += size;
                    newY -= 2 * size;
                }

            }

        WhileExit:
            usedCoords.Add((newX, newY));
            return (newX * 2, newY * 2);
        }


        public void SetCoordinates(double width, double height)
        {
            foreach (var item in substationEntities)
            {
                double x = SetToCanvasSize(item.X, xToScale, xMin, size, width / 2);
                double y = SetToCanvasSize(item.Y, yToScale, yMin, size, width / 2);

                (item.X, item.Y) = FindClosestXY(x, y, size);
                double temp = item.X;
                item.X = item.Y;
                item.Y = 4000 - temp;
                myMap.AddCell(item.X, item.Y, Spot.NODE);

            }
            foreach (var item in nodeEntities)
            {
                double x = SetToCanvasSize(item.X, xToScale, xMin, size, width / 2);
                double y = SetToCanvasSize(item.Y, yToScale, yMin, size, width / 2);

                (item.X, item.Y) = FindClosestXY(x, y, size);
                double temp = item.X;
                item.X = item.Y;
                item.Y = 4000 - temp;
                myMap.AddCell(item.X, item.Y, Spot.NODE);

            }
            foreach (var item in switchEntities)
            {
                double x = SetToCanvasSize(item.X, xToScale, xMin, size, width / 2);
                double y = SetToCanvasSize(item.Y, yToScale, yMin, size, width / 2);

                (item.X, item.Y) = FindClosestXY(x, y, size);
                double temp = item.X;
                item.X = item.Y;
                item.Y = 4000 - temp;
                myMap.AddCell(item.X, item.Y, Spot.NODE);

            }
        }

        public double SetToCanvasSize(double point, double scale, double start, double size, double width)
        {
            return Math.Round((point - start) * scale / size) * size % width; //prvi deo je na matrici a drugi je na canvasu
        }

        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }

        public void Draw(Canvas canvas)
        {

            foreach (var item in substationEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.DarkBlue };
                element.ToolTip = "ID:" + item.Id + "\nSubstation" + "\nName:" + item.Name;

                Canvas.SetLeft(element, item.X + 2);
                Canvas.SetTop(element, item.Y + 2);

                item.Shape = element;
                canvas.Children.Add(element);
                powerEntities.Add(item);
            }

            foreach (var item in nodeEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.DarkRed };
                element.ToolTip = "ID:" + item.Id + "\nNode " + "\nName:" + item.Name;

                Canvas.SetLeft(element, item.X + 2);
                Canvas.SetTop(element, item.Y + 2);

                item.Shape = element;
                canvas.Children.Add(element);
                powerEntities.Add(item);
            }
            foreach (var item in switchEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.DarkGreen };
                element.ToolTip = "ID: " + item.Id + "\nSwitch " + "\nName: " + item.Name + "\nStatus: " + item.Status;

                Canvas.SetLeft(element, item.X + 2);
                Canvas.SetTop(element, item.Y + 2);

                item.Shape = element;
                canvas.Children.Add(element);
                powerEntities.Add(item);
            }

            foreach (var item in lineEntities)
            {
                var element = new Line() { Stroke = Brushes.Black };
                (element.X1, element.Y1) = FindElementCoords(item.FirstEnd);
                (element.X2, element.Y2) = FindElementCoords(item.SecondEnd);
                if (element.X1 <= 0 || element.X2 <= 0 || element.Y1 <= 0 || element.Y2 <= 0)
                {
                    continue;
                }
                if (Math.Abs((element.X1 - element.X2)) < 4000 || Math.Abs((element.Y1 - element.Y2)) < 4000)
                {
                    var lines = myMap.createLine(element.X1, element.Y1, element.X2, element.Y2, 0);

                    if (lines.Count < 2) //prekratko
                    {
                        lines = myMap.createLine(element.X1, element.Y1, element.X2, element.Y2, 1);
                    }
                    if (lines.Count > 2)
                    {
                        CreateLine(lines, canvas, FindElement(item.FirstEnd), FindElement(item.SecondEnd), item);
                    }
                }
            }
        }

        private PowerEntity FindElement(long id)
        {
            foreach (var item in substationEntities)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            foreach (var item in nodeEntities)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            foreach (var item in switchEntities)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
        }

        private (double, double) FindElementCoords(long id)
        {
            if (substationEntities.Find((item) => item.Id == id) != null)
            {
                return (substationEntities.Find((item) => item.Id == id).X, substationEntities.Find((item) => item.Id == id).Y);
            }
            else if (nodeEntities.Find((item) => item.Id == id) != null)
            {
                return (nodeEntities.Find((item) => item.Id == id).X, nodeEntities.Find((item) => item.Id == id).Y);
            }
            else if (switchEntities.Find((item) => item.Id == id) != null)
            {
                return (switchEntities.Find((item) => item.Id == id).X, switchEntities.Find((item) => item.Id == id).Y);
            }
            else
            {
                return (0, 0);
            }
        }

        private void CreateLine(List<MyCell> lines, Canvas myCanvas, PowerEntity first, PowerEntity sec, LineEntity line)
        {
            List<Polyline> tempPolyLine = new List<Polyline>();
            tempPolyLine.Add(new Polyline());
            tempPolyLine[0].Stroke = new SolidColorBrush(Colors.Black);
            tempPolyLine[0].StrokeThickness = 1;
            int Xs = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                Spot current = Spot.FREE;
                if (i < lines.Count - 1)
                {
                    if (lines[i].X_coordinates != lines[i + 1].X_coordinates && lines[i].Y_coordinates != lines[i + 1].Y_coordinates)
                    {
                        current = Spot.LINE_CORNER;
                    }
                    else if (lines[i].X_coordinates != lines[i + 1].X_coordinates)
                    {
                        current = Spot.LINE_HORIZONTAL;
                    }
                    else if (lines[i].Y_coordinates != lines[i + 1].Y_coordinates)
                    {
                        current = Spot.LINE_VERICAL;
                    }
                    SetMark(lines[i].X_coordinates, lines[i].Y_coordinates, current);
                }
                System.Windows.Point point = new System.Windows.Point(lines[i].X_coordinates+5 , lines[i].Y_coordinates+5);
                if (myMap.Map[(int)lines[i].X, (int)lines[i].Y].Spot == Spot.LINE_X)
                {
                    Xs++;
                    tempPolyLine.Add(new Polyline());
                    tempPolyLine[Xs].Stroke = new SolidColorBrush(Colors.Black);
                    tempPolyLine[Xs].StrokeThickness = 1;

                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = 10;
                    rectangle.Height = 10;
                    rectangle.Fill = Brushes.Black;
                    rectangle.Stroke = Brushes.Red;


                    Canvas.SetLeft(rectangle, point.X -5);
                    Canvas.SetTop(rectangle, point.Y - 5);
                    myCanvas.Children.Add(rectangle);

                }
                else
                {
                    tempPolyLine[Xs].Points.Add(point);
                    tempPolyLine[Xs].MouseRightButtonDown += SetDefault;
                    tempPolyLine[Xs].MouseRightButtonDown += first.ClickFunction;
                    tempPolyLine[Xs].MouseRightButtonDown += sec.ClickFunction;
                   
                    tempPolyLine[Xs].ToolTip = "Power line\n" + "ID: " + line.Id + "\nName: " + line.Name + "\nTyle: " + line.LineType + "\nConductor material: " + line.ConductorMaterial + "\nUnderground: " + line.IsUnderground.ToString();
                }
            }
            foreach (Polyline polyline in tempPolyLine)
            {
                myCanvas.Children.Add(polyline);
            }

        }

        private void SetMark(double x, double y, Spot current)
        {
            if (current == Spot.FREE)
                return;
            myMap.SetMark(x, y, current);
        }

        private void SetDefault(object sender, EventArgs e)
        {
            foreach (var item in powerEntities)
                item.SetColor();
        }

        #endregion
    }
}
