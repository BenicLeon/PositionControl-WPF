using PositionControl.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace PositionControl.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        #region Fields and Properties

        
        private ObservableCollection<UserWorkPositionViewModel> _userWorkPositions = new ObservableCollection<UserWorkPositionViewModel>();
        private ObservableCollection<WorkPosition> _availablePositions = new ObservableCollection<WorkPosition>();
        private UserWorkPositionViewModel _selectedUser;
        private WorkPosition _selectedPosition;

        
        private string _positionName;
        private string _positionDescription;

        
        private string _newFirstName;
        private string _newLastName;
        private string _newUsername;
        private string _newPassword;

        public ObservableCollection<UserWorkPositionViewModel> UserWorkPositions
        {
            get => _userWorkPositions;
            set
            {
                _userWorkPositions = value;
                OnPropertyChanged(nameof(UserWorkPositions));
            }
        }

        public ObservableCollection<WorkPosition> AvailablePositions
        {
            get => _availablePositions;
            set
            {
                _availablePositions = value;
                OnPropertyChanged(nameof(AvailablePositions));
            }
        }

        public UserWorkPositionViewModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public WorkPosition SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        public string PositionName
        {
            get => _positionName;
            set
            {
                _positionName = value;
                OnPropertyChanged(nameof(PositionName));
            }
        }

        public string PositionDescription
        {
            get => _positionDescription;
            set
            {
                _positionDescription = value;
                OnPropertyChanged(nameof(PositionDescription));
            }
        }

        public string NewFirstName
        {
            get => _newFirstName;
            set
            {
                _newFirstName = value;
                OnPropertyChanged(nameof(NewFirstName));
            }
        }

        public string NewLastName
        {
            get => _newLastName;
            set
            {
                _newLastName = value;
                OnPropertyChanged(nameof(NewLastName));
            }
        }

        public string NewUsername
        {
            get => _newUsername;
            set
            {
                _newUsername = value;
                OnPropertyChanged(nameof(NewUsername));
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        #endregion

        #region Commands

        public ICommand CreatePositionCommand { get; }
        public ICommand ChangePositionCommand { get; }
        public ICommand CreateUserCommand { get; }

        #endregion

        public AdminViewModel()
        {
            CreatePositionCommand = new RelayCommand(_ => CreatePosition());
            ChangePositionCommand = new RelayCommand(_ => ChangePosition());
            CreateUserCommand = new RelayCommand(_ => CreateUser());

            
            LoadUsers();
            LoadAvailablePositions();
        }

        #region User and Position Loading

        private void LoadUsers()
        {
            ExecuteDatabaseAction(context =>
            {
                var users = from u in context.Users
                            join uwp in context.UserWorkPositions on u.Id equals uwp.UserId into uwpGroup
                            from uwp in uwpGroup.DefaultIfEmpty()
                            join wp in context.WorkPositions on uwp.PositionId equals wp.Id into wpGroup
                            from wp in wpGroup.DefaultIfEmpty()
                            where u.RoleId == 12
                            select new UserWorkPositionViewModel
                            {
                                UserId = u.Id,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Position = wp != null ? wp.PositionName : "No Position",
                                AssignDate = uwp != null ? uwp.AssignDate : DateTime.MinValue
                            };

                UpdateObservableCollection(UserWorkPositions, users.ToList());
            });
        }

        private void LoadAvailablePositions()
        {
            ExecuteDatabaseAction(context =>
            {
                var positions = context.WorkPositions.ToList();
                UpdateObservableCollection(AvailablePositions, positions);
            });
        }

        #endregion

        #region Position Management

        private void CreatePosition()
        {
            if (string.IsNullOrWhiteSpace(PositionName) || string.IsNullOrWhiteSpace(PositionDescription))
            {
                MessageBox.Show("Please provide both position name and description.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExecuteDatabaseAction(context =>
            {
                var newPosition = new WorkPosition
                {
                    PositionName = PositionName,
                    PositionDescription = PositionDescription
                };
                context.WorkPositions.Add(newPosition);
                context.SaveChanges();

                PositionName = string.Empty;
                PositionDescription = string.Empty;

                LoadAvailablePositions();
                MessageBox.Show("New position added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void ChangePosition()
        {
            if (SelectedUser == null || SelectedPosition == null)
            {
                MessageBox.Show("Please select both a user and a position.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExecuteDatabaseAction(context =>
            {
                if (SelectedUser == null || SelectedPosition == null)
                {
                    MessageBox.Show("User or Position is not selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                var userWorkPosition = context.UserWorkPositions.FirstOrDefault(uwp => uwp.UserId == SelectedUser.UserId);

                if (userWorkPosition == null)
                {
                    
                    context.UserWorkPositions.Add(new UserWorkPosition
                    {
                        UserId = SelectedUser.UserId,
                        PositionId = SelectedPosition.Id,
                        AssignDate = DateTime.Now
                    });
                }
                else if (userWorkPosition.PositionId != SelectedPosition.Id)
                {
                    
                    userWorkPosition.PositionId = SelectedPosition.Id;
                }
                else
                {
                    
                    MessageBox.Show("The selected position is already assigned to this user.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                
                context.SaveChanges();
                LoadUsers();
                MessageBox.Show("Position updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });





        }

        #endregion

        #region User Management

        private void CreateUser()
        {
            if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(NewFirstName) || string.IsNullOrWhiteSpace(NewLastName))
            {
                MessageBox.Show("Please fill in all user details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExecuteDatabaseAction(context =>
            {
                if (context.Users.Any(u => u.Username == NewUsername))
                {
                    MessageBox.Show("User already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newUser = new User
                {
                    FirstName = NewFirstName,
                    LastName = NewLastName,
                    Username = NewUsername,
                    PaswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword),
                    RoleId = 12
                };
                context.Users.Add(newUser);
                context.SaveChanges();

                LoadUsers();
                MessageBox.Show("User created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        #endregion

        #region Helper Methods

        private void ExecuteDatabaseAction(Action<AppDbContext> action)
        {
            using (var context = new AppDbContext())
            {
                action(context);
            }
        }

        private void UpdateObservableCollection<T>(ObservableCollection<T> collection, List<T> newData)
        {
            collection.Clear();
            foreach (var item in newData)
            {
                collection.Add(item);
            }
        }

        #endregion

        public class UserWorkPositionViewModel
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Position { get; set; }
            public DateTime AssignDate { get; set; }
        }
    }
}
