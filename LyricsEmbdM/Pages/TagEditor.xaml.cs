using Microsoft.Maui.Storage;

namespace LyricsEmbdM.Pages;

public partial class TagEditor : ContentPage
{
    public TagEditor()
    {
        InitializeComponent();
        //clear(false);
    }

    private async void bFile_Clicked(object sender, EventArgs e)
    {
        clear(true);
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
            PickerTitle = "Please select audio file",
            FileTypes = customFileType
        };
        var fileResult = await FilePicker.Default.PickAsync(options);
        var tfile = TagLib.File.Create(fileResult.FullPath);
        Preferences.Default.Set("Tag_Editor_File_Address", fileResult.FullPath);
        eTittle.Text = tfile.Tag.Title;
        eAlbum.Text = tfile.Tag.Album;
        foreach (string Artist in tfile.Tag.Artists)
            eArtist.Text += Artist + "/";
        if (eArtist.Text != string.Empty)
            eArtist.Text = eArtist.Text.Remove(eArtist.Text.Length - 1, 1);
        foreach (string AlbArtist in tfile.Tag.AlbumArtists)
            eAlbumArtist.Text = AlbArtist + "/";
        if (eAlbumArtist.Text != string.Empty)
            eAlbumArtist.Text = eAlbumArtist.Text.Remove(eAlbumArtist.Text.Length - 1, 1);
        foreach (string Composer in tfile.Tag.Composers)
            eComposser.Text = Composer + "/";
        if (eComposser.Text != string.Empty)
            eComposser.Text = eComposser.Text.Remove(eComposser.Text.Length - 1, 1);
        foreach (string Genre in tfile.Tag.Genres)
            eGenre.Text = Genre + "/";
        if (eGenre.Text != string.Empty)
            eGenre.Text = eGenre.Text.Remove(eGenre.Text.Length - 1, 1);
        eTrackNum.Text = tfile.Tag.Track.ToString();
        eDiscNum.Text = tfile.Tag.Disc.ToString();
        eYear.Text = tfile.Tag.Year.ToString();
        eComment.Text = tfile.Tag.Comment;
        ibAlbArt.Source = ImageSource.FromStream(() => new MemoryStream(tfile.Tag.Pictures[0].Data.ToArray()));
    }

    private void bSave_Clicked(object sender, EventArgs e)
    {
        string filePath = Preferences.Default.Get("Tag_Editor_File_Address", "");
        var tfile = TagLib.File.Create(filePath);
        var mode = new TagLib.File.AccessMode();
        mode = TagLib.File.AccessMode.Write;
        tfile.Tag.Title = eTittle.Text;
        tfile.Tag.Album = eAlbum.Text;
        if (eArtist.Text.Contains("/"))
        {
            foreach (var (value, i) in eArtist.Text.Split("/").Select((value, i) => (value, i)))
                tfile.Tag.Performers[i] = value;
        }
        else
            tfile.Tag.Performers[0] = eArtist.Text;
        if (eAlbumArtist.Text.Contains("/"))
        {
            foreach (var (value, i) in eAlbumArtist.Text.Split("/").Select((value, i) => (value, i)))
                tfile.Tag.Performers[i] = value;
        }
        else
            tfile.Tag.Performers[0] = eAlbumArtist.Text;
        if (eComposser.Text.Contains("/"))
        {
            foreach (var (value, i) in eComposser.Text.Split("/").Select((value, i) => (value, i)))
                tfile.Tag.Performers[i] = value;
        }
        else
            tfile.Tag.Performers[0] = eComposser.Text;
        if (eComposser.Text.Contains("/"))
        {
            foreach (var (value, i) in eComposser.Text.Split("/").Select((value, i) => (value, i)))
                tfile.Tag.Performers[i] = value;
        }
        else
            tfile.Tag.Performers[0] = eComposser.Text;
        tfile.Tag.Track = uint.Parse(eTrackNum.Text);
        tfile.Tag.Disc = uint.Parse(eDiscNum.Text);
        tfile.Tag.Year = uint.Parse(eYear.Text);
        tfile.Tag.Comment = eComment.Text;
        //tfile.Tag.Pictures[0].Data = 
        tfile.Save();
        clear(false);
    }

    private void bReset_Clicked(object sender, EventArgs e)
    {
        clear(false);
    }

    private void ibAlbArt_Clicked(object sender, EventArgs e)
    {

    }

    void clear(bool save)
    {
        eTittle.Text = string.Empty;
        eAlbum.Text = string.Empty;
        eArtist.Text = string.Empty;
        eAlbumArtist.Text = string.Empty;
        eComposser.Text = string.Empty;
        eGenre.Text = string.Empty;
        eTrackNum.Text = string.Empty;
        eDiscNum.Text = string.Empty;
        eYear.Text = string.Empty;
        eComment.Text = string.Empty;
        ibAlbArt.Source = string.Empty;
        Preferences.Default.Set("Tag_Editor_File_Address", string.Empty);
        bSave.IsEnabled = save;
    }
}