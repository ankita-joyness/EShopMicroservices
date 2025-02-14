﻿using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;

using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator
    : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.StoreBasket(command.Cart, cancellationToken);
        
        return new StoreBasketResult(result.UserName);
    }
}
