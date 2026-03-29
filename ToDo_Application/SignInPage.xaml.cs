using System.Net.Http.Json;
using Newtonsoft.Json;

namespace ToDo_Application;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        var email = emailEntry.Text;
        var password = passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Email and password are required.", "OK");
            return;
        }

        try
        {
            using var client = new HttpClient();
            var url = $"https://todo-list.dcism.org/signin_action.php?email={email}&password={password}";
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(content);

            if ((int)result.status == 200)
            {
                int userId = result.data.id;
                await DisplayAlert("Success", "Signed in successfully!", "OK");
                Application.Current.Windows[0].Page = new AppShell();
            }
            else
            {
                await DisplayAlert("Error", (string)result.message, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}
