﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Command.AssignUserRole;
using Restaurants.Application.Users.Command.UnAssignUserRole;
using Restaurants.Application.Users.Command.UpdateUserDetails;
using Restaurants.Domain.Contants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetail(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    //[HttpDelete("userRole")]
    //[Authorize(Roles = UserRoles.Admin)]
    //public async Task<IActionResult> RemoveUser (RemoveUserCommand command)
    //{
    //    await mediator.Send(command);
    //    return NoContent();
    //}

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UnAssignUserRole(UnAssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}