using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for PolygonDrawing.xaml
    /// </summary>
    public partial class PolygonDrawing : Window
    {
       

        public PolygonDrawing()
        {
            InitializeComponent();

            borderColorPicker.ItemsSource = typeof(Colors).GetProperties();
            borderColorPicker.SelectedIndex = 7;

            fillColorPicker.ItemsSource = typeof(Colors).GetProperties();
            fillColorPicker.SelectedIndex = 14;

            textColorPicker.ItemsSource = typeof(Colors).GetProperties();
            textColorPicker.SelectedIndex = 7;


            this.DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            for (i = ((MainWindow)Application.Current.MainWindow).drawing_canvas.Children.Count - 1, j = ((MainWindow)Application.Current.MainWindow).polygonPoints.Count; i >= 0 && j > 0; i--, j--)
            {
                ((MainWindow)Application.Current.MainWindow).drawing_canvas.Children.RemoveAt(i);
            }

            ((MainWindow)Application.Current.MainWindow).polygonPoints.Clear();
            ((MainWindow)Application.Current.MainWindow).polygonPoints.Count.ToString();

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PointCollection points = ((MainWindow)Application.Current.MainWindow).polygonPoints;
                double thicc = double.Parse(borderSize.Text, CultureInfo.InvariantCulture);
                if (thicc < 0) { errorLabel.Content = "Border size must be >= 0."; }
                else if (points.Count < 3) { errorLabel.Content = "Must mark at least 3 points."; }
                else
                {
                    SolidColorBrush borderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(borderColorPicker.SelectedItem.ToString().Split(' ')[1]);
                    SolidColorBrush fillBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(fillColorPicker.SelectedItem.ToString().Split(' ')[1]);
                    SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColorPicker.SelectedItem.ToString().Split(' ')[1]);

                    PathFigure pf = new PathFigure();
                    pf.IsClosed = true;
                    pf.StartPoint = points[0];

                    PathSegmentCollection psc = new PathSegmentCollection();



                    for (int i = 1; i < points.Count; i++)
                    {
                        LineSegment ls = new LineSegment();
                        ls.Point = points[i];
                        psc.Add(ls);
                    }

                    pf.Segments = psc;

                    PathFigureCollection pfc = new PathFigureCollection();
                    pfc.Add(pf);

                    PathGeometry pg = new PathGeometry();
                    pg.Figures = pfc;

                    Pen p = new Pen(borderBrush, thicc);

                    GeometryDrawing geometryDrawing = new GeometryDrawing();
                    geometryDrawing.Pen = p;
                    geometryDrawing.Geometry = pg;
                    geometryDrawing.Brush = fillBrush;

                    DrawingImage drawingImage = new DrawingImage();
                    drawingImage.Drawing = geometryDrawing;

                    Image image = new Image();
                    image.Source = drawingImage;

                    ((MainWindow)Application.Current.MainWindow).newImage = image;
                    ((MainWindow)Application.Current.MainWindow).text = text.Text;
                    ((MainWindow)Application.Current.MainWindow).textColor = textBrush.ToString();

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = "Enter numerical values only.";
            }
        }
    }
}
