﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Command.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext, IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user : {UserId} , with {@Request}", user!.Id, request);
        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);
        if (dbUser == null)
        {
            throw new NotFoundException(nameof(User), user!.Id);
        }

        dbUser.Nationlity = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBrith;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}