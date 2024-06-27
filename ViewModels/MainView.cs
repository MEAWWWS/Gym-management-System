using Gym.Views.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Gym.ViewModels
{
    class MainView : INotifyPropertyChanged
    {

        private Page loginPage;
        private Page adminPage;
        private Page trainerPage;
        private Page managerPage;

        private static string moveToPage = "login";

        private Page currentPage;
        public Page CurrentPage { get => currentPage; set { currentPage = value; OnPropertyChanged("CurrentPage"); } }

        private double frameOpacity;

        public double FrameOpacity { get => frameOpacity; set => frameOpacity = value; }

        public MainView()
        {
            loginPage = new LoginPage();
            adminPage = new AdminPage();

            FrameOpacity = 1;

            CurrentPage = loginPage;
            PageUpdater();
        }

        public static void ChangePage(string page)
        {
            switch (page)
            {
                case "login":
                    moveToPage = "login";
                    break;
                case "trainer":
                    moveToPage = "trainer";
                    break;
                case "manager":
                    moveToPage = "manager";
                    break;
                case "admin":
                    moveToPage = "admin";
                    break;
            }
        }

        private void PageUpdater()
        {
            DispatcherTimer dt = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(50) };
            dt.Tick += (o, e) =>
            {
                switch (moveToPage)
                {
                    case "admin":
                        if (CurrentPage != adminPage)
                        {
                            adminPage = new AdminPage();
                            CurrentPage = adminPage;
                        }
                        break;
                    case "trainer":
                        if(CurrentPage != trainerPage)
                        {
                            trainerPage = new TrainerPage();
                            CurrentPage = trainerPage;
                        }
                        break;
                    case "manager":
                        if(CurrentPage != managerPage)
                        {
                            managerPage = new ManagerPage();
                            CurrentPage = managerPage;
                        }
                        break;
                    case "login":
                        if (CurrentPage != loginPage)
                        {
                            loginPage = new LoginPage();
                            CurrentPage = loginPage;
                        }
                        break;
                }
            };
            dt.Start();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
