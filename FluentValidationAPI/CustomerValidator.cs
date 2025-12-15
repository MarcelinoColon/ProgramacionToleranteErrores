using FluentValidation;

namespace FluentValidationAPI
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Name)
                .NotEmpty().WithMessage("Nombre es obligatorio")
                .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres");

            RuleFor(customer => customer.Email)
                .NotEmpty().WithMessage("Correo electronico es obligatorio")
                .EmailAddress().WithMessage("Escribir correo electronico valido");

            RuleFor(customer => customer.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Saldo no puede ser negativo");
        }
    }
}
