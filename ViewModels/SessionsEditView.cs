using Gym.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Gym.ViewModels
{
    class SessionsEditView : INotifyPropertyChanged
    {
        public static Sessions SessionToEdit { get; set; } = null;
        public static TrainersInfo TrainerInfo { get; set; } = null;

        private int trainerId;
        public int TrainerId { get => trainerId; set {  trainerId = value; OnPropertyChanged("TrainerId"); } }
        private int cliendId;
        public int ClientId { get => cliendId; set { cliendId = value; OnPropertyChanged("ClientId"); } }
        private string sessionDate;
        public string SessionDate { get => sessionDate; set { sessionDate = value; OnPropertyChanged("SessionDate"); } }
        private string sessionTime;
        public string SessionTime { get => sessionTime; set { sessionTime = value; OnPropertyChanged("SessionTime"); } }
        private string sessionStartDate;
        public string SessionStartDate { get => sessionStartDate; set { sessionStartDate = value; OnPropertyChanged("SessionStartDate"); } }
        private string sessionStartTime;
        public string SessionStartTime { get => sessionStartTime; set { sessionStartTime = value; OnPropertyChanged("SessionStartTime"); } }

        private ObservableCollection<string> clients;
        public ObservableCollection<string> Clients { get => clients; set { clients = value; OnPropertyChanged("Clients"); } }
        private ObservableCollection<string> trainers;
        public ObservableCollection<string> Trainers { get => trainers; set { trainers = value; OnPropertyChanged("Trainers"); } }

        public SessionsEditView()
        {
            Clients = new ObservableCollection<string>(GymAppDbContext.GetContext().Clients.Select(c => c.Name).ToList());
            Trainers = new ObservableCollection<string>(GymAppDbContext.GetContext().TrainerInfos.Select(t => t.Name).ToList());
            if(SessionToEdit != null)
            {
                SessionDate = SessionToEdit.SessionDate.ToString("dd/MM/yyyy");
                SessionTime = SessionToEdit.SessionTime.ToString("HH:mm:ss");
                SessionStartDate = SessionToEdit.SessionStartDateTime.Date.ToString("dd.MM.yyyy");
                SessionStartTime = SessionToEdit.SessionStartDateTime.ToShortTimeString();

            }
            else if(TrainerInfo != null)
            {
                Clients = new ObservableCollection<string>(GymAppDbContext.GetContext().Clients.Where(c => c.TrainerId == TrainerInfo.TrainerId).Select(c => c.Name));
                Trainers = new ObservableCollection<string>(GymAppDbContext.GetContext().TrainerInfos.Where(ti => ti.TrainerId == TrainerInfo.TrainerId).Select(ti => ti.Name));
            }

        }

        private RelayCommand saveBtnCommand;
        public RelayCommand SaveBtnCommand => saveBtnCommand ?? (saveBtnCommand = new RelayCommand(obj =>
        {
            if(SessionToEdit != null)
            {
                var sess = GymAppDbContext.GetContext().Sessions.Where(s => s.SessionId == SessionToEdit.SessionId).Select(s => s).First();
                sess.SessionDate = DateOnly.Parse($"{SessionDate.Split(' ')[0].Split('/')[1]}.{SessionDate.Split(' ')[0].Split('/')[0]}.{SessionDate.Split(' ')[0].Split('/')[2]}");
                sess.SessionTime = TimeOnly.Parse(SessionTime);
                sess.SessionStartDateTime = DateTime.Parse($"{SessionStartDate.Split(' ')[0].Split('/')[1]}.{SessionStartDate.Split(' ')[0].Split('/')[0]}.{SessionStartDate.Split(' ')[0].Split('/')[2]} {SessionStartTime}");
                sess.ClientId = GymAppDbContext.GetContext().Clients.Select(c => c.ClientId).ToList()[ClientId];
                sess.TrainerId = GymAppDbContext.GetContext().TrainerInfos.Select(t => t.TrainerId).ToList()[TrainerId];
            }
            else
            {
                var sess = new Session
                {
                    SessionDate = DateOnly.Parse($"{SessionDate.Split(' ')[0].Split('/')[1]}.{SessionDate.Split(' ')[0].Split('/')[0]}.{SessionDate.Split(' ')[0].Split('/')[2]}"),
                    SessionTime = TimeOnly.Parse(SessionTime),
                    SessionStartDateTime = DateTime.Parse($"{SessionStartDate.Split(' ')[0].Split('/')[1]}.{SessionStartDate.Split(' ')[0].Split('/')[0]}.{SessionStartDate.Split(' ')[0].Split('/')[2]} {SessionStartTime}"),
                    ClientId = GymAppDbContext.GetContext().Clients.Select(c => c.ClientId).ToList()[ClientId],
                    TrainerId = GymAppDbContext.GetContext().TrainerInfos.Select(t => t.TrainerId).ToList()[TrainerId]
                };
                GymAppDbContext.GetContext().Sessions.Add(sess);
            }
            GymAppDbContext.GetContext().SaveChanges();
            (obj as Window).Close();
        }));

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
