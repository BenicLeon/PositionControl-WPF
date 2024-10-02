using PositionControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PositionControl.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string _name;
        private string _lastName;
        private string _positionName;
        private string _positionDescription;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string PositionName
        {
            get { return _positionName; }
            set
            {
                _positionName = value;
                OnPropertyChanged(nameof(PositionName));
            }
        }
        public string PositionDescription
        {
            get { return _positionDescription; }
            set
            {
                _positionDescription = value;
                OnPropertyChanged(nameof(PositionDescription));
            }
        }
        public UserViewModel()
        {
            LoadUserData();
        }

        
        private void LoadUserData()
        {

            var loggedInUserId = SessionManager.LoggedInUser.Id;

            using (var context = new AppDbContext())
            {
                
                var userWorkPosition = (from uwp in context.UserWorkPositions
                                        join wp in context.WorkPositions on uwp.PositionId equals wp.Id
                                        join u in context.Users on uwp.UserId equals u.Id
                                        where u.Id == loggedInUserId
                                        select new
                                        {
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            PositionName = wp.PositionName,
                                            PositionDescription = wp.PositionDescription
                                        }).FirstOrDefault();

                
                if (userWorkPosition != null)
                {
                    Name = userWorkPosition.FirstName;
                    LastName = userWorkPosition.LastName;
                    PositionName = userWorkPosition.PositionName;
                    PositionDescription = userWorkPosition.PositionDescription;
                }
                else
                {
                    
                    Name = SessionManager.LoggedInUser.FirstName;
                    LastName = SessionManager.LoggedInUser.LastName;
                    PositionName = "No position assigned";
                    PositionDescription = string.Empty;
                }
            }
        }
    }
}