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

public partial class UserControlOrder : UserControl
{
    public List<Order> orders = null!;
    AvtoserviceContext context = new();
    Ursa.Controls.WindowNotificationManager notificationManager;
    MenuWindow menuWindow1 = new();
    User user1 = new();
    public UserControlOrder()
    {
        InitializeComponent();
        notificationManager = new Ursa.Controls.WindowNotificationManager();
    }

    public UserControlOrder(MenuWindow menuWindow, User user)
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
        if (user1.RoleId == 2)
        {
            orders = new(context.Orders.Include(x => x.Status).Include(x => x.Services).Include(x => x.Car).ThenInclude(x => x.User));
        }
        else
        {
            orders = new(context.Orders.Include(x => x.Status).Include(x => x.Services).Include(x => x.Car).ThenInclude(x => x.User).Where(x => x.Car.UserId == user1.Id));
        }
        OrdersItemsControl.ItemsSource = orders;

        if (orders.Count == 0)
        {
            OrdersCount.IsVisible = true;
        }

        var services = context.Services.ToList();
        services.Insert(0, new Service
        {
            Id = 0,
            ServiceName = "Услуга"
        });
        ServiceComboBox.ItemsSource = services;
        ServiceComboBox.SelectedIndex = 0;

        var statuses = context.Statuses.ToList();
        statuses.Insert(0, new Status
        {
            Id = 0,
            Status1 = "Статус"
        });
        StatusComoboBox.ItemsSource = statuses;
        StatusComoboBox.SelectedIndex = 0;
    }

    private void AddOrderButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new NewOrderWindow(this, user1).Show();
    }

    private void UpdateOrderButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if ((sender as Button)!.DataContext is Order order && order != null)
        {
            new NewOrderWindow(order, this, user1).ShowDialog(menuWindow1);
        }
    }

    private async void DeleteOrderButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if ((sender as Button)!.DataContext is Order order && order != null)
        {
            var a = await OverlayMessageBox.ShowAsync("Удалить заказ?", "Удаление", null, MessageBoxIcon.Warning, MessageBoxButton.YesNo);
            if (a == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var selectedOrder = context.Orders.First(x => x.Id == order.Id);
                if (selectedOrder != null)
                {
                    context.Orders.Remove(selectedOrder);
                    await context.SaveChangesAsync();
                }
                await Load();

                var b = await OverlayMessageBox.ShowAsync("Успешно", null, null, MessageBoxIcon.Success);
            }
        }
    }

    private void Sort()
    {
        List<Order> sortedOrders = orders;
        if (ServiceComboBox.SelectedItem is Service service)
        {
            if (service.Id != 0)
            {
                sortedOrders = sortedOrders.Where(x => x.Services.Any(x => x.Id == service.Id)).ToList();
            }
        }
        if(StatusComoboBox.SelectedItem is Status status)
        {
            if(status.Id != 0)
            {
                sortedOrders = sortedOrders.Where(x => x.Status.Id == status.Id).ToList();
            }
        }
        if (SearchTextBox.Text is string searchText)
        {
            if (searchText != null)
            {
                sortedOrders = sortedOrders.Where(x => x.Car.CarName.ToLower().Contains(searchText.ToLower()) ||
                x.Car.CarNumber.ToLower().Contains(searchText.ToLower()) ||
                x.Services.Any(x => x.ServiceName.ToLower().Contains(searchText.ToLower()))).ToList();
            }
        }
        OrdersItemsControl.ItemsSource = sortedOrders;
    }

    private void ServiceComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        Sort();
    }

    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!IsLoaded) return;
        Sort();
    }

    private void StatusComoboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        Sort();
    }
}