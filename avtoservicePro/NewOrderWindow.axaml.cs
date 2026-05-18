using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using avtoservicePro.Context;
using avtoservicePro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace avtoservicePro;

public partial class NewOrderWindow : Window
{
    UserControlOrder userControlOrder1 = new();
    private Order order1 { get; set; } = null!;
    AvtoserviceContext context = new();
    WindowNotificationManager notificationManager;
    private bool IsEdit = false;
    private List<Service> services = null!;
    private string newImagePath = string.Empty;
    User user1 = new();
    public NewOrderWindow()
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager();
    }

    public NewOrderWindow(UserControlOrder userControlOrder, User user)
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        userControlOrder1 = userControlOrder;
        order1 = new Order();
        user1 = user;
        Load();
        RemoveImage.IsEnabled = false;
    }

    public NewOrderWindow(Order order, UserControlOrder userControlOrder, User user)
    {
        InitializeComponent();
        notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomLeft,
            MaxItems = 3
        };
        userControlOrder1 = userControlOrder;
        order1 = order;
        IsEdit = true;
        user1 = user;
        Load();

        Title = "Изменение заказа";
        TitleTextBox.Text = "Изменение заказа";

        CarComboBox.SelectedItem = order1.Car;
        DescriptionTextBox.Text = order1.Description;
        services = context.Services.ToList();
        var selectedService = order1.Services.Select(x => x.Id).ToList();
        foreach( var service in services)
        {
            if (selectedService.Contains(service.Id))
            {
                ServicesBox.SelectedItems?.Add(service);
            }
        }
        try
        {
            OrderImage.Source = new Bitmap($"Assets/{order1.Image}");
            RemoveImage.IsEnabled = true;
        }
        catch
        {
            OrderImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://avtoservicePro/Assets/image_placeholder_resource.png")));
            RemoveImage.IsEnabled = false;
        }
    }

    private void Load()
    {
        if(user1.RoleId == 2)
        {
            CarComboBox.ItemsSource = context.Cars.ToList();
        }
        else
        {
            CarComboBox.ItemsSource = context.Cars.Where(x => x.UserId == user1.Id).ToList();
        }
        services = context.Services.ToList();
        ServicesBox.ItemsSource = services;
    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel == null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите изображение",
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>
            {
                new FilePickerFileType("Изображения")
                {
                    Patterns = ["*.jpg", "*.jpeg", "*.png", "*.bmp"]
                }
            }
        });

        if (files.Count > 0)
        {
            newImagePath = files[0].Path.AbsolutePath;
            OrderImage.Source = new Bitmap(newImagePath);
            order1.Image = Guid.NewGuid().ToString() + Path.GetExtension(newImagePath);
            RemoveImage.IsEnabled = true;
        }
    }

    private void RemoveImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OrderImage.Source = new Bitmap("Assets/image_placeholder.png");
        RemoveImage.IsEnabled = false;
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(CarComboBox.SelectedItem == null || ServicesBox.SelectedItem == null)
        {
            notificationManager.Show("Поля заполнены неверно", NotificationType.Error, TimeSpan.FromSeconds(3));
            return;
        }

        if(IsEdit == true)
        {
            var order = context.Orders.Include(x => x.Services).Include(x => x.Car).First(x => x.Id == order1.Id);

            order.Description = DescriptionTextBox.Text;
            order.CarId = (CarComboBox.SelectedItem as Car)!.Id;

            if (!string.IsNullOrEmpty(newImagePath))
            {
                File.Copy(newImagePath, $"Assets/{order.Image}", true);
            }

            var selectedService = ServicesBox.SelectedItems!.OfType<Service>().Select(x => x.Id).ToList();
            order.Services = await context.Services.Where(x => selectedService.Contains(x.Id)).ToListAsync();

            context.Orders.Update(order);
        }
        else
        {
            order1.Description = DescriptionTextBox.Text;
            order1.CarId = (CarComboBox.SelectedItem as Car)!.Id;
            order1.OrderDayTime = DateTime.Now;

            if (!string.IsNullOrEmpty(newImagePath))
            {
                File.Copy(newImagePath, $"Assets/{order1.Image}", true);
            }

            foreach (var services in ServicesBox.SelectedItems!)
            {
                if (services is Service service)
                {
                    context.Attach(service);

                    order1.Services.Add(service);
                }
            }

            context.Orders.Add(order1);
        }

        await context.SaveChangesAsync();
        await userControlOrder1.Load();

        notificationManager.Show("Успешно", NotificationType.Success, TimeSpan.FromSeconds(3));
        return;
    }
}