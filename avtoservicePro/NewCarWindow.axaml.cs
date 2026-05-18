using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace avtoservicePro;

public partial class NewCarWindow : Window
{
    UserControlCar userControlCar1 = new();
    private Car car1 { get; set; } = null!;
    AvtoserviceContext context = new();
    WindowNotificationManager notificationManager;
    private bool IsEdit = false;
    User user1 = new();
    public NewCarWindow()
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager();
    }

    public NewCarWindow(UserControlCar userControlCar, User user)
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        userControlCar1 = userControlCar;
        car1 = new Car();
        user1 = user;
        Load();
    }

    public NewCarWindow(Car car, UserControlCar userControlCar)
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        userControlCar1 = userControlCar;
        car1 = car;
        IsEdit = true;
        Load();

        Title = "Изменение машины";
        TitleTextBox.Text = "Изменение автомобиля";

        CarNameTextBox.Text = car1.CarName;
        CarNumberTextBox.Text = car1.CarNumber;
        CarTypeComboBox.SelectedItem = car1.CarType;
        HorsePowerTextBox.Text = Convert.ToString(car1.HorsePower);
        EngineVolumeTextBox.Text = Convert.ToString(car1.EngineVolume);
        FuelTypeComboBox.SelectedItem = car1.FuelType;
        YearTextBox.Text = Convert.ToString(car1.Year);
        MileageTextBox.Text = Convert.ToString(car1.Mileage);
    }

    private void Load()
    {
        CarTypeComboBox.ItemsSource = context.CarTypes.ToList();
        FuelTypeComboBox.ItemsSource = context.FuelTypes.ToList();
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(CarNameTextBox.Text) || string.IsNullOrEmpty(CarNameTextBox.Text) || CarTypeComboBox.SelectedItem == null || string.IsNullOrEmpty(HorsePowerTextBox.Text) || string.IsNullOrEmpty(EngineVolumeTextBox.Text) || FuelTypeComboBox.SelectedItem == null || string.IsNullOrEmpty(YearTextBox.Text) || string.IsNullOrEmpty(MileageTextBox.Text))
        {
            notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
            return;
        }

        if(IsEdit == true)
        {
            var car = context.Cars.Include(x => x.CarType).Include(x => x.FuelType).Include(x => x.User).First(x => x.Id == car1.Id);

            car.CarName = CarNameTextBox.Text;
            car.CarNumber = CarNumberTextBox.Text!;
            car.CarTypeId = context.CarTypes.First(x => x.Type == CarTypeComboBox.Text).Id;
            car.HorsePower = Convert.ToInt32(HorsePowerTextBox.Text);
            car.EngineVolume = Convert.ToInt32(EngineVolumeTextBox.Text);
            car.FuelTypeId = context.FuelTypes.First(x => x.Type == FuelTypeComboBox.Text).Id;
            car.Year = Convert.ToInt32(YearTextBox.Text);
            car.Mileage = Convert.ToInt32(MileageTextBox.Text);

            context.Cars.Update(car);
        }
        else
        {
            car1.Id = context.Cars.Max(x => x.Id) + 1;
            car1.CarName = CarNameTextBox.Text;
            car1.CarNumber = CarNumberTextBox.Text!;
            car1.CarTypeId = (CarTypeComboBox.SelectedItem as CarType)!.Id;
            car1.HorsePower = Convert.ToInt32(HorsePowerTextBox.Text);
            car1.EngineVolume = Convert.ToInt32(EngineVolumeTextBox.Text);
            car1.FuelTypeId = (FuelTypeComboBox.SelectedItem as FuelType)!.Id;
            car1.Year = Convert.ToInt32(YearTextBox.Text);
            car1.Mileage = Convert.ToInt32(MileageTextBox.Text);
            car1.UserId = user1.Id;

            context.Cars.Add(car1);
        }

        await context.SaveChangesAsync();
        await userControlCar1.Load();

        notificationManager.Show("Успешно", NotificationType.Success, TimeSpan.FromSeconds(3));
        return;
    }
}