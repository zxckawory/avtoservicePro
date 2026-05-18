using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using avtoservicePro.Context;
using avtoservicePro.Models;
using System.Linq;

namespace avtoservicePro;

public partial class MenuWindow : Window
{
    private User user1 = new();
    AvtoserviceContext context = new();
    public MenuWindow()
    {
        InitializeComponent();
    }

    public MenuWindow(User user)
    {
        InitializeComponent();
        user1 = user;
        FullNameTextBlock.Text = user1.Name;
        RoleTextBlock.Text = context.Users.Where(x => x.Id == user1.Id).Select(x => x.Role!.Name).FirstOrDefault()!.ToString();

        if(user1.RoleId == 1)
        {
            UserTabItem.IsVisible = false;
        }
        else
        {
            UserTabItem.IsVisible = true;
        }

        CarTabItem.Content = new UserControlCar(this, user1);
        ServiceTabItem.Content = new UserControlService(this, user1);
        UserTabItem.Content = new UserControlUser(this);
        OrderTabItem.Content = new UserControlOrder(this,user1);
    }

    private void ExitButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}