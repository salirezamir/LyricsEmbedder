// AndroidMusicFilePicker.cs (in your Android platform project)
using Android.Content;
using Android.Net;
using Android.OS;
using AndroidApp = Android.App.Application;
using AndroidX.Core.Content;
using Android.App;
using Android.Webkit;



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace LyricsEmbdM.Platforms.Android
{
    //class AndroidMusicFilePicker
    //{

    //}
    [Activity(Exported = true)]
    [IntentFilter(
    new[] { Intent.ActionGetContent },
    Categories = new[] { Intent.CategoryOpenable },
    DataMimeType = "audio/*")]
    public class MusicFilePickerActivity : Activity
    {
        private static TaskCompletionSource<Stream> _pickFileTaskCompletionSource;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Intent?.Action == Intent.ActionGetContent)
            {
                var intent = new Intent(Intent.ActionGetContent);
                intent.SetType("audio/*");
                intent.AddCategory(Intent.CategoryOpenable);
                StartActivityForResult(Intent.CreateChooser(intent, "Select music file"), 1001);
            }
            else if (Intent?.Action == Intent.ActionView)
            {
                var uri = Intent.Data;
                if (uri != null)
                {
                    _pickFileTaskCompletionSource?.TrySetResult(ContentResolver.OpenInputStream(uri));
                }
                else
                {
                    _pickFileTaskCompletionSource?.TrySetResult(null);
                }
                Finish();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1001)
            {
                if (resultCode == Result.Ok && data?.Data != null)
                {
                    var uri = data.Data;
                    var stream = ContentResolver.OpenInputStream(uri);
                    _pickFileTaskCompletionSource?.TrySetResult(stream);
                }
                else
                {
                    _pickFileTaskCompletionSource?.TrySetResult(null);
                }
                Finish();
            }
        }

        public static Task<Stream> PickFileAsync()
        {
            _pickFileTaskCompletionSource = new TaskCompletionSource<Stream>();

            var intent = new Intent(AndroidApp.Context, typeof(MusicFilePickerActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            AndroidApp.Context.StartActivity(intent);

            return _pickFileTaskCompletionSource.Task;
        }
    }

    public class AndroidMusicFilePicker : IMusicFilePicker
    {
        public async Task<Stream> PickMusicFileAsync()
        {
            try
            {
                var stream = await MusicFilePickerActivity.PickFileAsync();
                return stream ?? throw new Exception("No file selected");
            }
            catch (Exception ex)
            {
                throw new Exception("Error picking music file", ex);
            }
        }
    }
}
