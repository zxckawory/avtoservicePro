using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ursa.Controls;

namespace avtoservicePro;

public partial class UserControlService : UserControl
{
    public List<Service> services = null!;
    AvtoserviceContext context = new();
    Ursa.Controls.WindowNotificationManager notificationManager;
    MenuWindow menuWindow1 = new();
    public bool IsAdmin { get; set; }
    public UserControlService()
    {
        InitializeComponent();
        notificationManager = new Ursa.Controls.WindowNotificationManager();
    }

    public UserControlService(MenuWindow menuWindow, User user)
    {
        InitializeComponent();
        menuWindow1 = menuWindow;
        Loaded += async (_, _) => await Load();
        notificationManager = new Ursa.Controls.WindowNotificationManager(menuWindow)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };

        if(user.RoleId == 1)
        {
            NavigationExpander.IsVisible = false;
            IsAdmin = false;
        }
        else
        {
            NavigationExpander.IsVisible = true;
            IsAdmin = true;

        }
    }

    public async Task Load()
    {
        context = new AvtoserviceContext();
        services = new(context.Services);
        ServiceItemsControl.ItemsSource = services;
    }

    private void AddServiceButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new NewServiceWindow(this).ShowDialog(menuWindow1);
    }

    private void UpdateServiceButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if ((sender as Button)!.DataContext is Service service && service != null)
        {
            new NewServiceWindow(service, this).ShowDialog(menuWindow1);
        }
    }

    private async void DeleteServiceButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if ((sender as Button)!.DataContext is Service service && service != null)
        {
            var a = await OverlayMessageBox.ShowAsync("Удалить услугу?", "Удаление", null, MessageBoxIcon.Warning, MessageBoxButton.YesNo);
            if (a == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var selectedService = context.Services.First(x => x.Id == service.Id);
                if (selectedService != null)
                {
                    context.Services.Remove(selectedService);
                    await context.SaveChangesAsync();
                }
                await Load();

                var b = await OverlayMessageBox.ShowAsync("Успешно", null, null, MessageBoxIcon.Success);
            }
        }
    }
}