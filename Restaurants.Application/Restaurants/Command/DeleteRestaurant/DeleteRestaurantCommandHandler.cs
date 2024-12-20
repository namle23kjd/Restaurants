﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Command.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommand> logger, IRestaurantsRepository restaurantsRepository , IMapper mapper) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id : {Restaurant}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync( request.Id );
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());


        await restaurantsRepository.Delete(restaurant);   
    }
}