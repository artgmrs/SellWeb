using SellWeb.Business.Interfaces;
using SellWeb.Business.Models;
using SellWeb.Business.Models.Validator;
using System;
using System.Threading.Tasks;

namespace SellWeb.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {

        private readonly IProdutoRepository _produtoRepo;

        public ProdutoService(IProdutoRepository produtoRepo,
                              INotificador notificador) : base(notificador)
        {
            _produtoRepo = produtoRepo; ;
        }

        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidator(), produto)) return;

            await _produtoRepo.Adicionar(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidator(), produto)) return;

            await _produtoRepo.Atualizar(produto);
        }

        public async Task Remover(Guid id)
        {
            await _produtoRepo.Remover(id);
        }

        public void Dispose()
        {
            _produtoRepo?.Dispose();
        }
    }
}
