using PositionControl.Models;
using System.Linq;

namespace PositionControl.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        #region Fields and Properties

        private string _name;
        private string _lastName;
        private string _positionName;
        private string _positionDescription;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
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
       

        #endregion

        public UserViewModel()
        {
            LoadUserData();
            
        }

        #region Private Methods

        private void LoadUserData()
        {
            var loggedInUser = SessionManager.LoggedInUser;

            if (loggedInUser == null)
                return;

            using (var context = new AppDbContext())
            {
                var userWorkPosition = GetUserWorkPosition(context, loggedInUser.Id);

                if (userWorkPosition != null)
                {
                    PopulateUserData(userWorkPosition.FirstName, userWorkPosition.LastName, userWorkPosition.PositionName, userWorkPosition.PositionDescription);
                }
                else
                {
                    
                    PopulateUserData(loggedInUser.FirstName, loggedInUser.LastName, "No position assigned", string.Empty);
                }
            }
        }

        private dynamic GetUserWorkPosition(AppDbContext context, int userId)
        {
            return (from uwp in context.UserWorkPositions
                    join wp in context.WorkPositions on uwp.PositionId equals wp.Id
                    join u in context.Users on uwp.UserId equals u.Id
                    where u.Id == userId
                    select new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        PositionName = wp.PositionName,
                        PositionDescription = wp.PositionDescription
                    }).FirstOrDefault();
        }

        private void PopulateUserData(string firstName, string lastName, string positionName, string positionDescription)
        {
            Name = firstName;
            LastName = lastName;
            PositionName = positionName;
            PositionDescription = positionDescription;
        }

        #endregion
    }
}
