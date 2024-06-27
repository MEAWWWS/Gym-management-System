using Gym.Models;
using Gym.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Gym.ViewModels
{
    class AdminsView : INotifyPropertyChanged
    {
        public static Users AdminUser { get; set; } = null;
        //user tab
        private RelayCommand userAddBtnCommand;
        public RelayCommand UserAddBtnCommand => userAddBtnCommand ?? (userAddBtnCommand = new RelayCommand(obj =>
        {
            UsersEditView.UserToEdit = null;
            UserEditWindow userEditWindow = new UserEditWindow();
            userEditWindow.ShowDialog();
            UserUpdateCommand.Execute(this);
        }));
        private RelayCommand userRemoveBtnCommand;
        public RelayCommand UserRemoveBtnCommand => userRemoveBtnCommand ?? (userRemoveBtnCommand = new RelayCommand(obj =>
        {
            if(MessageBox.Show($"Would you remove {selectedUser.Login}", "Confimation", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().Users.Remove(selectedUser);
                GymAppDbContext.GetContext().SaveChanges();
                UserUpdateCommand.Execute(this);
            }
        }));
        private RelayCommand userEditBtnCommand;
        public RelayCommand UserEditBtnCommand => userEditBtnCommand ?? (userEditBtnCommand = new RelayCommand(obj =>
        {
            UsersEditView.UserToEdit = selectedUser;
            UserEditWindow userEditWindow = new UserEditWindow();
            userEditWindow.ShowDialog();
            UserUpdateCommand.Execute(this);
        }));
        private RelayCommand userUpdateCommand;
        public RelayCommand UserUpdateCommand => userUpdateCommand ?? (userUpdateCommand = new RelayCommand(obj =>  
        {
            Users = new ObservableCollection<Users>(GymAppDbContext.GetContext().Users);
        }));
        private Users selectedUser;
        public Users SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private ObservableCollection<Users> users;
        public ObservableCollection<Users> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        //equipment
        private Equipments selectedEquipment;
        public Equipments SelectedEquipment
        {
            get => selectedEquipment;
            set
            {
                selectedEquipment = value;
                OnPropertyChanged("SelectedEquipment");
            }
        }

        private ObservableCollection<Equipments> equipments;
        public ObservableCollection<Equipments> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                OnPropertyChanged(nameof(Equipments));
            }
        }

        private RelayCommand equipmentAddBtnCommand;
        public RelayCommand EquipmentAddBtnCommand => equipmentAddBtnCommand ?? (equipmentAddBtnCommand = new RelayCommand(obj => 
        {
            EquipmentsEditView.EquipmentToEdit = null;
            EquipmentEditWindow equipmentEditWindow = new EquipmentEditWindow();
            equipmentEditWindow.ShowDialog();
            EquipmentUpdateBtnCommand.Execute(this);

        }));
        private RelayCommand equipmentRemoveBtnCommand;
        public RelayCommand EquipmentRemoveBtnCommand => equipmentRemoveBtnCommand ?? (equipmentRemoveBtnCommand = new RelayCommand(obj =>
        {
            if(MessageBox.Show($"Do you really want remove {selectedEquipment.Title} equipment?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().Equipment.Remove(SelectedEquipment);
                GymAppDbContext.GetContext().SaveChanges();
                EquipmentUpdateBtnCommand.Execute(this);
            }
        }));
        private RelayCommand equipmentEditBtnCommand;
        public RelayCommand EquipmentEditBtnCommand => equipmentEditBtnCommand ?? (equipmentEditBtnCommand = new RelayCommand(obj =>
        {
            EquipmentsEditView.EquipmentToEdit = SelectedEquipment;
            EquipmentEditWindow equipmentEditWindow = new EquipmentEditWindow();
            equipmentEditWindow.ShowDialog();
            EquipmentUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand equipmentUpdateBtnCommand;
        public RelayCommand EquipmentUpdateBtnCommand => equipmentUpdateBtnCommand ?? (equipmentUpdateBtnCommand = new RelayCommand(obj =>
        {
            Equipments = new ObservableCollection<Equipments>(GymAppDbContext.GetContext().Equipment);
        }));
        //clients
        private Clients selectedClient;
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

        private RelayCommand clientAddBtnCommand;
        public RelayCommand ClientAddBtnCommand => clientAddBtnCommand ?? (clientAddBtnCommand = new RelayCommand(obj =>
        {
            ClientsEditView.ClientToEdit = null;
            var cew = new ClientEditWindow();
            cew.ShowDialog();
            ClientUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand clientRemoveBtnCommand;
        public RelayCommand ClientRemoveBtnCommand => clientRemoveBtnCommand ?? (clientRemoveBtnCommand = new RelayCommand(obj =>
        {
            if(MessageBox.Show($"Do you really want remove {selectedClient.Name} client?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().Clients.Remove(selectedClient);
                GymAppDbContext.GetContext().SaveChanges();
                ClientUpdateBtnCommand.Execute(this);
            }
        }));
        private RelayCommand clientEditBtnCommand;
        public RelayCommand ClientEditBtnCommand => clientEditBtnCommand ?? (clientEditBtnCommand = new RelayCommand(obj =>
        {
            ClientsEditView.ClientToEdit = SelectedClient;
            var cew = new ClientEditWindow();
            cew.ShowDialog();
            ClientUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand clientUpdateBtnCommand;
        public RelayCommand ClientUpdateBtnCommand => clientUpdateBtnCommand ?? (clientUpdateBtnCommand = new RelayCommand(obj =>
        {
            Clients = new ObservableCollection<Clients>(GymAppDbContext.GetContext().Clients);
        }));
        //reports

        //aboniments
        private Aboniments selectedAboniment;
        public Aboniments SelectedAboniment { get => selectedAboniment; set { selectedAboniment = value; OnPropertyChanged("SelectedAboniment"); } }

        private ObservableCollection<Aboniments> aboniments;
        public ObservableCollection<Aboniments> Aboniments { get => aboniments; set { aboniments = value; OnPropertyChanged("Aboniments"); } }

        private RelayCommand abonimentAddBtnCommand;
        public RelayCommand AbonimentAddBtnCommand => abonimentAddBtnCommand ?? (abonimentAddBtnCommand = new RelayCommand(obj =>
        {
            AbonimentsEditView.AbonimentToEdit = null;
            var aew = new AbonimentEditWindow();
            aew.ShowDialog();
            AbonimentUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand abonimentRemoveBtnCommand;
        public RelayCommand AbonimentRemoveBtnCommand => abonimentRemoveBtnCommand ?? (abonimentRemoveBtnCommand = new RelayCommand(obj =>
        {
            if(MessageBox.Show($"Do you really want remove {selectedAboniment.AbonimentId} aboniment?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().Aboniments.Remove(SelectedAboniment);
                GymAppDbContext.GetContext().SaveChanges();
                AbonimentUpdateBtnCommand.Execute(this);
            }
        }));
        private RelayCommand abonimentEditBtnCommand;
        public RelayCommand AbonimentEditBtnCommand => abonimentEditBtnCommand ?? (abonimentEditBtnCommand = new RelayCommand(obj =>
        {
            AbonimentsEditView.AbonimentToEdit = SelectedAboniment;
            var aew = new AbonimentEditWindow();
            aew.ShowDialog();
            AbonimentUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand abonimentUpdateBtnCommand;
        public RelayCommand AbonimentUpdateBtnCommand => abonimentUpdateBtnCommand ?? (abonimentUpdateBtnCommand = new RelayCommand(obj =>
        {
            Aboniments = new ObservableCollection<Aboniments>(GymAppDbContext.GetContext().Aboniments);
        }));
        //trainers
        private TrainersInfo selectedTrainer;
        public TrainersInfo SelectedTrainer { get => selectedTrainer; set { selectedTrainer = value; OnPropertyChanged("SelectedTrainer"); } }

        private ObservableCollection<TrainersInfo> trainers;
        public ObservableCollection<TrainersInfo> Trainers { get => trainers; set { trainers = value; OnPropertyChanged("Trainers"); } }

        private RelayCommand trainderAddBtnCommand;
        public RelayCommand TrainderAddBtnCommand => trainderAddBtnCommand ?? (trainderAddBtnCommand = new RelayCommand(obj =>
        {
            TrainersEditView.TrainerToEdit = null;
            TrainerEditWindow tew = new TrainerEditWindow();
            tew.ShowDialog();
            TrainderUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand trainderRemoveBtnCommand;
        public RelayCommand TrainderRemoveBtnCommand => trainderRemoveBtnCommand ?? (trainderRemoveBtnCommand = new RelayCommand(obj =>
        {
            if(MessageBox.Show($"Do you really want remove info about {selectedTrainer.User.Login} user", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) ==MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().TrainerInfos.Remove(selectedTrainer);
                GymAppDbContext.GetContext().SaveChanges();
                TrainderUpdateBtnCommand.Execute(this);
            }
        }));
        private RelayCommand trainderEditBtnCommand;
        public RelayCommand TrainderEditBtnCommand => trainderEditBtnCommand ?? (trainderEditBtnCommand = new RelayCommand(obj =>
        {
            TrainersEditView.TrainerToEdit = SelectedTrainer;
            TrainerEditWindow tew = new TrainerEditWindow();
            tew.ShowDialog();
            TrainderUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand trainderUpdateBtnCommand;
        public RelayCommand TrainderUpdateBtnCommand => trainderUpdateBtnCommand ?? (trainderUpdateBtnCommand = new RelayCommand(obj =>
        {
            Trainers = new ObservableCollection<TrainersInfo>(GymAppDbContext.GetContext().TrainerInfos);
        }));
        //session
        private Sessions selectedSession;
        public Sessions SelectedSession { get => selectedSession; set { selectedSession = value; OnPropertyChanged("SelectedSession"); } }

        private ObservableCollection<Sessions> sessions;
        public ObservableCollection<Sessions> Sessions { get => sessions; set { sessions = value; OnPropertyChanged("Sessions"); } }

        private RelayCommand sessionAddBtnCommand;
        public RelayCommand SessionAddBtnCommand => sessionAddBtnCommand ?? (sessionAddBtnCommand = new RelayCommand(obj =>
        {
            SessionsEditView.SessionToEdit = null;
            var sew = new SessionEditWindow();
            sew.ShowDialog();
            SessionUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand sessionRemoveBtnCommand;
        public RelayCommand SessionRemoveBtnCommand => sessionRemoveBtnCommand ?? (sessionRemoveBtnCommand = new RelayCommand(obj =>
        {
            if(MessageBox.Show($"Do you really want remove {SelectedSession.SessionId} Session?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GymAppDbContext.GetContext().Sessions.Remove(SelectedSession);
                GymAppDbContext.GetContext().SaveChanges();
                SessionUpdateBtnCommand.Execute(this);
            }
        }));
        private RelayCommand sessionEditBtnCommand;
        public RelayCommand SessionEditBtnCommand => sessionEditBtnCommand ?? (sessionEditBtnCommand = new RelayCommand(obj =>
        {
            SessionsEditView.SessionToEdit = SelectedSession;
            var sew = new SessionEditWindow();
            sew.ShowDialog();
            SessionUpdateBtnCommand.Execute(this);
        }));
        private RelayCommand sessionUpdateBtnCommand;
        public RelayCommand SessionUpdateBtnCommand=> sessionUpdateBtnCommand ?? (sessionUpdateBtnCommand = new RelayCommand(obj => 
        {
            Sessions = new ObservableCollection<Sessions>(GymAppDbContext.GetContext().Sessions);
        }));


        //common
        private RelayCommand exitBtnCommand;
        public RelayCommand ExitBtnCommand => exitBtnCommand ?? (exitBtnCommand = new RelayCommand(obj => 
        {
            MainView.ChangePage("login");
        }));
        //add on closing command on all edit windows to clear static prop


        public AdminsView()
        {
            Users = new ObservableCollection<Users>(GymAppDbContext.GetContext().Users);
            Equipments = new ObservableCollection<Equipments>(GymAppDbContext.GetContext().Equipment);
            Clients = new ObservableCollection<Clients>(GymAppDbContext.GetContext().Clients);
            Trainers = new ObservableCollection<TrainersInfo>(GymAppDbContext.GetContext().TrainerInfos);
            Aboniments = new ObservableCollection<Aboniments>(GymAppDbContext.GetContext().Aboniments);
            Sessions = new ObservableCollection<Sessions>(GymAppDbContext.GetContext().Sessions);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
