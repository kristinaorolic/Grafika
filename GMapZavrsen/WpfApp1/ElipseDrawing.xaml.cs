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
    /// Interaction logic for ElipseDrawing.xaml
    /// </summary>
    public partial class ElipseDrawing : Window
    {

   

        public ElipseDrawing()
        {
            InitializeComponent();

            borderColorPicker.ItemsSource = typeof(Colors).GetProperties();
            borderColorPicker.SelectedIndex = 7;

            fillColorPicker.ItemsSource = typeof(Colors).GetProperties();
            fillColorPicker.SelectedIndex = 14;

            textColorPicker.ItemsSource = typeof(Colors).GetProperties();
            textColorPicker.SelectedItem = 7;


            this.DataContext = this;
        }

      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double x, y, thicc;
                string txt = "";
                x = double.Parse(radiusX.Text, CultureInfo.InvariantCulture);
                y = double.Parse(radiusY.Text, CultureInfo.InvariantCulture);
                thicc = double.Parse(borderSize.Text, CultureInfo.InvariantCulture);
                txt = text.Text.ToString();
                if (x <= 0) { errorLabel.Content = "Radius X must be > 0."; }
                else if (y <= 0) { errorLabel.Content = "Radius Y must be > 0."; }
                else if (thicc < 0) { errorLabel.Content = "Border size must be >= 0."; }
                else
                {
                    EllipseGeometry ellipseGeometry = new EllipseGeometry();
                    ellipseGeometry.RadiusX = x;
                    ellipseGeometry.RadiusY = y;


                    //TextBlock textBlock = new TextBlock();
                    //textBlock.Text = text.Text;
                    //SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColorPicker.SelectedItem.ToString().Split(' ')[1]);
                    //textBlock.Foreground = textBrush;

                    //var bitmap = new RenderTargetBitmap((int)(2 * x), (int)(2 * y), 96, 96, PixelFormats.Pbgra32);
                    //bitmap.Render(textBlock);


                    SolidColorBrush borderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(borderColorPicker.SelectedItem.ToString().Split(' ')[1]);
                    SolidColorBrush fillBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(fillColorPicker.SelectedItem.ToString().Split(' ')[1]);
                    SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColorPicker.SelectedItem.ToString().Split(' ')[1]);

                    Pen p = new Pen(borderBrush, thicc);

                    GeometryDrawing geometryDrawing = new GeometryDrawing();
                    geometryDrawing.Pen = p;
                    geometryDrawing.Geometry = ellipseGeometry;
                    geometryDrawing.Brush = fillBrush;

                    DrawingImage drawingImage = new DrawingImage();
                    drawingImage.Drawing = geometryDrawing;




                    Image image = new Image();
                    image.Source = drawingImage;


                 


                    ((MainWindow)Application.Current.MainWindow).newImage = image;
                    //((MainWindow)Application.Current.MainWindow).textImage = image2;


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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FillColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
