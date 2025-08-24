using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DeskBooking.Models;
using DeskBooking.Services;

namespace DeskBooking.Views
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private readonly CrudService<Booking> _service = new();
        private readonly CrudService<Employee> _employeeService = new();
        private readonly CrudService<Desk> _deskService = new();
        private ObservableCollection<Booking> _bookings = new();
        private ObservableCollection<Booking> _filteredBookings = new();
        private ObservableCollection<Employee> _employees = new();
        private ObservableCollection<Desk> _desks = new();
        private Booking? _selectedBooking;
        private string _searchText = string.Empty;

        public ObservableCollection<Booking> Bookings
        {
            get => _filteredBookings;
            set { _filteredBookings = value; OnPropertyChanged(); }
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set { _selectedBooking = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterBookings(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public BookingViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand(AddBooking);
            EditCommand = new RelayCommand(EditBooking, _ => SelectedBooking != null);
            DeleteCommand = new RelayCommand(DeleteBooking, _ => SelectedBooking != null);
        }

        private void LoadData()
        {
            _employees = new ObservableCollection<Employee>(_employeeService.GetAll());
            _desks = new ObservableCollection<Desk>(_deskService.GetAll());
            _bookings = new ObservableCollection<Booking>(_service.GetAll()
                .Select(b =>
                {
                    b.Employee = _employees.FirstOrDefault(e => e.Id == b.EmployeeId);
                    b.Desk = _desks.FirstOrDefault(d => d.Id == b.DeskId);
                    return b;
                }));
            FilterBookings();
        }

        private void FilterBookings()
        {
            Bookings = string.IsNullOrWhiteSpace(SearchText)
                ? new ObservableCollection<Booking>(_bookings)
                : new ObservableCollection<Booking>(_bookings.Where(b =>
                    (b.Employee?.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (b.Desk?.DeskNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)));
        }

        private void AddBooking(object? obj)
        {
            var form = new BookingFormWindow(_employees, new ObservableCollection<Desk>(_desks.Where(d => d.IsActive)), null, null, DateTime.Today, DateTime.Today.AddDays(1));
            if (form.ShowDialog() == true)
            {
                if (form.SelectedEmployee == null || form.SelectedDesk == null)
                {
                    MessageBox.Show("Please select both an employee and an active desk.", "Booking Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var booking = new Booking
                {
                    EmployeeId = form.SelectedEmployee.Id,
                    DeskId = form.SelectedDesk.Id,
                    BookedFrom = form.BookedFrom,
                    BookedUntil = form.BookedUntil
                };
                _service.Add(booking);
                LoadData();
            }
        }

        private void EditBooking(object? obj)
        {
            if (SelectedBooking == null) return;
            var form = new BookingFormWindow(
                _employees,
                new ObservableCollection<Desk>(_desks.Where(d => d.IsActive)),
                SelectedBooking.Employee,
                SelectedBooking.Desk,
                SelectedBooking.BookedFrom,
                SelectedBooking.BookedUntil);
            if (form.ShowDialog() == true)
            {
                if (form.SelectedEmployee == null || form.SelectedDesk == null)
                {
                    MessageBox.Show("Please select both an employee and an active desk.", "Booking Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var booking = new Booking
                {
                    Id = SelectedBooking.Id,
                    EmployeeId = form.SelectedEmployee.Id,
                    DeskId = form.SelectedDesk.Id,
                    BookedFrom = form.BookedFrom,
                    BookedUntil = form.BookedUntil
                };
                _service.Update(booking);
                LoadData();
            }
        }

        private void DeleteBooking(object? obj)
        {
            if (SelectedBooking == null) return;
            _service.Delete(SelectedBooking.Id);
            LoadData();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
