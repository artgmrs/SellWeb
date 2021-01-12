using SellWeb.Business.Notifications;
using System.Collections.Generic;

namespace SellWeb.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacao();
        void Handle(Notificacao notificacao);
    }
}
