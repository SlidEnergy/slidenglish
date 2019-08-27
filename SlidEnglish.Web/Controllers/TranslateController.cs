using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using System;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TranslateService _translateService;
        private readonly GoogleCredential _googleCredential;

        public TranslateController(IMapper mapper, TranslateService translateService, GoogleCredential googleCredential)
        {
            _mapper = mapper;
            _translateService = translateService;
            _googleCredential = googleCredential;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult<TranslateData>> Translate([FromBody]TranslateDataBindingModel data)
        {
            var userId = User.GetUserId();

            await _translateService.ProcessTranslate(userId, data.Text);

            try
            {
                TranslationClient client = TranslationClient.Create(_googleCredential);
                var response = await client.TranslateTextAsync(data.Text, "ru", "en");

                return Ok(new TranslateData { Text = response.TranslatedText });
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}