using System.IO;
using System.Threading;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
//using Android.Content;
//using Android.Provider;
//using Android.App;
//using AndroidX.Core.Content;
//using Android;
using Microsoft.Maui.Controls.PlatformConfiguration;

//using Microsoft.Maui.Essentials;

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

        //sadasdasd();

        var fileResult = await FilePicker.PickMultipleAsync(options);
        foreach (FileResult file in fileResult)
        {
            var tfile = TagLib.File.Create(file.FullPath);
            var mode = new TagLib.File.AccessMode();
            mode = TagLib.File.AccessMode.Write;
            Genius genius = new Genius();
            string lyrics = await genius.GetLyricsAsync(tfile.Tag.Title, tfile.Tag.Artists[0]);
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
    //kjgfkjfktgfkgyubuilhopriutdjeropimctjugo8cpu5vt5om,dfphd4%%%%%GPT
    //private async void sadasdasd()
    //{
    //    PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

    //    if (status == PermissionStatus.Granted)
    //    {
    //        var projection = new List<string>()
    //            {
    //                Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Id,
    //                Android.Provider.MediaStore.Audio.Media.InterfaceConsts.DisplayName,
    //               Android.Provider.MediaStore.Audio.Media.InterfaceConsts.DateAdded,
    //              Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Title,
    //               Android.Provider.MediaStore.Audio.Media.InterfaceConsts.RelativePath,
    //              Android.Provider.MediaStore.Audio.Media.InterfaceConsts.MimeType,
    //              Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Data
    //            }.ToArray();

    //        string selection = Android.Provider.MediaStore.Audio.Media.InterfaceConsts.MimeType + " = ?";

    //        var selectionArgs = new List<string>()
    //        {
    //                "audio/mpeg"
    //        };

    //        ContentResolver contentResolver = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.ContentResolver;
    //        Android.Database.ICursor cursor = contentResolver.Query(Android.Provider.MediaStore.Audio.Media.ExternalContentUri, projection, selection, selectionArgs.ToArray(), null);
    //        if (cursor != null && cursor.Count > 0)
    //        {
    //            int idColumn = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Audio.Media.InterfaceConsts.id);
    //            int dispNameColumn = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Audio.Media.InterfaceConsts.DisplayName);
    //            cursor.MoveToFirst();
    //            do
    //            {
    //                //1000000034
    //                long id = cursor.GetLong(idColumn);
    //                string displayName = cursor.GetString(dispNameColumn);
    //                Android.Net.Uri uri1 = Android.Provider.MediaStore.Audio.Media.ExternalContentUri.BuildUpon().AppendPath(id.ToString()).Build();
    //                //string b = Android.Provider.MediaStore.GetDocumentUri(new test,uri1);
    //                //"content://media/external/audio/media/1000000037"
    //                string androidUri = uri1.ToString();
    //                long androidFileId = id;
    //                break;

    //            }
    //            while (cursor.MoveToNext());
    //            cursor.Close();
    //            cursor.Dispose();
    //        }
    //    }
    //    else
    //    {
    //        PermissionStatus RequestStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
    //    }

    //}

    //public void UpdatedisplayNameInformation(long audioId, string displayName)
    //{
    //    ContentResolver contentResolver = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.ContentResolver;
    //    // Create the content values object to hold the new display name information
    //    ContentValues values = new ContentValues();

    //    values.Put(Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Album, displayName);
    //    // Build the content URI for the specific audio file using its ID
    //    Android.Net.Uri contentUri = ContentUris.WithAppendedId(Android.Provider.MediaStore.Audio.Media.ExternalContentUri, audioId);
    //    contentResolver.Update(contentUri, values, null, null);

    //}
}