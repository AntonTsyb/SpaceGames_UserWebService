using SpaceGame.UserService.API.Models;
using SpaceGame.UserService.API.Services;
using SpaceGames.UserService.Api.Services;

namespace SpaceGames.UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : Controller
    {
        IUserService _userService;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _userPool;


        public UsersController(IUserService userService,
            UserManager<CognitoUser> userManager,
            CognitoUserPool userPool)
        {
            _userService = userService;
            _userManager = (CognitoUserManager<CognitoUser>)userManager;
            _userPool = userPool;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.Get(activeOnly: true);
            return Ok(users);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUser([FromRoute] string email)
        {
            var user = await _userService.GetByEmail(email);
            return Ok(user);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> EditUser([FromRoute] string email, [FromBody] UserProfileRequestModel model)
        {
            CognitoIdentityHelper.TryGetEmailFromClaim(base.User, out string emailFromToken);
            if (email != emailFromToken)
            {
                return Forbid();
            }

            await _userService.UpdateUser(email, model);

            return Ok();
        }

        [HttpPost("{email}/avatar")]
        public async Task<IActionResult> UploadAvatar([FromRoute] string email, UserAvatarRequestModel file)
        {
            CognitoIdentityHelper.TryGetEmailFromClaim(base.User, out string emailFromToken);
            if (email != emailFromToken)
            {
                return Forbid();
            }

            await _userService.UpdateUser(email, file);

            return Ok();
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string email)
        {
            CognitoIdentityHelper.TryGetEmailFromClaim(base.User, out string emailFromToken);
            if (email != emailFromToken)
            {
                return Forbid();
            }
            await _userService.DeleteUser(email);

            var cognitoUser = _userPool.GetUser(email);
            if (cognitoUser != null)
                await _userManager.DeleteAsync(cognitoUser);

            return Ok();
        }
    }
}