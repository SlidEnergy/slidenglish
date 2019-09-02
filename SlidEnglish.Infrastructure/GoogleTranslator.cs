using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlidEnglish.Infrastructure
{
    public class GoogleTranslator : ITranslator
    {
        private readonly GoogleCredential _googleCredential;

        public GoogleTranslator(GoogleCredential googleCredential)
        {
            _googleCredential = googleCredential;
        }

        public async Task<string> TranslateAsync(string text)
        {
            TranslationClient client = TranslationClient.Create(_googleCredential);
            var response = await client.TranslateTextAsync(text, "ru", "en");

            return response.TranslatedText;
        }
    }
}
