namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category,
    string Description, decimal Price, string ImageFile) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image File is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
    }
}



public class CreateProductCommandHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        // Business logic implementation
        var product = new Product
        {
            Category = command.Category,
            Description = command.Description,
            Price = command.Price,
            ImageFile = command.ImageFile,
            Name = command.Name
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
