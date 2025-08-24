using System.Windows;
using DeskBooking.Models;

namespace DeskBooking.Views
{
    public partial class UserFormWindow : Window
    {
        public Employee User { get; set; }
        public UserFormWindow(Employee user)
        {
            InitializeComponent();
            User = user;
            DataContext = User;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
