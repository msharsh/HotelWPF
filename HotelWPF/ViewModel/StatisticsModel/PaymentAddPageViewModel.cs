using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelWPF.ViewModel.StatisticsModel
{
    public class PaymentAddPageViewModel : ViewModelBase
    {
        private readonly HotelDataAccess hotel;
        private ObservableCollection<string> unpaidReservations = new ObservableCollection<string>();
        public ObservableCollection<string> UnpaidReservations
        {
            get => unpaidReservations;
            set
            {
                unpaidReservations = value;
                OnPropertyChanged(nameof(UnpaidReservations));
            }
        }
        private int selectedReservation;
        public int SelectedReservation
        {
            get => selectedReservation;
            set
            {
                selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
                SetAmountText();
            }
        }
        private ComboBoxItem selectedPaymentMethod;
        public ComboBoxItem SelectedPaymentMethod
        {
            get => selectedPaymentMethod;
            set
            {
                selectedPaymentMethod = value;
                OnPropertyChanged(nameof(SelectedPaymentMethod));
            }
        }
        private string toPayText;
        public string ToPayText
        {
            get => toPayText;
            set
            {
                toPayText = value;
                OnPropertyChanged(nameof(ToPayText));
            }
        }
        private string paymentAmountValue;
        public string PaymentAmountValue
        {
            get => paymentAmountValue;
            set
            {
                paymentAmountValue = value;

                float amountPaid;
                if (float.TryParse(PaymentAmountValue, out amountPaid))
                    if (amountPaid > amountToPay)
                        paymentAmountValue = amountToPay.ToString();

                OnPropertyChanged(nameof(PaymentAmountValue));
            }
        }
        private float amountToPay;


        public ICommand NavigateInfoCommand { get; }
        public ICommand SubmitCommand { get; }

        public PaymentAddPageViewModel(NavigationStore navigationStore, HotelDataAccess hotelDataAccess)
        {
            hotel = hotelDataAccess;
            List<Reservation> unpaid = hotel.GetUnpaidReservations();
            unpaid.ForEach(e => UnpaidReservations.Add("Room Number: " + e.Room.RoomNumber + " Guest Name: " + e.GuestName));

            // Calculate For Selected
            SetAmountText();

            NavigateInfoCommand = new NavigateCommand(navigationStore, () => new StatisticsInfoPageViewModel(navigationStore, hotel));
            SubmitCommand = new RelayCommand(SubmitPayment, CanSubmitPayment);
        }

        private void SetAmountText()
        {
            List<Reservation> unpaid = hotel.GetUnpaidReservations();

            if (unpaid.Count > 0)
            {
                float totalCost = hotel.GetReservationCost(unpaid[SelectedReservation].Id);
                List<Payment> payments = hotel.GetPaymentsByReservation(unpaid[SelectedReservation].Id);
                float amountPaid = payments.Sum(e => e.AmountPaid);
                ToPayText = "Payment Amount (" + (totalCost - amountPaid).ToString() + " left)";
                amountToPay = totalCost - amountPaid;
            }
        }

        private void SubmitPayment(object parameter)
        {
            int rId = hotel.GetUnpaidReservations()[SelectedReservation].Id;
            if (hotel.AddPayment(new Payment(
                0, rId,
                float.Parse(PaymentAmountValue), DateOnly.MinValue,
                SelectedPaymentMethod.Content.ToString()
            )))
            {
                NavigateInfoCommand.Execute(null);
            }
        }

        private bool CanSubmitPayment(object parameter)
        {
            float amountPaid;
            if(!float.TryParse(PaymentAmountValue, out amountPaid))
                return false;

            if (amountPaid <= 0) return false;

            return SelectedPaymentMethod != null && SelectedPaymentMethod.Content.ToString() != string.Empty;
        }
    }
}
