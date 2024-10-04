using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PositionControl.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PositionControl.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields and Properties

        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        #region Command Methods

        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }

        private void ExecuteLogin(object parameter)
        {
            var user = AuthenticateUser(Username, Password);

            if (user == null)
            {
                MessageBox.Show("User not found or invalid password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SessionManager.LoggedInUser = user;

            
            NavigateUser(parameter, user);
        }

        #endregion

        #region Helper Methods

        private User AuthenticateUser(string username, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users
                                  .Include(u => u.Role)
                                  .FirstOrDefault(u => u.Username == username);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PaswordHash))
                {
                    return user;
                }
            }

            return null;
        }

        private void NavigateUser(object parameter, User user)
        {
            if (parameter is MainWindow mainWindow)
            {
                if (user.Role.RoleName == "Admin")
                {
                    mainWindow.Content = new Views.AdminView();
                }
                else if (user.Role.RoleName == "User")
                {
                    mainWindow.Content = new Views.UserView();
                }
            }
        }

        #endregion
    }
}
