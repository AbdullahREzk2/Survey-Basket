namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userservice = userService;

    #region get All users 
    [HttpGet("get-all-users")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> getAllUsers(CancellationToken cancellationToken)
    {
        return Ok(await _userservice.GetAllUsersAsync(cancellationToken));       
    }
    #endregion

    #region get user Details 
    [HttpGet("get-user-Details/{userId}")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> getUserDetails(string userId)
    {
        var userDetails = await _userservice.GetUserDetailsAsync(userId);
        return userDetails.IsSuccess ?
              Ok(userDetails.Value)
            : userDetails.ToProblem(); 
    }
    #endregion

    #region create new User
    [HttpPost("create-user")]
    [HasPermission(Permissions.AddUsers)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request,CancellationToken cancellationToken)
    {
        var result = await _userservice.createUserAsync(request,cancellationToken);

        return result.IsSuccess?
            CreatedAtAction(nameof(getUserDetails), new {userId = result.Value.Id },result.Value)
            : result.ToProblem();
    }
    #endregion

    #region update User
    [HttpPut("update-user/{userId}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Update(string userId, [FromBody] UpdateUserRequest request,CancellationToken cancellationToken)
    {
        var result = await _userservice.updateUserAsync(userId,request,cancellationToken);
        return result.IsSuccess?
            NoContent()
          : result.ToProblem();
    }
    #endregion

    #region toggle user status
    [HttpPut("toggle-user-status/{userId}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> ToggleStatus(string userId)
    {
        var result = await _userservice.toggleStatusAsync(userId);
        return result.IsSuccess?
            NoContent()
          : result.ToProblem();
    }
    #endregion

    #region unlock user
    [HttpPut("unlock-user/{userId}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Unlock(string userId)
    {
        var result = await _userservice.unlockUserAsync(userId);
        return result.IsSuccess?
            NoContent()
          : result.ToProblem();
    }
    #endregion


}
