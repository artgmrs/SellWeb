using FluentValidation;

namespace SellWeb.Business.Models.Validator
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 200).WithMessage("O campo {PropertyName} precisa ser ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 500).WithMessage("O campo {PropertyName} precisa ser ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(p => p.Valor)
                .GreaterThan(0).WithMessage("O campo {PropertyName} precisa ser maior que {ComparisonValue}");

        }
    }
}

