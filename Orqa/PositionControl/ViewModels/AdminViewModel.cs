using PositionControl.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PositionControl.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        // Collections for Users and Positions
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<WorkPosition> Positions { get; set; }

        // Properties for the selected user and selected position
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                LoadAssignedPosition();
            }
        }

        private WorkPosition _selectedPosition;
        public WorkPosition SelectedPosition
        {
            get { return _selectedPosition; }
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        private UserWorkPosition _assignedPosition;
        public UserWorkPosition AssignedPosition
        {
            get { return _assignedPosition; }
            set
            {
                _assignedPosition = value;
                OnPropertyChanged(nameof(AssignedPosition));
            }
        }

        // Commands
        public ICommand AssignPositionCommand { get; set; }
        public ICommand CreateUserCommand { get; set; }

        public AdminViewModel()
        {
            Users = new ObservableCollection<User>();
            Positions = new ObservableCollection<WorkPosition>();

            // Commands
            AssignPositionCommand = new RelayCommand(AssignPosition, CanAssignPosition);
            CreateUserCommand = new RelayCommand(CreateUser);

            // Load initial data
            LoadUsers();
            LoadPositions();
        }

        // Load users and positions
        private void LoadUsers()
        {
            using (var context = new AppDbContext())
            {
                var users = context.Users.ToList();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
        }

        private void LoadPositions()
        {
            using (var context = new AppDbContext())
            {
                var positions = context.WorkPositions.ToList();
                Positions.Clear();
                foreach (var position in positions)
                {
                    Positions.Add(position);
                }
            }
        }

        // Load the assigned position for the selected user
        private void LoadAssignedPosition()
        {
            if (SelectedUser != null)
            {
                using (var context = new AppDbContext())
                {
                    AssignedPosition = context.UserWorkPositions
                        .FirstOrDefault(uwp => uwp.UserId == SelectedUser.Id);
                }
            }
        }

        // Assign a new position to the selected user
        private void AssignPosition(object parameter)
        {
            if (SelectedUser != null && SelectedPosition != null)
            {
                using (var context = new AppDbContext())
                {
                    var userWorkPosition = context.UserWorkPositions
                        .FirstOrDefault(uwp => uwp.UserId == SelectedUser.Id);

                    if (userWorkPosition != null)
                    {
                        userWorkPosition.PositionId = SelectedPosition.Id;
                        userWorkPosition.AssignDate = DateTime.Now;
                    }
                    else
                    {
                        // Create a new assignment if it doesn't exist
                        var newAssignment = new UserWorkPosition
                        {
                            UserId = SelectedUser.Id,
                            PositionId = SelectedPosition.Id,
                            AssignDate = DateTime.Now
                        };
                        context.UserWorkPositions.Add(newAssignment);
                    }

                    context.SaveChanges();
                    LoadAssignedPosition(); // Refresh
                }

                MessageBox.Show("Position assigned successfully.");
            }
        }

        private bool CanAssignPosition(object parameter)
        {
            return SelectedUser != null && SelectedPosition != null;
        }

        // Create a new user
        private void CreateUser(object parameter)
        {
            using (var context = new AppDbContext())
            {
                var newUser = new User
                {
                    FirstName = "New",
                    LastName = "User",
                    Username = "newuser",
                    PaswordHash = BCrypt.Net.BCrypt.HashPassword("defaultpassword"), // Set default password
                    RoleId = 2 // Assuming 2 is the ID for a standard User role
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                Users.Add(newUser);
                MessageBox.Show("New user created successfully.");
            }
        }
    }
}
