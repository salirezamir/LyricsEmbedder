using System.IO;
using System.Threading;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace LyricsEmbdM.Pages;

public partial class Embedder : ContentPage
{

    public Embedder()
    {
        InitializeComponent();
        tCommandL.Text = "Ready to Command :)\n";
    }

    private async void bExecute_Clicked(object sender, EventArgs e)
    {
        FilePickerFileType customFileType = new FilePickerFileType(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
                        { DevicePlatform.iOS, new[] { "public.audio" } }, // UTType values
                        { DevicePlatform.Android, new[] { "audio/*" } }, // MIME type
                        { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".wma", ".m4a", ".flac" } }, // file extension
                        { DevicePlatform.Tizen, new[] { "audio/*" } },
                        { DevicePlatform.macOS, new[] { ".mp3", ".wav", ".wma", ".m4a", ".flac" } }, // UTType values
        });
        PickOptions options = new()
        {
            PickerTitle = "Please select audio file's",
            FileTypes = customFileType
        };

        var fileResult = await FilePicker.PickMultipleAsync(options);
        foreach (FileResult file in fileResult)
        {
            var tfile = TagLib.File.Create(file.FullPath);
            var mode = new TagLib.File.AccessMode();
            mode = TagLib.File.AccessMode.Write;
            try
            {
                Genius genius = new Genius();
                string lyrics = await genius.GetLyricsAsync(tfile.Tag.Title + " " + tfile.Tag.Performers[0]);
                if (tfile.Tag.Lyrics == null || sSkip.IsToggled)
                {
                    if (lyrics != string.Empty)
                    {
                        tfile.Tag.Lyrics = lyrics;
                        tCommandL.Text += tfile.Tag.Title + " ✔️\n";
                        tfile.Save();
                    }
                    else
                        tCommandL.Text += tfile.Tag.Title + " ❌\n";
                }
                else if (tfile.Tag.Lyrics.ToString().Length < 100)
                {
                    if (lyrics != string.Empty)
                    {
                        tfile.Tag.Lyrics = lyrics;
                        tCommandL.Text += tfile.Tag.Title + " ✔️\n";
                    }
                    else
                        tCommandL.Text += tfile.Tag.Title + " ❌\n";
                }
                else
                    tCommandL.Text += tfile.Tag.Title + " ⚠️\n";
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error",ex.Message , "OK");
            }
        }
    }

    private void bClear_Clicked(object sender, EventArgs e)
    {
        tCommandL.Text = "Log Cleared :) \n";
    }

    private void sSkip_Toggled(object sender, ToggledEventArgs e)
    {
        if (!sSkip.IsToggled)
            tHint.Text = "✔️:Lyrics Embedded ❌:Lyrics Not Found ⚠️:Lyrics Existed";
        else
            tHint.Text = "✔️:Lyrics Embedded ❌:Lyrics Not Found";
    }
}