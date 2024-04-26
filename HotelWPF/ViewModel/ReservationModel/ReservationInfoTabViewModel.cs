using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelWPF.ViewModel.ReservationModel
{
    public class ReservationInfoTabViewModel : ViewModelBase
    {
        private ReservationInfoPageViewModel parentVM;
        private readonly HotelDataAccess hotel;

        private Reservation selected;
        public Reservation Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));

                if (value != null)
                {
                    TotalCost = hotel.GetReservationCost(Selected.Id).ToString();
                    SetUIListText();
                }
                InfoTabVisibilty = value == null ? Visibility.Collapsed : Visibility.Visible;
                NullInfoTabVisibilty = value != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private string totalCost;
        public string TotalCost
        {
            get => totalCost;
            set
            {
                totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }   


        private string discountText;
        public string DiscountText
        {
            get => discountText;
            set
            {
                discountText = value;
                OnPropertyChanged(nameof(DiscountText));
            }
        }
        private string serviceText;
        public string ServiceText
        {
            get => serviceText;
            set
            {
                serviceText = value;
                OnPropertyChanged(nameof(ServiceText));
            }
        }


        // Show Nothing On Nothing Selected
        private Visibility _infoTabVisibilty = Visibility.Collapsed;
        public Visibility InfoTabVisibilty
        {
            get => _infoTabVisibilty;
            set
            {
                _infoTabVisibilty = value;
                OnPropertyChanged(nameof(InfoTabVisibilty));
            }
        }
        private Visibility _nullInfoTabVisibilty = Visibility.Visible;
        public Visibility NullInfoTabVisibilty
        {
            get => _nullInfoTabVisibilty;
            set
            {
                _nullInfoTabVisibilty = value;
                OnPropertyChanged(nameof(NullInfoTabVisibilty));
            }
        }


        public ICommand NavigateEditCommand { get; }
        public ICommand EnableEditCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ReservationInfoTabViewModel(NavigationStore navigationStore, ReservationInfoPageViewModel parentVM, HotelDataAccess hotel)
        {
            this.parentVM = parentVM;
            this.hotel = hotel;
            parentVM.PropertyChanged += OnParentViewModelPropertyChanged;
            Selected = parentVM.SelectedReservation;
            

            NavigateEditCommand = new NavigateCommand(navigationStore, () => new ReservationEditTabViewModel(navigationStore, parentVM, hotel));
            EnableEditCommand = new RelayCommand(EnableEdit, CanEnableEdit);
            DeleteReservationCommand = new RelayCommand(DeleteReservation);
        }

        private void EnableEdit(object parameter)
        {
            NavigateEditCommand.Execute(null);
        }

        private bool CanEnableEdit(object parameter)
        {
            if (Selected == null) return false;
            return Selected.CheckOutDate.ToDateTime(TimeOnly.MaxValue) >= DateTime.Now;
        }

        private void OnParentViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(parentVM.SelectedReservation))
            {
                Selected = parentVM.SelectedReservation;
                if (Selected != null)
                    SetUIListText();
            }
        }

        private void SetUIListText()
        {
            if (Selected.ReservedServices.Count == 0)
                ServiceText = "None";
            else
            {
                string serviceListText = "";
                for (int i = 0; i < Selected.ReservedServices.Count - 1; ++i)
                {
                    serviceListText += Selected.ReservedServices[i].Name + ", ";
                }
                serviceListText += Selected.ReservedServices[Selected.ReservedServices.Count - 1].Name;
                ServiceText = serviceListText;
            }

            if (Selected.AppliedDiscounts.Count == 0)
                DiscountText = "None";
            else
            {
                string discountListText = "";
                for (int i = 0; i < Selected.AppliedDiscounts.Count - 1; ++i)
                {
                    discountListText += Selected.AppliedDiscounts[i].Name + " "
                        + (int)(Selected.AppliedDiscounts[i].Value * 100) + "%, ";
                }
                discountListText += Selected.AppliedDiscounts[Selected.AppliedDiscounts.Count - 1].Name + " "
                    + (int)(Selected.AppliedDiscounts[Selected.AppliedDiscounts.Count - 1].Value * 100) + "%";
                DiscountText = discountListText;
            }
        }

        private void DeleteReservation(object parameter)
        {
            parentVM.DeleteReservation();
        }
    }
}
