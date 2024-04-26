using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.ViewModel.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Command
{
    public class EditRoomCommand : CommandBase
    {
        private readonly RoomInfoPageViewModel viewModel;
        private readonly HotelDataAccess hotel;
        private Action<object> execute;

        public EditRoomCommand(RoomInfoPageViewModel viewModel, HotelDataAccess hotel, Action<object> execute)
        {
            this.viewModel = viewModel;
            this.hotel = hotel;
            this.execute = execute;

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            RoomViewModel edited = viewModel.EditedRoom;
            string status;
            if (viewModel.SelectedRoomMaintenance)
            {
                status = "Under Maintenance";
            }
            else
            {
                if (viewModel.SelectedCurrentRoomMaintenance)
                {
                    status = "Available";
                }
                else
                {
                    status = edited.Status;
                }
            }
            string selectedRoomType = viewModel.SelectedRoomType;
            if (hotel.UpdateRoom(new Room(
                edited.Id,
                edited.RoomNumber,
                int.Parse(edited.Floor),
                hotel.GetRoomTypes().FirstOrDefault(e => e.Name == selectedRoomType),
                status
                )))
            {
                viewModel.SelectedRoom = null;
                viewModel.LoadData();
                execute?.Invoke(parameter);
            }
        }

        public override bool CanExecute(object? parameter)
        {
            RoomType? selectedRoomType = hotel.GetRoomTypes().FirstOrDefault(e => e.Name == "Single");
            RoomViewModel edited = viewModel.EditedRoom;

            return Room.Validate(edited.RoomNumber, edited.Floor, selectedRoomType) && base.CanExecute(parameter);
        }
    }
}
