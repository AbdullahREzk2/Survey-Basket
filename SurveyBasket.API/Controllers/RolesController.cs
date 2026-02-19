namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleservice = roleService;


    #region get all roles
    [HttpGet("getAllRoles")]
    [HasPermission(Permissions.GetRoles)] 
    public async Task<IActionResult> getAllRoles([FromQuery] bool includeDisabled,CancellationToken cancellationToken)
    {
        var roles = await _roleservice.getAllRoles(includeDisabled,cancellationToken);
        return Ok(roles);
    }
    #endregion

    #region get role details
    [HttpGet("getRoleDetails/{RoleId}")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> getRoleDetails(string RoleId)
    {
        var roleDetails  = await _roleservice.getRoleDetails(RoleId);

        return roleDetails.IsSuccess ? 
             Ok(roleDetails.Value) 
            : roleDetails.ToProblem();
    }
    #endregion

    #region create role
    [HttpPost("createRole")]
    [HasPermission(Permissions.AddRoles)]
    public async Task<IActionResult> createRole(RoleRequest request,CancellationToken cancellationToken)
    {
        var createdRole = await _roleservice.CreateRole(request);
        return createdRole.IsSuccess ? 
            CreatedAtAction(nameof(getRoleDetails),new { createdRole.Value.Id},createdRole.Value)
            : createdRole.ToProblem();
    }
    #endregion

    #region update role
    [HttpPut("updateRole/{RoleId}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> updateRole(string RoleId, RoleRequest request)
    {
        var updatedRole = await _roleservice.UpdateRole(RoleId, request);
        return updatedRole.IsSuccess ? 
            NoContent() 
          : updatedRole.ToProblem();
    }
    #endregion

    #region toggle status
    [HttpPut("ToggleStatus/{roleId}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> updateToggleStatus(string roleId)
    {
        var updateResult = await _roleservice.RoleToggleStatus(roleId);

        return updateResult.IsSuccess?
             NoContent()
            :updateResult.ToProblem();
    }
    #endregion
}
