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
    /// Interaction logic for TextDrawing.xaml
    /// </summary>
    public partial class TextDrawing : Window
    {
        public TextDrawing()
        {
            InitializeComponent();
            textColorPicker.ItemsSource = typeof(Colors).GetProperties();
            textColorPicker.SelectedIndex = 7;

            this.DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double size;
                string txt = "";
                size = double.Parse(textSize.Text, CultureInfo.InvariantCulture);
                txt = text.Text.ToString();

                if (size < 0) { errorLabel.Content = "Font size must be >0."; }
                else if (txt == "") { errorLabel.Content = "Text must be entered"; }
                else
                {
                    
       
                    size= Double.Parse(textSize.Text, CultureInfo.InvariantCulture);

                   
                    SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColorPicker.SelectedItem.ToString().Split(' ')[1]);
                  
                    EllipseGeometry ellipseGeometry = new EllipseGeometry();
                    ellipseGeometry.RadiusX = 40;
                    ellipseGeometry.RadiusY = 50;

                    GeometryDrawing geometryDrawing = new GeometryDrawing();
    
                    geometryDrawing.Geometry = ellipseGeometry;
                    geometryDrawing.Brush = Brushes.Transparent;

                    DrawingImage drawingImage = new DrawingImage();
                    drawingImage.Drawing = geometryDrawing;

                    Image image = new Image();
                    image.Source = drawingImage;


                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = text.Text;
                   // SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                    textBlock.Foreground = textBrush;
                    textBlock.FontSize = size;


                    ((MainWindow)Application.Current.MainWindow).newImage = image;
                    ((MainWindow)Application.Current.MainWindow).newTextBlock = textBlock;
                    ((MainWindow)Application.Current.MainWindow).text = text.Text;
                    ((MainWindow)Application.Current.MainWindow).textColor = textBrush.ToString();
                    ((MainWindow)Application.Current.MainWindow).textSize = size;

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
