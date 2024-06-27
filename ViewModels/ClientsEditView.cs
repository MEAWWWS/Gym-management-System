using Gym.Models;
using Gym.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Gym.ViewModels
{
    class ClientsEditView : INotifyPropertyChanged
    {
        public static Clients ClientToEdit { get; set; } = null;

        private string name;
        public string Name { get => name; set { name = value; OnPropertyChanged("Name"); } }
        private string phone;
        public string Phone { get => phone; set { phone = value; OnPropertyChanged("Phone"); } }
        private string email;
        public string Email { get => email; set { email = value; OnPropertyChanged("Email"); } }
        private int abonimentId;
        public int AbonimentId { get => abonimentId; set { abonimentId = value; OnPropertyChanged("AbonimentId"); } }
        private int trainerId;
        public int TrainerId { get => trainerId; set { trainerId = value; OnPropertyChanged("TrainerId"); } }

        private ObservableCollection<string> aboniments;
        public ObservableCollection<string> Aboniments { get => aboniments; set { aboniments = value; OnPropertyChanged("Aboniments"); } }
        private ObservableCollection<string> trainers;
        public ObservableCollection<string> Trainers { get => trainers; set { trainers = value; OnPropertyChanged("Trainers"); } }

        public ClientsEditView()
        {
            Aboniments = new ObservableCollection<string>(GymAppDbContext.GetContext().Aboniments.Where(a => a.Clients.Count == 0).Select(a => a.DeadlineDate.ToString("dd.MM.yyyy")));
            Trainers = new ObservableCollection<string>(GymAppDbContext.GetContext().TrainerInfos.Select(t => t.Name));
            if (ClientToEdit != null)
            {
                Name = ClientToEdit.Name;
                Phone = ClientToEdit.Phone;
                Email = ClientToEdit.Email ?? "";
                AbonimentId = ClientToEdit.AbonimentId;
                TrainerId = ClientToEdit.TrainerId;
            }
        }

        private RelayCommand addAbonimentBtnCommand;
        public RelayCommand AddAbonimentBtnCommand => addAbonimentBtnCommand ?? (addAbonimentBtnCommand = new RelayCommand(obj =>
        {
            var aew = new AbonimentEditWindow();
            aew.ShowDialog();
            Aboniments = new ObservableCollection<string>(GymAppDbContext.GetContext().Aboniments.Where(a => a.Clients.Count == 0).Select(a => a.DeadlineDate.ToString("dd.MM.yyyy")));
        }));

        private RelayCommand saveBtnCommand;
        public RelayCommand SaveBtnCommand => saveBtnCommand ?? (saveBtnCommand = new RelayCommand(obj =>
        {
            if (ClientToEdit != null)
            {
                var client = GymAppDbContext.GetContext().Clients.Where(c => c.ClientId == ClientToEdit.ClientId).Select(c => c).First();
                client.Name = Name;
                client.Phone = Phone;
                client.Email = Email;
                client.AbonimentId = GymAppDbContext.GetContext().Aboniments.Where(a => a.Clients.Count == 0).Select(a => a.AbonimentId).ToList()[AbonimentId];
                client.TrainerId = GymAppDbContext.GetContext().TrainerInfos.Select(t => t.TrainerId).ToList()[TrainerId];
            }
            else
            {
                var client = new Client
                {
                    Name = Name,
                    Phone = Phone,
                    Email = Email,
                    AbonimentId = GymAppDbContext.GetContext().Aboniments.Where(a => a.Clients.Count == 0).Select(a => a.AbonimentId).ToList()[AbonimentId],
                    TrainerId = GymAppDbContext.GetContext().TrainerInfos.Select(t => t.TrainerId).ToList()[TrainerId]
                };
                GymAppDbContext.GetContext().Clients.Add(client);
            }
            GymAppDbContext.GetContext().SaveChanges();
            ClientToEdit = null;
            (obj as Window).Close();
        }));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
