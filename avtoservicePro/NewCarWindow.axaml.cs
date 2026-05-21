using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using avtoservicePro.Context;
using avtoservicePro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
    private string newImagePath = string.Empty;
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
        RemoveImage.IsEnabled = false;
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

        try
        {
            CarImage.Source = new Bitmap($"Assets/{car1.Image}");
            RemoveImage.IsEnabled = true;
        }
        catch
        {
            CarImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://avtoservicePro/Assets/image_placeholder_resource.png")));
            RemoveImage.IsEnabled = false;
        }
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
        if (string.IsNullOrEmpty(CarNameTextBox.Text) || string.IsNullOrEmpty(CarNameTextBox.Text) || CarTypeComboBox.SelectedItem == null || string.IsNullOrEmpty(HorsePowerTextBox.Text) || string.IsNullOrEmpty(EngineVolumeTextBox.Text) || FuelTypeComboBox.SelectedItem == null || string.IsNullOrEmpty(YearTextBox.Text) || string.IsNullOrEmpty(MileageTextBox.Text))
        {
            notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
            return;
        }

        if (IsEdit == true)
        {
            var car = context.Cars.Include(x => x.CarType).Include(x => x.FuelType).Include(x => x.User).First(x => x.Id == car1.Id);

            car.CarName = CarNameTextBox.Text;
            car.CarNumber = CarNumberTextBox.Text!;
            car.CarTypeId = context.CarTypes.First(x => x.Type == CarTypeComboBox.Text).Id;
            car.HorsePower = Convert.ToInt32(HorsePowerTextBox.Text);
            car.EngineVolume = Convert.ToDecimal(EngineVolumeTextBox.Text);
            car.FuelTypeId = context.FuelTypes.First(x => x.Type == FuelTypeComboBox.Text).Id;
            car.Year = Convert.ToInt32(YearTextBox.Text);
            car.Mileage = Convert.ToInt32(MileageTextBox.Text);

            if (!string.IsNullOrEmpty(newImagePath))
            {
                if (string.IsNullOrEmpty(car.Image))
                {
                    car.Image = Guid.NewGuid().ToString() +
                                Path.GetExtension(newImagePath);
                }

                var path = Path.Combine(
                    AppContext.BaseDirectory,
                    "Assets",
                    car.Image
                );

                File.Copy(newImagePath, path, true);
            }

            context.Cars.Update(car);
        }
        else
        {
            car1.Id = context.Cars.Max(x => x.Id) + 1;
            car1.CarName = CarNameTextBox.Text;
            car1.CarNumber = CarNumberTextBox.Text!;
            car1.CarTypeId = (CarTypeComboBox.SelectedItem as CarType)!.Id;
            car1.HorsePower = Convert.ToInt32(HorsePowerTextBox.Text);
            car1.EngineVolume = Convert.ToDecimal(EngineVolumeTextBox.Text);
            car1.FuelTypeId = (FuelTypeComboBox.SelectedItem as FuelType)!.Id;
            car1.Year = Convert.ToInt32(YearTextBox.Text);
            car1.Mileage = Convert.ToInt32(MileageTextBox.Text);
            car1.UserId = user1.Id;

            if (!string.IsNullOrEmpty(newImagePath) &&
    !string.IsNullOrEmpty(car1.Image))
            {
                var path = Path.Combine(
                    AppContext.BaseDirectory,
                    "Assets",
                    car1.Image
                );

                File.Copy(newImagePath, path, true);
            }

            context.Cars.Add(car1);
        }

        await context.SaveChangesAsync();
        await userControlCar1.Load();

        notificationManager.Show("Успешно", NotificationType.Success, TimeSpan.FromSeconds(3));
        return;
    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel == null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите изображение",
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>
            {
                new FilePickerFileType("Изображения")
                {
                    Patterns = ["*.jpg", "*.jpeg", "*.png", "*.bmp"]
                }
            }
        });

        if (files.Count > 0)
        {
            newImagePath = files[0].Path.AbsolutePath;
            CarImage.Source = new Bitmap(newImagePath);
            car1.Image = Guid.NewGuid().ToString() + Path.GetExtension(newImagePath);
            RemoveImage.IsEnabled = true;
        }
    }

    private void RemoveImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CarImage.Source = new Bitmap("Assets/image_placeholder.png");
        RemoveImage.IsEnabled = false;
    }
}