using System;
using System.Collections.ObjectModel;
using System.Windows;
using DeskBooking.Models;

namespace DeskBooking.Views
{
    public partial class BookingFormWindow : Window
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<Desk> Desks { get; set; }
        public Employee SelectedEmployee { get; set; }
        public Desk SelectedDesk { get; set; }
        public DateTime BookedFrom { get; set; }
        public DateTime BookedUntil { get; set; }

        public BookingFormWindow(ObservableCollection<Employee> employees, ObservableCollection<Desk> desks, Employee selectedEmployee, Desk selectedDesk, DateTime bookedFrom, DateTime bookedUntil)
        {
            InitializeComponent();
            Employees = employees;
            Desks = desks;
            SelectedEmployee = selectedEmployee;
            SelectedDesk = selectedDesk;
            BookedFrom = bookedFrom;
            BookedUntil = bookedUntil;
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
