using System.Text.RegularExpressions;
namespace LyricsEmbdM.Pages;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
        eKey.Text = string.Empty;
        if (Preferences.Default.ContainsKey("my_key"))
        {
            eKey.Text = Preferences.Default.Get("my_key", "");
        }
        else
        {
            lKey.Text = "Key not found";
            lKey.TextColor = Color.Parse("Red");
        }
    }

    private void bSaveKey_Clicked(object sender, EventArgs e)
    {
        eKey.Text = eKey.Text.Trim();
        if (eKey.Text != string.Empty)
        {
            if (Regex.IsMatch(eKey.Text, @"^[a-zA-Z0-9_-]{64}$"))
            {
                Preferences.Default.Set("my_key", eKey.Text);
                DisplayAlert("Success", "Key saved successfully", "OK");
                lKey.Text = "Key Found";
                lKey.TextColor = Color.Parse("Green");
            }
            else
            {
                DisplayAlert("Error", "NOT VALID API KEY", "OK");
            }
        }
        else
        {
            DisplayAlert("Error", "Key cannot be empty", "OK");
        }
    }
}