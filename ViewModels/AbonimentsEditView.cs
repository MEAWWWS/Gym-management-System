using Gym.Models;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Gym.ViewModels
{
    class AbonimentsEditView : INotifyPropertyChanged
    {
        public static Aboniments AbonimentToEdit { get; set; } = null;

        private string purchaseDate;
        public string PurchaseDate { get => purchaseDate; set { purchaseDate = value; OnPropertyChanged("PurchaseDate"); } }
        private string deadlineDate;
        public string DeadlineDate { get => deadlineDate; set { deadlineDate = value; OnPropertyChanged("DeadlineDate"); } }
        private decimal price;
        public decimal Price { get => price; set { price = value; OnPropertyChanged("Price"); } }

        private RelayCommand saveBtnCommand;
        public RelayCommand SaveBtnCommand => saveBtnCommand ?? (saveBtnCommand = new RelayCommand(obj =>
        {
            if(AbonimentToEdit != null)
            {
                var abon = GymAppDbContext.GetContext().Aboniments.Where(a => a.AbonimentId == AbonimentToEdit.AbonimentId).Select(a => a).First();
                abon.PurchaseDate = DateOnly.Parse($"{PurchaseDate.Split(' ')[0].Split('/')[1]}.{PurchaseDate.Split(' ')[0].Split('/')[0]}.{PurchaseDate.Split(' ')[0].Split('/')[2]}");
                abon.DeadlineDate = DateOnly.Parse($"{DeadlineDate.Split(' ')[0].Split('/')[1]}.{DeadlineDate.Split(' ')[0].Split('/')[0]}.{DeadlineDate.Split(' ')[0].Split('/')[2]}");
                abon.Price = Price;
            }
            else
            {
                //var pd = $"{PurchaseDate.Split(' ')[0].Split('/')[1]}.{PurchaseDate.Split(' ')[0].Split('/')[0]}.{PurchaseDate.Split(' ')[0].Split('/')[2]}";
                //abon.PurchaseDate = DateOnly.Parse(pd);
                //var dd = $"{DeadlineDate.Split(' ')[0].Split('/')[1]}.{DeadlineDate.Split(' ')[0].Split('/')[0]}.{DeadlineDate.Split(' ')[0].Split('/')[2]}";
                //abon.DeadlineDate = DateOnly.Parse(dd);
                //abon.Price = Price;
                var abon = new Aboniment
                { 
                    PurchaseDate = DateOnly.Parse($"{PurchaseDate.Split(' ')[0].Split('/')[1]}.{PurchaseDate.Split(' ')[0].Split('/')[0]}.{PurchaseDate.Split(' ')[0].Split('/')[2]}"),
                    DeadlineDate = DateOnly.Parse($"{DeadlineDate.Split(' ')[0].Split('/')[1]}.{DeadlineDate.Split(' ')[0].Split('/')[0]}.{DeadlineDate.Split(' ')[0].Split('/')[2]}"),
                    Price = Price
                };
                GymAppDbContext.GetContext().Add(abon);
            }
            AbonimentToEdit = null;
            GymAppDbContext.GetContext().SaveChanges();
            (obj as Window).Close();
        }));

        public AbonimentsEditView()
        {
            if(AbonimentToEdit != null)
            {
                PurchaseDate = AbonimentToEdit.PurchaseDate.ToShortDateString();
                DeadlineDate = AbonimentToEdit.DeadlineDate.ToShortDateString();
                Price = AbonimentToEdit.Price;
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
