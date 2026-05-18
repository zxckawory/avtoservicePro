using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using avtoservicePro.Context;
using avtoservicePro.Models;
using System;
using System.Linq;

namespace avtoservicePro
{
    public partial class MainWindow : Window
    {
        WindowNotificationManager notificationManager;
        AvtoserviceContext context = new();
        public MainWindow()
        {
            InitializeComponent();
            notificationManager = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.BottomLeft,
                MaxItems = 3
            };
        }

        private void EnterButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            IsEnabled = false;

            User? user = context.Users.FirstOrDefault(x => x.Login == LoginTextBox.Text && x.Password == PasswordTextBox.Text);
            if (user == null)
            {
                notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
                IsEnabled = true;
                return;
            }
            else
            {
                new MenuWindow(user).Show();
                Close();
            }
        }

        private void RegistrationHLButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new RegistrationWindow().Show();
            Close();
        }
    }
}