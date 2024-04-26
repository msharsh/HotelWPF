using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using HotelWPF.ViewModel;
using HotelWPF.ViewModel.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelWPF.Command
{
    public class AddRoomCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _createViewModel;
        private readonly RoomAddPageViewModel _viewModel;
        private readonly HotelDataAccess _hotel;

        public AddRoomCommand(RoomAddPageViewModel viewModel, HotelDataAccess hotel, NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _viewModel = viewModel;
            _hotel = hotel;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            if (_hotel.AddRoom(new Room(
                0, _viewModel.RoomNumber,
                int.Parse(_viewModel.Floor),
                _hotel.GetRoomTypes().FirstOrDefault(e => e.Name == _viewModel.SelectedRoomType),
                "Available"
            )))
            {
                _navigationStore.CurrentViewModel = _createViewModel();
            }

        }

        public override bool CanExecute(object? parameter)
        {
            RoomType? selected = _hotel.GetRoomTypes().FirstOrDefault(e => e.Name == _viewModel.SelectedRoomType);

            return Room.Validate(_viewModel.RoomNumber, _viewModel.Floor, selected) && base.CanExecute(parameter);
        }
    }
}
