using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace avtoservicePro;

public partial class NewServiceWindow : Window
{
    UserControlService userControlService1 = new();
    private Service service1 { get; set; } = null!;
    AvtoserviceContext context = new();
    WindowNotificationManager notificationManager;
    private bool IsEdit = false;
    public NewServiceWindow()
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager();
    }

    public NewServiceWindow(Service service, UserControlService userControlService)
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        userControlService1 = userControlService;
        service1 = service;
        IsEdit = true;

        Title = "Изменение услуги";
        TitleTextBox.Text = "Изменение услуги";

        ServiceTextBox.Text = service1.ServiceName;
        CostTextBox.Text = Convert.ToString(service1.ServiceCost);
    }

    public NewServiceWindow(UserControlService userControlService)
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        userControlService1 = userControlService;
        service1 = new Service();
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(ServiceTextBox.Text) || string.IsNullOrEmpty(CostTextBox.Text))
        {
            notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
            return;
        }
        if(IsEdit == true)
        {
            var service = context.Services.First(x => x.Id == service1.Id);

            service.ServiceName = ServiceTextBox.Text;
            service.ServiceCost = Convert.ToInt32(CostTextBox.Text);

            context.Services.Update(service);
        }
        else
        {
            service1.Id = context.Services.Max(x => x.Id) + 1;
            service1.ServiceName = ServiceTextBox.Text;
            service1.ServiceCost = Convert.ToInt32(CostTextBox.Text);

            context.Services.Add(service1);
        }

        await context.SaveChangesAsync();
        await userControlService1.Load();

        notificationManager.Show("Успешно", NotificationType.Success, TimeSpan.FromSeconds(3));
        return;
    }
}