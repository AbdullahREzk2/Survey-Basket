namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userservice = userService;

    #region get user info
    [HttpGet("userInfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var result = await _userservice.GetProfileAsync(User.GetUserId()!);

        return result.IsSuccess? Ok(result.Value): result.ToProblem();   
    }
    #endregion


}
