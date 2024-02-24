using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace LyricsEmbdM
{
    internal class Genius
    {
        private static readonly string GeniusApiBaseUrl = "https://api.genius.com";
        private string GeniusApiKey = "API_Code";

        private readonly HttpClient httpClient;

        public Genius()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GeniusApiKey}");
        }

        public async Task<string> GetLyricsAsync(string songTitle, string artist)
        {
            try
            {
                string searchUrl = $"{GeniusApiBaseUrl}/search?q={Uri.EscapeDataString(songTitle)}%20{Uri.EscapeDataString(artist)}";
                HttpResponseMessage searchResponse = await httpClient.GetAsync(searchUrl);
                searchResponse.EnsureSuccessStatusCode();
                string searchResponseContent = await searchResponse.Content.ReadAsStringAsync();
                string songURL = GetSongURLFromJson(searchResponseContent);

                if (!string.IsNullOrEmpty(songURL))
                {
                    HttpResponseMessage lyricsResponse = await httpClient.GetAsync(songURL);
                    lyricsResponse.EnsureSuccessStatusCode();
                    string lyricsResponseContent = await lyricsResponse.Content.ReadAsStringAsync();
                    string lyrics = GetLyricsFromHtml(lyricsResponseContent);
                    return lyrics;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                //Console.WriteLine($"Error occurred: {ex.Message}");
                return string.Empty;
            }
        }

        private string GetSongURLFromJson(string json)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(json);
            JsonElement hitsElement = jsonDocument.RootElement.GetProperty("response").GetProperty("hits");

            if (hitsElement.GetArrayLength() < 1)
            {
                return string.Empty;
            }
            string? lyricsUrl = hitsElement
                .EnumerateArray()
                .First()
                .GetProperty("result")
                .GetProperty("url")
                .GetString();
            return lyricsUrl;
        }

        private string GetLyricsFromHtml(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            foreach (var brNode in htmlDocument.DocumentNode.Descendants("br").ToList())
            {
                brNode.ParentNode.ReplaceChild(HtmlNode.CreateNode("\n"), brNode);
            }
            HtmlNodeCollection divNodes = htmlDocument.DocumentNode.SelectNodes("//div[@data-lyrics-container='true']");
            string extractedText = string.Empty;
            if (divNodes != null)
            {
                foreach (HtmlNode divNode in divNodes)
                {

                    extractedText += divNode.InnerText + "\n";
                }
                extractedText = extractedText.Replace("<br>", "\n");
                extractedText = extractedText.Replace("&#x27;", "'");
                //&#x27;
                return extractedText;
            }
            return string.Empty;
        }
    }
}
