using Gym.Models;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Gym.ViewModels
{
    class UsersEditView
    {
        public static Users UserToEdit { get; set; } = null;

        private string login;
        private string password;
        private string role;

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }

        public UsersEditView()
        {
            if(UserToEdit != null)
            {
                Login = UserToEdit.Login;
                Role = UserToEdit.Role;
            }
        }

        private RelayCommand saveBtnCommand;
        public RelayCommand SaveBtnCommand => saveBtnCommand ?? (saveBtnCommand = new RelayCommand(obj =>
        {
            if (UserToEdit != null)
            {
                var user = GymAppDbContext.GetContext().Users.Where(u => u.UserId == UserToEdit.UserId).Select(u => u).First();
                StringBuilder passHash = new StringBuilder();

                using (var hash = SHA256.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    byte[] result = hash.ComputeHash(enc.GetBytes(Password));

                    foreach (byte b in result)
                        passHash.Append(b.ToString("x2"));
                }

                user.Login = Login;
                user.PasswordHash = passHash.ToString();
                user.Role = Role;

                GymAppDbContext.GetContext().SaveChanges();
                (obj as Window).Close();
                UserToEdit = null;
            }
            else
            {
                StringBuilder passHash = new StringBuilder();

                using (var hash = SHA256.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    byte[] result = hash.ComputeHash(enc.GetBytes(Password));

                    foreach (byte b in result)
                        passHash.Append(b.ToString("x2"));
                }

                var user = new User { Login = Login, PasswordHash = passHash.ToString(), Role = Role };
                GymAppDbContext.GetContext().Users.Add(user);
                GymAppDbContext.GetContext().SaveChanges();
                (obj as Window).Close();
            }
        }));

    }
}
