using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace avtoservicePro;

public partial class UserControlOrderHistory : UserControl
{
    public List<OrderHistory> orders = null!;
    AvtoserviceContext context = new();
    Ursa.Controls.WindowNotificationManager notificationManager;
    MenuWindow menuWindow1 = new();
    User user1 = new();

    public UserControlOrderHistory()
    {
        InitializeComponent();
        notificationManager = new Ursa.Controls.WindowNotificationManager();
    }

    public UserControlOrderHistory(MenuWindow menuWindow, User user)
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
            orders = new(context.OrderHistories.Include(x => x.HistoryType)
                .Include(x => x.Order).ThenInclude(x => x.Services)
                .Include(x => x.Order).ThenInclude(x => x.Car).ThenInclude(x => x.User)
                .Include(x => x.Order).ThenInclude(x => x.Status));
        }
        else
        {
            orders = new(context.OrderHistories.Include(x => x.HistoryType)
                .Include(x => x.Order).ThenInclude(x => x.Services)
                .Include(x => x.Order).ThenInclude(x => x.Car).ThenInclude(x => x.User).Where(x => x.Order.Car.User.Id == user1.Id)
                .Include(x => x.Order).ThenInclude(x => x.Status));
        }
        OrdersItemsControl.ItemsSource = orders;

        if (orders.Count == 0)
        {
            OrdersCount.IsVisible = true;
        }

        var types = context.HistoryTypes.ToList();
        types.Insert(0, new HistoryType
        {
            Id = 0,
            Type = "Статус"
        });
        HistoryTypeComboBox.ItemsSource = types;
        HistoryTypeComboBox.SelectedIndex = 0;
    }

    private void Sort()
    {
        List<OrderHistory> sortedOrders = orders;
        if (HistoryTypeComboBox.SelectedItem is HistoryType type)
        {
            if (type.Id != 0)
            {
                sortedOrders = sortedOrders.Where(x => x.HistoryTypeId == type.Id).ToList();
            }
        }
        if (SearchTextBox.Text is string searchText)
        {
            if (searchText != null)
            {
                sortedOrders = sortedOrders.Where(x => x.Order.Car.CarName.ToLower().Contains(searchText.ToLower()) ||
                x.Order.Car.CarNumber.ToLower().Contains(searchText.ToLower())).ToList();
            }
        }
        switch (DateComboBox.SelectedIndex)
        {
            case 0:
                sortedOrders = sortedOrders.ToList();
                break;
            case 1:
                sortedOrders = sortedOrders.OrderByDescending(x => x.HistoryTime).ToList();
                break;
            case 2:
                sortedOrders = sortedOrders.OrderBy(x => x.HistoryTime).ToList();
                break;
        }
        OrdersItemsControl.ItemsSource = sortedOrders;
    }

    private void HistoryTypeComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        Sort();
    }

    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!IsLoaded) return;
        Sort();
    }

    private void DateComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        Sort();
    }
}