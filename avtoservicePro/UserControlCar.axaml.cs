using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ursa.Controls;

namespace avtoservicePro;

public partial class UserControlCar : UserControl
{
    public List<Car> cars = null!;
    AvtoserviceContext context = new();
    Ursa.Controls.WindowNotificationManager notificationManager;
    MenuWindow menuWindow1 = new();
    User user1 = new();
    public UserControlCar()
    {
        InitializeComponent();
        notificationManager = new Ursa.Controls.WindowNotificationManager();
    }

    public UserControlCar(MenuWindow menuWindow, User user)
    {
        InitializeComponent();
        menuWindow1 = menuWindow;
        user1 = user;
        Loaded += async (_, _) => await Load();
        notificationManager = new Ursa.Controls.WindowNotificationManager(menuWindow)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
    }

    public async Task Load()
    {
        context = new AvtoserviceContext();

        if(user1.RoleId == 1)
        {
            cars = new(context.Cars.Include(x => x.CarType).Include(x => x.FuelType).Include(x => x.Orders).Where(x => x.UserId == user1.Id));
        }
        else
        {
            cars = new(context.Cars.Include(x => x.CarType).Include(x => x.FuelType).Include(x => x.Orders));
        }
        CarItemsControl.ItemsSource = cars;
    }

    private void AddCarButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new NewCarWindow(this, user1).ShowDialog(menuWindow1);
    }

    private void UpdateCarButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if((sender as Button)!.DataContext is Car car && car != null)
        {
            new NewCarWindow(car, this).ShowDialog(menuWindow1);
        }
    }

    private async void DeleteCarButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if ((sender as Button)!.DataContext is Car car && car != null)
        {
            var a = await OverlayMessageBox.ShowAsync("Удалить машину?", "Удаление", null, MessageBoxIcon.Warning, MessageBoxButton.YesNo);
            if (a == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var selectedCar = context.Cars.First(x => x.Id == car.Id);
                if (selectedCar != null)
                {
                    context.Cars.Remove(selectedCar);
                    await context.SaveChangesAsync();
                }
                await Load();

                var b = await OverlayMessageBox.ShowAsync("Успешно", null, null, MessageBoxIcon.Success);
            }
        }
    }
}