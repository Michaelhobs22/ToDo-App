using System.Net.Http.Json;
using Newtonsoft.Json;

namespace ToDo_Application;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        var firstName = firstNameEntry.Text;
        var lastName = lastNameEntry.Text;
        var email = emailEntry.Text;
        var password = passwordEntry.Text;
        var confirmPassword = confirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Email and password are required.", "OK");
            return;
        }

        var signupData = new
        {
            first_name = firstName,
            last_name = lastName,
            email = email,
            password = password,
            confirm_password = confirmPassword
        };

        try
        {
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(
                "https://todo-list.dcism.org/signup_action.php",
                signupData
            );

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(content);

            if ((int)result.status == 200)
            {
                await DisplayAlert("Success", (string)result.message, "OK");
                await Navigation.PopAsync();
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

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
