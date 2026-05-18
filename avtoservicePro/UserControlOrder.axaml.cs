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
        if(user1.RoleId == 2)
        {
            orders = new(context.Orders.Include(x => x.Services).Include(x => x.Car).ThenInclude(x => x.User));
        }
        else
        {
            orders = new(context.Orders.Include(x => x.Services).Include(x => x.Car).ThenInclude(x => x.User).Where(x => x.Id == user1.Id));
        }
        OrdersItemsControl.ItemsSource = orders;
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
}