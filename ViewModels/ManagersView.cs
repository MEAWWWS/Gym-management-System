using Gym.Models;
using Gym.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Gym.ViewModels
{
    class ManagersView : INotifyPropertyChanged
    {
        public static Users ManagerUser { get; set; } = null;

        public ManagersView()
        {
            Equipments = new ObservableCollection<Equipments>(GymAppDbContext.GetContext().Equipment);
            Clients = new ObservableCollection<Clients>(GymAppDbContext.GetContext().Clients);
            Trainers = new ObservableCollection<TrainersInfo>(GymAppDbContext.GetContext().TrainerInfos);
            Aboniments = new ObservableCollection<Aboniments>(GymAppDbContext.GetContext().Aboniments);
        }

        //client
        private Clients? selectedClient;
        public Clients SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        private ObservableCollection<Clients> clients;
        public ObservableCollection<Clients> Clients
        {
            get => clients;
            set
            {
                clients = value;
                OnPropertyChanged("Clients");
            }
        }

        private RelayCommand? clientUpdateBtnCommand;
        public RelayCommand ClientUpdateBtnCommand => clientUpdateBtnCommand ?? (clientUpdateBtnCommand = new RelayCommand(obj =>
        {
            Clients = new ObservableCollection<Clients>(GymAppDbContext.GetContext().Clients);
        }));

        //equipment
        private Equipments? selectedEquipment;
        public Equipments SelectedEquipment
        {
            get => selectedEquipment;
            set
            {
                selectedEquipment = value;
                OnPropertyChanged("SelectedEquipment");
            }
        }

        private ObservableCollection<Equipments>? equipments;
        public ObservableCollection<Equipments> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                OnPropertyChanged(nameof(Equipments));
            }
        }

        private RelayCommand? equipmentUpdateBtnCommand;
        public RelayCommand EquipmentUpdateBtnCommand => equipmentUpdateBtnCommand ?? (equipmentUpdateBtnCommand = new RelayCommand(obj =>
        {
            Equipments = new ObservableCollection<Equipments>(GymAppDbContext.GetContext().Equipment);
        }));

        //abon
        private Aboniments? selectedAboniment;
        public Aboniments SelectedAboniment { get => selectedAboniment; set { selectedAboniment = value; OnPropertyChanged("SelectedAboniment"); } }

        private ObservableCollection<Aboniments>? aboniments;
        public ObservableCollection<Aboniments>? Aboniments { get => aboniments; set { aboniments = value; OnPropertyChanged("Aboniments"); } }

        private RelayCommand? abonimentAddBtnCommand;
        public RelayCommand AbonimentAddBtnCommand => abonimentAddBtnCommand ?? (abonimentAddBtnCommand = new RelayCommand(obj =>
        {
            AbonimentsEditView.AbonimentToEdit = null;
            var aew = new AbonimentEditWindow();
            aew.ShowDialog();
            AbonimentUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand? abonimentRemoveBtnCommand;
        public RelayCommand AbonimentRemoveBtnCommand => abonimentRemoveBtnCommand ?? (abonimentRemoveBtnCommand = new RelayCommand(obj =>
        {
            if (MessageBox.Show($"Do you really want remove {selectedAboniment.AbonimentId} aboniment?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().Aboniments.Remove(SelectedAboniment);
                GymAppDbContext.GetContext().SaveChanges();
                AbonimentUpdateBtnCommand.Execute(this);
            }
        }));
        private RelayCommand? abonimentEditBtnCommand;
        public RelayCommand AbonimentEditBtnCommand => abonimentEditBtnCommand ?? (abonimentEditBtnCommand = new RelayCommand(obj =>
        {
            AbonimentsEditView.AbonimentToEdit = SelectedAboniment;
            var aew = new AbonimentEditWindow();
            aew.ShowDialog();
            AbonimentUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand? abonimentUpdateBtnCommand;
        public RelayCommand AbonimentUpdateBtnCommand => abonimentUpdateBtnCommand ?? (abonimentUpdateBtnCommand = new RelayCommand(obj =>
        {
            Aboniments = new ObservableCollection<Aboniments>(GymAppDbContext.GetContext().Aboniments);
        }));

        //trens
        private TrainersInfo? selectedTrainer;
        public TrainersInfo? SelectedTrainer { get => selectedTrainer; set { selectedTrainer = value; OnPropertyChanged("SelectedTrainer"); } }

        private ObservableCollection<TrainersInfo>? trainers;
        public ObservableCollection<TrainersInfo>? Trainers { get => trainers; set { trainers = value; OnPropertyChanged("Trainers"); } }

        private RelayCommand? trainderUpdateBtnCommand;
        public RelayCommand TrainderUpdateBtnCommand => trainderUpdateBtnCommand ?? (trainderUpdateBtnCommand = new RelayCommand(obj =>
        {
            Trainers = new ObservableCollection<TrainersInfo>(GymAppDbContext.GetContext().TrainerInfos);
        }));

        private RelayCommand exitBtnCommand;
        public RelayCommand ExitBtnCommand => exitBtnCommand ?? (exitBtnCommand = new RelayCommand(obj =>
        {
            MainView.ChangePage("login");
        }));

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
