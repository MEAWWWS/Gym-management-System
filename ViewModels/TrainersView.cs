using Gym.Models;
using Gym.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gym.ViewModels
{
    class TrainersView : INotifyPropertyChanged
    {
        public static Users TrainerUser { get; set; } = null;
        //clients
        private Clients selectedClient;
        public Clients SelectedClient { get => selectedClient; set { selectedClient = value; OnPropertyChanged("SelectedClient"); } }

        private ObservableCollection<Clients> clients;
        public ObservableCollection<Clients> Clients { get => clients; set { clients = value; OnPropertyChanged("Clients"); } }

        private RelayCommand clientAddBtnCommand;
        public RelayCommand ClientSetSessionBtnCommand => clientAddBtnCommand ?? (clientAddBtnCommand = new RelayCommand(obj =>
        {
            SessionAddBtnCommand.Execute(this);
            ClientUpdateBtnCommand.Execute(this);
        }));
        
        private RelayCommand clientUpdateBtnCommand;
        public RelayCommand ClientUpdateBtnCommand => clientUpdateBtnCommand ?? (clientUpdateBtnCommand = new RelayCommand(obj =>
        {
            var tid = GymAppDbContext.GetContext().TrainerInfos.Where(ti => ti.UserId == TrainerUser.UserId).Select(ti => ti).First();
            Clients = new ObservableCollection<Clients>(GymAppDbContext.GetContext().Clients.Where(c => c.TrainerId == tid.TrainerId).Select(c => c));
        }));
        //sessions
        private Sessions selectedSession;
        public Sessions SelectedSession { get => selectedSession; set { selectedSession = value; OnPropertyChanged("SelectedSession"); } }

        private ObservableCollection<Sessions> sessions;
        public ObservableCollection<Sessions> Sessions { get => sessions; set { sessions = value; OnPropertyChanged("Sessions"); } }

        private RelayCommand sessionAddBtnCommand;
        public RelayCommand SessionAddBtnCommand => sessionAddBtnCommand ?? (sessionAddBtnCommand = new RelayCommand(obj =>
        {
            SessionsEditView.TrainerInfo = GymAppDbContext.GetContext().TrainerInfos.Where(ti => ti.UserId == TrainerUser.UserId).Select(ti => ti).First();
            var sew = new SessionEditWindow();
            sew.ShowDialog();
            SessionsEditView.TrainerInfo = null;
            SessionUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand sessionUpdateBtnCommand;
        public RelayCommand SessionUpdateBtnCommand => sessionUpdateBtnCommand ?? (sessionUpdateBtnCommand = new RelayCommand(obj =>
        {
            var tid = GymAppDbContext.GetContext().TrainerInfos.Where(ti => ti.UserId == TrainerUser.UserId).Select(ti => ti).First();
            Sessions = new ObservableCollection<Sessions>(GymAppDbContext.GetContext().Sessions.Where(s => s.TrainerId == tid.TrainerId).Select(s => s));
        }));

        public TrainersView()
        {
            var tid = GymAppDbContext.GetContext().TrainerInfos.Where(ti => ti.UserId == TrainerUser.UserId).Select(ti => ti).First();
            Clients = new ObservableCollection<Clients>(GymAppDbContext.GetContext().Clients.Where(c => c.TrainerId == tid.TrainerId).Select(c => c));
            Sessions = new ObservableCollection<Sessions>(GymAppDbContext.GetContext().Sessions.Where(s => s.TrainerId == tid.TrainerId).Select(s => s));
        }

        private RelayCommand exitBtnCommand;
        public RelayCommand ExitBtnCommand => exitBtnCommand ?? (exitBtnCommand = new RelayCommand(obj =>
        {
            MainView.ChangePage("login");
        }));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
