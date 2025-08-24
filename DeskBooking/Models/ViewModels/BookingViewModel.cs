using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DeskBooking.Models;

namespace DeskBooking.Views
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Booking> _bookings;
        private ObservableCollection<Booking> _filteredBookings;
        private Booking _selectedBooking;
        private string _searchText;

        public ObservableCollection<Booking> Bookings
        {
            get => _filteredBookings;
            set { _filteredBookings = value; OnPropertyChanged(); }
        }

        public Booking SelectedBooking
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
        public ICommand DeleteCommand { get; }

        public BookingViewModel()
        {
            // Example data (replace with data access logic)
            _bookings = new ObservableCollection<Booking>
            {
                new Booking { Id = 1, Employee = new Employee { FirstName = "Alice", LastName = "Smith" }, Desk = new Desk { DeskNumber = "D-101" }, BookedFrom = DateTime.Today, BookedUntil = DateTime.Today.AddDays(1) },
                new Booking { Id = 2, Employee = new Employee { FirstName = "Bob", LastName = "Jones" }, Desk = new Desk { DeskNumber = "D-102" }, BookedFrom = DateTime.Today, BookedUntil = DateTime.Today.AddDays(2) }
            };
            _filteredBookings = new ObservableCollection<Booking>(_bookings);

            AddCommand = new RelayCommand(AddBooking);
            DeleteCommand = new RelayCommand(DeleteBooking);
        }

        private void FilterBookings()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                Bookings = new ObservableCollection<Booking>(_bookings);
            else
                Bookings = new ObservableCollection<Booking>(
                    _bookings.Where(b => (b.Employee?.FullName.ToLower().Contains(SearchText.ToLower()) ?? false) ||
                                         (b.Desk?.DeskNumber.ToLower().Contains(SearchText.ToLower()) ?? false))
                );
        }

        private void AddBooking(object obj)
        {
            var newBooking = new Booking
            {
                Id = _bookings.Any() ? _bookings.Max(b => b.Id) + 1 : 1,
                Employee = new Employee { FirstName = "New", LastName = "Employee" },
                Desk = new Desk { DeskNumber = "New Desk" },
                BookedFrom = DateTime.Today,
                BookedUntil = DateTime.Today.AddDays(1)
            };
            _bookings.Add(newBooking);
            FilterBookings();
        }

        private void DeleteBooking(object obj)
        {
            if (SelectedBooking != null)
            {
                _bookings.Remove(SelectedBooking);
                FilterBookings();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
