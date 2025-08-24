using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DeskBooking.Models;
using DeskBooking.Services;

namespace DeskBooking.Views
{
    public class DeskViewModel : INotifyPropertyChanged
    {
        private readonly CrudService<Desk> _service = new();
        private ObservableCollection<Desk> _desks = new();
        private ObservableCollection<Desk> _filteredDesks = new();
        private Desk? _selectedDesk;
        private string _searchText = string.Empty;

        public ObservableCollection<Desk> Desks
        {
            get => _filteredDesks;
            set { _filteredDesks = value; OnPropertyChanged(); }
        }

        public Desk? SelectedDesk
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
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public DeskViewModel()
        {
            LoadDesks();
            AddCommand = new RelayCommand(AddDesk);
            EditCommand = new RelayCommand(EditDesk, _ => SelectedDesk != null);
            DeleteCommand = new RelayCommand(DeleteDesk, _ => SelectedDesk != null);
        }

        private void LoadDesks()
        {
            _desks = new ObservableCollection<Desk>(_service.GetAll());
            FilterDesks();
        }

        private void FilterDesks()
        {
            Desks = string.IsNullOrWhiteSpace(SearchText)
                ? new ObservableCollection<Desk>(_desks)
                : new ObservableCollection<Desk>(_desks.Where(d => d.DeskNumber.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));
        }

        private void AddDesk(object? obj)
        {
            var desk = new Desk();
            var form = new DeskFormWindow(desk);
            if (form.ShowDialog() == true)
            {
                _service.Add(desk);
                LoadDesks();
            }
        }

        private void EditDesk(object? obj)
        {
            if (SelectedDesk == null) return;
            var deskCopy = new Desk { Id = SelectedDesk.Id, DeskNumber = SelectedDesk.DeskNumber, IsActive = SelectedDesk.IsActive };
            var form = new DeskFormWindow(deskCopy);
            if (form.ShowDialog() == true)
            {
                _service.Update(deskCopy);
                LoadDesks();
            }
        }

        private void DeleteDesk(object? obj)
        {
            if (SelectedDesk == null) return;
            _service.Delete(SelectedDesk.Id);
            LoadDesks();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}