using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricsEmbdM
{
    public interface IMusicFilePicker
    {
        Task<Stream> PickMusicFileAsync();
    }
}
