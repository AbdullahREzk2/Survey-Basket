using SurveyBasket.BLL.Features.Roles.Command.CreateRole;
using SurveyBasket.BLL.Features.Roles.Command.RoleToggleStatus;
using SurveyBasket.BLL.Features.Roles.Command.UpdateRole;
using SurveyBasket.BLL.Features.Roles.Query.GetAllRoles;
using SurveyBasket.BLL.Features.Roles.Query.GetRoleDetails;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    #region get all roles
    [HttpGet("getAllRoles")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> getAllRoles([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
    {
        var roles = await _mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }
    #endregion

    #region get role details
    [HttpGet("getRoleDetails/{Id}")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> getRoleDetails(string Id)
    {
        var roleDetails = await _mediator.Send(new GetRoleDetailsQuery(Id));

        return roleDetails.IsSuccess ?
             Ok(roleDetails.Value)
            : roleDetails.ToProblem();
    }
    #endregion

    #region create role
    [HttpPost("createRole")]
    [HasPermission(Permissions.AddRoles)]
    public async Task<IActionResult> createRole(RoleRequest request, CancellationToken cancellationToken)
    {
        var createdRole = await _mediator.Send(new CreateRoleCommand(request));
        return createdRole.IsSuccess ?
            CreatedAtAction(nameof(getRoleDetails), new { createdRole.Value.Id }, createdRole.Value)
            : createdRole.ToProblem();
    }
    #endregion

    #region update role
    [HttpPut("updateRole/{RoleId}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> updateRole(string RoleId, RoleRequest request)
    {
        var updatedRole = await _mediator.Send(new UpdateRoleCommand(RoleId, request));
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
        var updateResult = await _mediator.Send(new RoleToggleStatusCommand(roleId));

        return updateResult.IsSuccess ?
             NoContent()
            : updateResult.ToProblem();
    }
    #endregion
}
