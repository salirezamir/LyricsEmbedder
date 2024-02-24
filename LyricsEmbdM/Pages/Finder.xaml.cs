using TagLib;

namespace LyricsEmbdM.Pages;

public partial class Finder : ContentPage
{
    public Finder()
    {
        InitializeComponent();
    }

    private async void bInput_Clicked(object sender, EventArgs e)
    {
        Genius genius = new Genius();
        string lyrics = await genius.GetLyricsAsync(eTitle.Text, eArt.Text);
        if (lyrics == string.Empty)
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                eLyrics.TextColor = Colors.IndianRed;
            else
                eLyrics.TextColor = Colors.DarkRed;
            eLyrics.Text = "Lyrics Not Available :(";
        }
        else
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                eLyrics.TextColor = Colors.LightGreen;
            else
                eLyrics.TextColor = Colors.Green;
            eLyrics.Text = lyrics;
        }
    }

    private async void bFile_Clicked(object sender, EventArgs e)
    {
        FilePickerFileType customFileType = new FilePickerFileType(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
                        { DevicePlatform.iOS, new[] { "public.audio" } }, // UTType values
                        { DevicePlatform.Android, new[] { "audio/*" } }, // MIME type
                        { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".wma", ".m4a", ".flac" } }, // file extension
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                        { DevicePlatform.macOS, new[] { ".mp3", ".wav", ".wma", ".m4a", ".flac" } }, // UTType values
        });
        PickOptions options = new()
        {
            PickerTitle = "Please select audio file",
            FileTypes = customFileType,
        };
        var fileResult = await FilePicker.PickAsync(options);
        var tfile = TagLib.File.Create(fileResult.FullPath);
        Genius genius = new Genius();
        string lyrics = await genius.GetLyricsAsync(tfile.Tag.Title, tfile.Tag.Artists[0]);
        eArt.Text = tfile.Tag.Artists[0];
        eTitle.Text = tfile.Tag.Title;
        if (lyrics == string.Empty)
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                eLyrics.TextColor = Colors.IndianRed;
            else
                eLyrics.TextColor= Colors.DarkRed;
            eLyrics.Text = "Lyrics Not Available :(";
        }
        else
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                eLyrics.TextColor = Colors.LightGreen;
            else
                eLyrics.TextColor = Colors.Green;
            eLyrics.Text = lyrics;
        }
    }
}