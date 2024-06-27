using Gym.Models;
using Gym.Views.Pages;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Markup.Localizer;

namespace Gym.ViewModels
{
    class LoginsView
    {
        private string login;
        public string Login
        {
            get => login; set => login = value;
        }
        private string password;
        public string Password
        {
            get => password; set => password = value;
        }

        private RelayCommand signInBtnCommand;
        public RelayCommand SignInBtnCommand => signInBtnCommand ?? (signInBtnCommand = new RelayCommand(obj =>
        {
            if(GymAppDbContext.GetContext().Users.Select(u => u.Login).Contains(Login))
            {
                StringBuilder passHash = new StringBuilder();

                using (var hash = SHA256.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    byte[] result = hash.ComputeHash(enc.GetBytes(Password));

                    foreach (byte b in result)
                        passHash.Append(b.ToString("x2"));
                }

                if (GymAppDbContext.GetContext().Users.Where(u => u.Login == Login).Select(u => u).First().PasswordHash == passHash.ToString())
                {
                    switch(GymAppDbContext.GetContext().Users.Where(u => u.Login == Login).Select(u => u.Role).First())
                    {
                        case "Admin":
                            AdminsView.AdminUser = GymAppDbContext.GetContext().Users.Where(u => u.Login == Login).Select(u => u).First();
                            MainView.ChangePage("admin");
                            break;
                        case "Trainer":
                            TrainersView.TrainerUser = GymAppDbContext.GetContext().Users.Where(u => u.Login == Login).Select(u => u).First();
                            MainView.ChangePage("trainer");
                            break;
                        case "Manager":
                            MainView.ChangePage("manager");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Wrong password", "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Sorry that user does not exist", "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }));

    }
}
