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

public partial class UserControlUser : UserControl
{
    public List<User> users = null!;
    AvtoserviceContext context = new();
    Ursa.Controls.WindowNotificationManager notificationManager;
    MenuWindow menuWindow1 = new();
    public UserControlUser()
    {
        InitializeComponent();
        notificationManager = new Ursa.Controls.WindowNotificationManager();
    }
    public UserControlUser(MenuWindow menuWindow)
    {
        InitializeComponent();
        notificationManager = new Ursa.Controls.WindowNotificationManager(menuWindow)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        menuWindow1 = menuWindow;
        Loaded += async (_, _) => await Load();
    }

    private async Task Load()
    {
        context = new AvtoserviceContext();
        users = new(context.Users);
        UserItemsControl.ItemsSource = users;
    }

    private async void DeleteUserButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if ((sender as Button)!.DataContext is User user && user != null)
        {
            var a = await OverlayMessageBox.ShowAsync("Удалить пользователя?", "Удаление", null, MessageBoxIcon.Warning, MessageBoxButton.YesNo);
            if (a == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var selectedUser = context.Users.First(x => x.Id == user.Id);
                if (selectedUser != null)
                {
                    context.Users.Remove(selectedUser);
                    await context.SaveChangesAsync();
                }
                await Load();

                var b = await OverlayMessageBox.ShowAsync("Успешно", null, null, MessageBoxIcon.Success);
            }
        }
    }
}