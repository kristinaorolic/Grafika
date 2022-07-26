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
    public class NodeEntity : PowerEntity
    {
        public override void SetColor()
        {
            Shape.Fill = Brushes.DarkRed;
        }
    }
}
