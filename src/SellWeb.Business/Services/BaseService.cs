using FluentValidation;
using FluentValidation.Results;
using SellWeb.Business.Interfaces;
using SellWeb.Business.Models;
using SellWeb.Business.Notifications;

namespace SellWeb.Business.Services
{
    public class BaseService
    {
        private readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        public void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                Notificar(error.ErrorMessage);
        }

        public void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);
            return false;
        }
    }
}
