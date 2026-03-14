using System.Windows;
using System.Windows.Input;
using ShopQA.App.ViewModels;
using ShopQA.Core.Database;

namespace ShopQA.App.Views;

public partial class LoginWindow : Window
{
    private readonly DatabaseHelper _dbHelper;
    
    public LoginWindow()
    {
        InitializeComponent();
        _dbHelper = new DatabaseHelper();
        //  Pass ownsDb: true since Window created the helper
        DataContext = new LoginViewModel(_dbHelper, ownsDb: true);
        Loaded += (s, e) => EmailTextBox.Focus();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            AttemptLogin();
        }
    }

    private void OnLoginClick(object sender, RoutedEventArgs e) => AttemptLogin();

    //  Must be async void to await LoginAsync
    private async void AttemptLogin()
    {
        var email = EmailTextBox.Text?.Trim();
        var password = PasswordBox.Password;
        ErrorMessage.Visibility = Visibility.Collapsed;

        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Please enter your email address.");
            EmailTextBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ShowError("Please enter your password.");
            PasswordBox.Focus();
            return;
        }

        if (DataContext is LoginViewModel vm)
        {
            // Await the async login method
            var result = await vm.LoginAsync(email, password);
            
            if (result.IsSuccess)
            {
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                //  Correct property: ErrorMessage
                ShowError(result.ErrorMessage);
            }
        }
    }

    private void ShowError(string message)
    {
        ErrorMessage.Text = message;
        ErrorMessage.Visibility = Visibility.Visible;
    }

    //  Clean up resources when window closes
    protected override void OnClosed(EventArgs e)
    {
        if (DataContext is IDisposable vm)
            vm.Dispose();
        base.OnClosed(e);
    }
}