using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SellWeb.Business.Interfaces;
using System.Threading.Tasks;

namespace SellWeb.App.Extensions
{
    public class NotificacoesViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;
        private readonly INotyfService _notyf;

        public NotificacoesViewComponent(INotificador notificador,
                                         INotyfService notyf)
        {
            _notificador = notificador;
            _notyf = notyf;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacao());

            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));

            notificacoes.ForEach(c => _notyf.Error(c.Mensagem));

            return View();
        }
    }
}
