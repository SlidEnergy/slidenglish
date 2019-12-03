using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System;
using System.Threading.Tasks;

namespace SlidEnglish.Web
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        private readonly ITranslateService _translateService;

        public TranslateController(ITranslateService translateService)
        {
            _translateService = translateService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult<TranslateData>> Translate([FromBody]TranslateDataBindingModel data)
        {
            var userId = User.GetUserId();

            var translateData = await _translateService.ProcessTranslate(userId, data.Text);

            return Ok(translateData);
        }
    }
}