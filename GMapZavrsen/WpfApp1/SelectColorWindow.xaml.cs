using System;
using System.Collections.Generic;
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
    /// Interaction logic for SelectColorWindow.xaml
    /// </summary>
    public partial class SelectColorWindow : Window
    {
        private SolidColorBrush switches, substations, nodes;

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                var x = cmbNode.SelectedItem.ToString().Split(' ')[1];
                System.Windows.Media.Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(x);
                nodes.Color = color;
            }
            catch (Exception)
            { 
            }
            try
            { 
                var x = cmbSubstation.SelectedItem.ToString().Split(' ')[1];
                System.Windows.Media.Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(x);
                substations.Color = color;
            }
            catch (Exception)
            { 
            }
            try
            { 
                var x = cmbSwitch.SelectedItem.ToString().Split(' ')[1];
                System.Windows.Media.Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(x);
                switches.Color=color;
            }
            catch (Exception)
            { 
            }
            Close();
        }

        public SelectColorWindow(SolidColorBrush switches, SolidColorBrush substations, SolidColorBrush nodes)
        {
            InitializeComponent();
            this.nodes = nodes;
            this.switches = switches;
            this.substations = substations;
            cmbSwitch.ItemsSource = typeof(Colors).GetProperties();
            cmbSubstation.ItemsSource = typeof(Colors).GetProperties(); 
            cmbNode.ItemsSource = typeof(Colors).GetProperties(); 
        }
    }
}
