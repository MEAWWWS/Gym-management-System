using Gym.Models;
using System.Windows;

namespace Gym.ViewModels
{
    public class EquipmentsEditView
    {
        public static Equipments EquipmentToEdit { get; set; }

        private string title;
        private string description;

        public string Title
        {
            get => title;
            set => title = value;
        }
        public string Description
        {
            get => description;
            set => description = value;
        }

        public EquipmentsEditView()
        {
            if (EquipmentToEdit != null)
            {
                Title = EquipmentToEdit.Title;
                Description = EquipmentToEdit.Description;
            }
        }

        private RelayCommand saveBtnCommand;
        public RelayCommand SaveButtonCommand => saveBtnCommand ?? (saveBtnCommand = new RelayCommand(obj =>
        {
            if (EquipmentToEdit != null)
            {
                var eq = GymAppDbContext.GetContext().Equipment.Where(eq => eq.EquipmentId == EquipmentToEdit.EquipmentId).Select(eq => eq).First();
                eq.Title = Title;
                eq.Description = Description;
                GymAppDbContext.GetContext().SaveChanges();
                EquipmentToEdit = null;
            }
            else
            {
                var eq = new Equipment { Title = Title, Description = Description };
                GymAppDbContext.GetContext().Equipment.Add(eq);
                GymAppDbContext.GetContext().SaveChanges();
            }
            (obj as Window).Close();
        }));
    }
}
