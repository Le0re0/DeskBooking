using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DeskBooking.Models;
using DeskBooking.Services;

namespace DeskBooking.Views
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly CrudService<Employee> _service = new();
        private ObservableCollection<Employee> _users = new();
        private ObservableCollection<Employee> _filteredUsers = new();
        private Employee? _selectedUser;
        private string _searchText = string.Empty;

        public ObservableCollection<Employee> Users
        {
            get => _filteredUsers;
            set { _filteredUsers = value; OnPropertyChanged(); }
        }

        public Employee? SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterUsers(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public UserViewModel()
        {
            LoadUsers();
            AddCommand = new RelayCommand(AddUser);
            EditCommand = new RelayCommand(EditUser, _ => SelectedUser != null);
            DeleteCommand = new RelayCommand(DeleteUser, _ => SelectedUser != null);
        }

        private void LoadUsers()
        {
            _users = new ObservableCollection<Employee>(_service.GetAll());
            FilterUsers();
        }

        private void FilterUsers()
        {
            Users = string.IsNullOrWhiteSpace(SearchText)
                ? new ObservableCollection<Employee>(_users)
                : new ObservableCollection<Employee>(_users.Where(u =>
                    u.FirstName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));
        }

        private void AddUser(object? obj)
        {
            var user = new Employee();
            var form = new UserFormWindow(user);
            if (form.ShowDialog() == true)
            {
                _service.Add(user);
                LoadUsers();
            }
        }

        private void EditUser(object? obj)
        {
            if (SelectedUser == null) return;
            var userCopy = new Employee { Id = SelectedUser.Id, FirstName = SelectedUser.FirstName, LastName = SelectedUser.LastName, Email = SelectedUser.Email };
            var form = new UserFormWindow(userCopy);
            if (form.ShowDialog() == true)
            {
                _service.Update(userCopy);
                LoadUsers();
            }
        }

        private void DeleteUser(object? obj)
        {
            if (SelectedUser == null) return;
            _service.Delete(SelectedUser.Id);
            LoadUsers();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
