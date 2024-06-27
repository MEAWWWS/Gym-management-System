using Gym.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Gym.ViewModels
{
    class TrainersEditView : INotifyPropertyChanged
    {
        public static TrainersInfo TrainerToEdit { get; set; } = null;

        private string name;
        public string Name { get => name; set { name = value; OnPropertyChanged("Name"); } }
        private string specialization;
        public string Specialization { get => specialization; set { specialization = value; OnPropertyChanged("Specialization"); } }
        private string schedule;
        public string Schedule { get => schedule; set { schedule = value; OnPropertyChanged("Schedule"); } }
        private int userId;
        public int UserId { get => userId; set { userId = value; OnPropertyChanged("UserId"); } }

        private ObservableCollection<String> trainersLogins;
        public ObservableCollection<String> TrainersLogins { get => trainersLogins; set { trainersLogins = value; OnPropertyChanged("TrainersLogins"); } }

        private RelayCommand saveBtnCommand;
        public RelayCommand SaveBtnCommand => saveBtnCommand ?? (saveBtnCommand = new RelayCommand(obj =>
        {
            if(TrainerToEdit != null)
            {
                var ti = GymAppDbContext.GetContext().TrainerInfos.Where(ti => ti.TrainerId == TrainerToEdit.TrainerId).Select(ti => ti).First();
                ti.Name = Name;
                ti.Specialization = Specialization;
                ti.Schedule = Schedule;
                ti.UserId = GymAppDbContext.GetContext().Users.Where(u => u.Role == "Trainer").Select(ti => ti.UserId).ToList()[UserId];
            }
            else
            {
                var ti = new TrainerInfo();
                ti.Name = Name;
                ti.Specialization = Specialization;
                ti.Schedule = Schedule;
                ti.UserId = GymAppDbContext.GetContext().Users.Where(u => u.Role == "Trainer").Select(ti => ti.UserId).ToList()[UserId];
                GymAppDbContext.GetContext().TrainerInfos.Add(ti);
            }
            TrainerToEdit = null;
            GymAppDbContext.GetContext().SaveChanges();
            (obj as Window).Close();
            //uid select by login
        }));

        public TrainersEditView()
        {
            TrainersLogins = new ObservableCollection<string>(GymAppDbContext.GetContext().Users.Where(u => u.Role == "Trainer").Select(ti => ti.Login));
            if(TrainerToEdit != null)
            {
                Name = TrainerToEdit.Name;
                Specialization = TrainerToEdit.Specialization;
                Schedule = TrainerToEdit.Schedule;
                UserId = TrainerToEdit.UserId;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
