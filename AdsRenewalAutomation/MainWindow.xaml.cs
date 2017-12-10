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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdsRenewalAutomation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scraper Scraper = new Scraper();
        public MainWindow()
        {
            InitializeComponent();

        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(usernameField.Text) && !String.IsNullOrEmpty(passwordField.Password.ToString()))
            {
                Scraper.Start(usernameField.Text, passwordField.Password.ToString());
                StartButton.Visibility = Visibility.Collapsed;
                StopButton.Visibility = Visibility.Visible;
                FormGrid.Visibility = Visibility.Collapsed;
                Loader.Visibility = Visibility.Visible;
            }
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            Scraper.ShouldStop = true;
            StopButton.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Visible;
            Loader.Visibility = Visibility.Collapsed;
            FormGrid.Visibility = Visibility.Visible;
        }
    }
}
