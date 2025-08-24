using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DeskBooking.Models;

namespace DeskBooking.Views
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _users;
        private ObservableCollection<Employee> _filteredUsers;
        private Employee _selectedUser;
        private string _searchText;

        public ObservableCollection<Employee> Users
        {
            get => _filteredUsers;
            set { _filteredUsers = value; OnPropertyChanged(); }
        }

        public Employee SelectedUser
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
        public ICommand DeleteCommand { get; }

        public UserViewModel()
        {
            // Example data (replace with data access logic)
            _users = new ObservableCollection<Employee>
            {
                new Employee { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
                new Employee { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" }
            };
            _filteredUsers = new ObservableCollection<Employee>(_users);

            AddCommand = new RelayCommand(AddUser);
            DeleteCommand = new RelayCommand(DeleteUser);
        }

        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                Users = new ObservableCollection<Employee>(_users);
            else
                Users = new ObservableCollection<Employee>(
                    _users.Where(u => u.FirstName.ToLower().Contains(SearchText.ToLower()) ||
                                      u.LastName.ToLower().Contains(SearchText.ToLower()) ||
                                      u.Email.ToLower().Contains(SearchText.ToLower()))
                );
        }

        private void AddUser(object obj)
        {
            var newUser = new Employee { Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1, FirstName = "New", LastName = "User", Email = "newuser@example.com" };
            _users.Add(newUser);
            FilterUsers();
        }

        private void DeleteUser(object obj)
        {
            if (SelectedUser != null)
            {
                _users.Remove(SelectedUser);
                FilterUsers();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
