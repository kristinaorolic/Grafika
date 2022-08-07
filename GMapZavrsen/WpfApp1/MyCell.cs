using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp1.Model;

namespace WpfApp1
{
    public enum Spot { FREE, LINE, NODE, LINE_HORIZONTAL, LINE_VERICAL, LINE_CORNER, LINE_X }
    class MyCell
    {
        private PowerEntity value;
        private double x_coordinates;
        private double y_coordinates;
        private Spot spot;
        private Brush color;
        private int x;
        private int y;
        private Shape uiElement;

        public PowerEntity Value { get => value; set => this.value = value; }
        public double X_coordinates { get => x_coordinates; set => x_coordinates = value; }
        public double Y_coordinates { get => y_coordinates; set => y_coordinates = value; }
        public Spot Spot { get => spot; set => spot = value; }
        public Brush Color { get => color; set => color = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public Shape UiElement { get => uiElement; set => uiElement = value; }

        public MyCell(double x_coordinates, double y_coordinates, Spot spot, int x, int y, Shape uiElement)
        {
            this.x_coordinates = x_coordinates;
            this.y_coordinates = y_coordinates;
            this.spot = spot;
            this.x = x;
            this.y = y;
            this.uiElement = uiElement;
        }

        public MyCell(double x_coordinates, double y_coordinates, int x, int y)
        {
            this.x_coordinates = x_coordinates;
            this.y_coordinates = y_coordinates;
            this.x = x;
            this.y = y;
            this.uiElement = null;
            this.spot = Spot.FREE;
        }

        public MyCell()
        {
            this.x_coordinates = -1;
            this.y_coordinates = -1;
            this.x = -1;
            this.y = -1;
            this.uiElement = null;
            this.spot = Spot.FREE;
        }
    }
}
