using HotelWPF.Command;
using HotelWPF.DataAccess;
using HotelWPF.Model;
using HotelWPF.Store;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelWPF.ViewModel.StatisticsModel
{
    public class StatisticsInfoPageViewModel : ViewModelBase
    {
        HotelDataAccess hotel;

        private int selectedCBItem = 0;
        public int SelectedCBItem
        {
            get => selectedCBItem;
            set
            {
                selectedCBItem = value;
                OnPropertyChanged(nameof(SelectedCBItem));
                Replot();
            }
        }

        private DateTime dateFrom = DateTime.Now.AddDays(-7);
        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                dateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
                Replot();
            }
        }
        private DateTime dateTo = DateTime.Now;
        public DateTime DateTo
        {
            get => dateTo;
            set
            {
                dateTo = value;
                OnPropertyChanged(nameof(DateTo));
                Replot();
            }
        }

        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get => plotModel;
            set
            {
                plotModel = value;
                OnPropertyChanged(nameof(PlotModel));
            }
        }

        private Visibility periodVisibility = Visibility.Visible;
        public Visibility PeriodVisibility
        {
            get => periodVisibility;
            set
            {
                periodVisibility = value;
                OnPropertyChanged(nameof(PeriodVisibility));
            }
        }

        public ICommand AddPaymentCommand { get; }
        public ICommand ExportCommand { get; }

        public StatisticsInfoPageViewModel(NavigationStore navigationStore, HotelDataAccess hotel)
        {
            this.hotel = hotel;
            PlotModel = new PlotModel();
            Replot();

            AddPaymentCommand = new NavigateCommand(navigationStore, () => new PaymentAddPageViewModel(navigationStore, hotel));
            ExportCommand = new RelayCommand(ExportData);
        }

        private void ExportData(object parameter)
        {
            hotel.ExportPaymentData(DateFrom, DateTo);
        }

        private void Replot()
        {
            switch (SelectedCBItem)
            {
                case 0:
                    PeriodVisibility = Visibility.Visible;
                    PlotProfitGraph();
                    break;
                case 1:
                    PeriodVisibility = Visibility.Hidden;
                    PlotServiceHistogram();
                    break;
                case 2:
                    PeriodVisibility = Visibility.Hidden;
                    PlotRoomTypeHistogram();
                    break;
                default:
                    break;
            }
        }

        private void PlotProfitGraph()
        {
            PlotModel.Title = "Profit";
            PlotModel.Series.Clear();
            PlotModel.Axes.Clear();

            var data = new List<Tuple<DateTime, float>>();
            var processedData = new Dictionary<double, float>();
            var dateTimeAxisData = new List<double>();
            int daysDiff = (DateTo - DateFrom).Days;
            double step = (DateTo - DateFrom).TotalSeconds / 50;


            // Profit
            List<Payment> payments = hotel.GetPaymentsByPeriod(DateFrom, DateTo);
            foreach (var payment in payments)
            {
                data.Add(new Tuple<DateTime, float>(payment.PaymentDate.ToDateTime(TimeOnly.MinValue), payment.AmountPaid));
            }

            if (daysDiff < 100)
            {
                foreach (var tuple in data)
                {
                    if (processedData.ContainsKey(DateTimeAxis.ToDouble(tuple.Item1)))
                    {
                        processedData[DateTimeAxis.ToDouble(tuple.Item1)] += tuple.Item2;
                    }
                    else
                    {
                        processedData[DateTimeAxis.ToDouble(tuple.Item1)] = tuple.Item2;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 51; ++i)
                    processedData[DateTimeAxis.ToDouble(DateFrom.AddSeconds(i * step))] = 0;

                foreach (var tuple in data)
                {
                    int currentStep = (int)(tuple.Item1 - DateFrom).TotalSeconds / (int)step;
                    processedData[DateTimeAxis.ToDouble(DateFrom.AddSeconds(currentStep * step))] += tuple.Item2;
                }
            }

            var unsortedLineSeries = new LineSeries();
            var sortedLineSeries = new LineSeries();

            foreach (var pair in processedData)
            {
                unsortedLineSeries.Points.Add(new DataPoint(pair.Key, pair.Value));
            }
            unsortedLineSeries.Points.OrderBy(e => e.X).ToList().ForEach(sortedLineSeries.Points.Add);
            PlotModel.Series.Add(sortedLineSeries);


            //Expenses
            float totalSalary = hotel.GetTotalSalary();
            var processedExpenses = new Dictionary<double, float>();

            var expenseData = new List<Tuple<DateTime, float>>();
            List<Expense> expenses = hotel.GetExpenses(DateFrom, DateTo);
            foreach (var expense in expenses)
            {
                expenseData.Add(new Tuple<DateTime, float>(expense.Date, expense.Value));
            }

            for (int i = 0; i < 51; ++i)
                processedExpenses[DateTimeAxis.ToDouble(DateFrom.AddSeconds(i * step))] = (float)(totalSalary * step / 2629743.83);

            foreach (var tuple in expenseData)
            {
                int currentStep = (int)(tuple.Item1 - DateFrom).TotalSeconds / (int)step;
                processedExpenses[DateTimeAxis.ToDouble(DateFrom.AddSeconds(currentStep * step))] += tuple.Item2;
            }

            var expenseLineSeries = new LineSeries();
            expenseLineSeries.Color = OxyColors.Red;
            foreach (var pair in processedExpenses)
            {
                expenseLineSeries.Points.Add(new DataPoint(pair.Key, pair.Value));
            }
            PlotModel.Series.Add(expenseLineSeries);

            //


            PlotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                StringFormat = "yyyy-MM-dd",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Value" });
            PlotModel.InvalidatePlot(true);

            OnPropertyChanged(nameof(PlotModel));
        }

        private void PlotServiceHistogram()
        {
            PlotModel.Title = "Service Popularity";
            PlotModel.Series.Clear();
            PlotModel.Axes.Clear();

            var data = new Dictionary<string, int>();
            hotel.GetServices().Select(e => e.Name).ToList().ForEach(e => data[e] = hotel.FindServiceCountByName(e));

            var barSeries = new BarSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
            };

            foreach (var kvp in data)
            {
                barSeries.Items.Add(new BarItem { Value = kvp.Value });
            }
            PlotModel.Series.Add(barSeries);

            PlotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Services",
                ItemsSource = data.Keys,
            });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Frequency" });

            PlotModel.InvalidatePlot(true);
            OnPropertyChanged(nameof(PlotModel));
        }

        private void PlotRoomTypeHistogram()
        {
            PlotModel.Title = "Room Type Popularity";
            PlotModel.Series.Clear();
            PlotModel.Axes.Clear();

            var data = new Dictionary<string, int>();
            hotel.GetRoomTypes().Select(e => e.Name).ToList().ForEach(e => data[e] = hotel.FindRoomTypeCountByName(e));

            var barSeries = new BarSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
            };

            foreach (var kvp in data)
            {
                barSeries.Items.Add(new BarItem { Value = kvp.Value });
            }
            PlotModel.Series.Add(barSeries);

            PlotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Room Types",
                ItemsSource = data.Keys,
            });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Frequency" });

            PlotModel.InvalidatePlot(true);
            OnPropertyChanged(nameof(PlotModel));
        }
    }
}
