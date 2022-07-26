using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;


namespace WpfApp1.Model
{
    public class PowerEntity
    {
        private long id;
        private string name;
        private double x;
        private double y;
        private Ellipse shape;

        public PowerEntity()
        {

        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public Ellipse Shape { get => shape; set => shape = value; }


        virtual public void SetColor()
        {

        }

        public void ClickFunction(object sender, EventArgs e)
        {
            Shape.Fill = Brushes.HotPink;
        }
    }
}
