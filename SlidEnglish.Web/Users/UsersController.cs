using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.Domain;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace SlidEnglish.Web
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
		private readonly ITokenService _tokenService;

		public UsersController(IMapper mapper, IUsersService usersService, ITokenService tokenService)
        {
            _mapper = mapper;
            _usersService = usersService;
			_tokenService = tokenService;
        }

        [HttpGet("current")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
		[Authorize]
        public async Task<ActionResult<Dto.User>> GetCurrentUser()
        {
            var userId = User.GetUserId();

            var user = await _usersService.GetById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<Dto.User>(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Dto.User>> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(model);

            var result = await _usersService.Register(user, model.Password);

            if (!result.Succeeded) {
                foreach (var e in result.Errors)
                {
                    ModelState.TryAddModelError(e.Code, e.Description);
                }

                return BadRequest(ModelState);
            }

            return Created("", _mapper.Map<Dto.User>(user));
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Dto.TokenInfo>> Login(LoginBindingModel userData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var tokens = await _tokenService.Login(userData.Email, userData.Password);

                return new Dto.TokenInfo() { Token = tokens.Token, RefreshToken = tokens.RefreshToken, Email = userData.Email };
            }
            catch(AuthenticationException)
            {
                return BadRequest();
            }
        }
    }
}
