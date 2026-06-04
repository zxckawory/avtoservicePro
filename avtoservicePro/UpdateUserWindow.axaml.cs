using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using System;
using System.Linq;

namespace avtoservicePro;

public partial class UpdateUserWindow : Window
{
    private User user1 = new();
    AvtoserviceContext context = new();
    private MenuWindow menuWindow1 = new();
    WindowNotificationManager notificationManager;
    public UpdateUserWindow()
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this);
    }

    public UpdateUserWindow(User user, MenuWindow menuWindow)
    {
        InitializeComponent();
        user1 = user;
        menuWindow1 = menuWindow;
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };

        LoginTextBox.Text = user1.Login;
        PasswordTextBox.Text = user1.Password;
        NameTextBox.Text = user1.Name;
        PhoneNumberTextBox.Text = user1.PhoneNumber;
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(PhoneNumberTextBox.Text))
        {
            notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
            return;
        }
        else
        {
            var user = context.Users.First(x => x.Id == user1.Id);

            user.Login = LoginTextBox.Text;
            user.Password = PasswordTextBox.Text;
            user.Name = NameTextBox.Text;
            user.PhoneNumber = PhoneNumberTextBox.Text;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            menuWindow1.Load();

            notificationManager.Show("Успешно", NotificationType.Success, TimeSpan.FromSeconds(3));
            return;
        }
    }
}