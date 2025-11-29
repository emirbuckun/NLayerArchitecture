using FluentValidation;

namespace App.Application.Features.Products.Create {
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest> {
        public CreateProductRequestValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(0, 20).WithMessage("Product name must be less than 20 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Product category ID must be greater than zero.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(0, 100).WithMessage("Product stock must be between 0 and 100.");
        }
    }
}