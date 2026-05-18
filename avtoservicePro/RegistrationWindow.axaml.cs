using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using System;
using System.Linq;
using System.Threading;

namespace avtoservicePro;

public partial class RegistrationWindow : Window
{
    WindowNotificationManager notificationManager;
    AvtoserviceContext context = new();
    public RegistrationWindow()
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
    }

    private void NewUserButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;

        if(string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(PhoneNumberTextBox.Text))
        {
            notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
            IsEnabled = true;
            return;
        }
        else
        {
            var user = new User
            {
                Id = context.Users.Max(x => x.Id) + 1,
                Name = NameTextBox.Text!,
                Login = LoginTextBox.Text!,
                Password = PasswordTextBox.Text!,
                PhoneNumber = PhoneNumberTextBox.Text!,
                RoleId = 1
            };

            if(context.Users.Any(x => x.Login == user.Login))
            {
                notificationManager.Show("Пользователь с таким логином уже существует", NotificationType.Error, TimeSpan.FromSeconds(3));
                IsEnabled = true;
                return;
            }

            context.Users.Add(user);
            context.SaveChanges();

            notificationManager.Show("Пользователь успешно зарегестирован", NotificationType.Success, TimeSpan.FromSeconds(3));
            Thread.Sleep(4000);

            new MainWindow().Show();
            Close();
            return;
        }
    }

    private void EnterHLButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}