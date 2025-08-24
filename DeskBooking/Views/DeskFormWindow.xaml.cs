using System.Windows;
using DeskBooking.Models;

namespace DeskBooking.Views
{
    public partial class DeskFormWindow : Window
    {
        public Desk Desk { get; set; }
        public DeskFormWindow(Desk desk)
        {
            InitializeComponent();
            Desk = desk;
            DataContext = Desk;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
