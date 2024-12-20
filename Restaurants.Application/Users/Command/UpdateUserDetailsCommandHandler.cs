using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Command;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext, IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user : {UserId} , with {@Request}",user!.id , request);
        var dbUser = await userStore.FindByIdAsync(user!.id, cancellationToken);
        if (dbUser == null)
        {
            throw new NotFoundException(nameof(User), user!.id); 
        } 
          
        dbUser.Nationlity = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBrith;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
