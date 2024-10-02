using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PositionControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PositionControl.ViewModels
{
    public class LoginViewModel:ViewModelBase
    {
        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }
        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }

        private void ExecuteLogin(object parameter)
        {
            using (var context = new AppDbContext())
            {
                
                var user = context.Users
                                  .Include(u => u.Role) 
                                  .FirstOrDefault(u => u.Username == Username);

                if (user != null)
                {
                    
                    if (BCrypt.Net.BCrypt.Verify(Password, user.PaswordHash))
                    {
                        MessageBox.Show("Login successful!");

                       
                        SessionManager.LoggedInUser = user;

                        
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
                    else
                    {
                        MessageBox.Show("Invalid password!");
                    }
                }
                else
                {
                    MessageBox.Show("User not found!");
                }
            }
        }





    }

}


