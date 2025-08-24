using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DeskBooking.Models;

namespace DeskBooking.Views
{
    public class DeskViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Desk> _desks;
        private ObservableCollection<Desk> _filteredDesks;
        private Desk _selectedDesk;
        private string _searchText;

        public ObservableCollection<Desk> Desks
        {
            get => _filteredDesks;
            set { _filteredDesks = value; OnPropertyChanged(); }
        }

        public Desk SelectedDesk
        {
            get => _selectedDesk;
            set { _selectedDesk = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterDesks(); }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public DeskViewModel()
        {
            // Example data (replace with data access logic)
            _desks = new ObservableCollection<Desk>
            {
                new Desk { Id = 1, DeskNumber = "D-101", IsActive = true },
                new Desk { Id = 2, DeskNumber = "D-102", IsActive = false }
            };
            _filteredDesks = new ObservableCollection<Desk>(_desks);

            AddCommand = new RelayCommand(AddDesk);
            DeleteCommand = new RelayCommand(DeleteDesk);
        }

        private void FilterDesks()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                Desks = new ObservableCollection<Desk>(_desks);
            else
                Desks = new ObservableCollection<Desk>(
                    _desks.Where(d => d.DeskNumber.ToLower().Contains(SearchText.ToLower()))
                );
        }

        private void AddDesk(object obj)
        {
            var newDesk = new Desk { Id = _desks.Any() ? _desks.Max(d => d.Id) + 1 : 1, DeskNumber = "New Desk", IsActive = true };
            _desks.Add(newDesk);
            FilterDesks();
        }

        private void DeleteDesk(object obj)
        {
            if (SelectedDesk != null)
            {
                _desks.Remove(SelectedDesk);
                FilterDesks();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}