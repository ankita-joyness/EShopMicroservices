using Catalog.API.Exceptions;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Product Product) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product Id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("Image File is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");
    }
}


public class UpdateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken) ?? throw new ProductNotFoundException(command.Product.Id);

        product.Name = command.Product.Name;
        product.Description = command.Product.Description;
        product.Category = command.Product.Category;
        product.Price = command.Product.Price;
        product.ImageFile = command.Product.ImageFile;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
