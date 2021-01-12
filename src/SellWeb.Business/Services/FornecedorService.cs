using SellWeb.Business.Interfaces;
using SellWeb.Business.Models;
using SellWeb.Business.Models.Validator;
using SellWeb.Business.Models.Validator.DocumentsValidator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SellWeb.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {

        private readonly IFornecedorRepository _fornecedorRepo;
        private readonly IEnderecoRepository _enderecoRepo;

        public FornecedorService(IFornecedorRepository fornecedorRepo,
                                 IEnderecoRepository enderecoRepo,
                                 INotificador notificador
                                 ) : base(notificador)
        {
            _fornecedorRepo = fornecedorRepo;
            _enderecoRepo = enderecoRepo; ;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidator(), fornecedor)
                || !ExecutarValidacao(new EnderecoValidator(), fornecedor.Endereco)) return;

            fornecedor.Documento = Utils.ApenasNumeros(fornecedor.Documento);

            if (_fornecedorRepo.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("já existe um fornecedor com o documento informado");
                return;
            }

            await _fornecedorRepo.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidator(), fornecedor)) return;

            if (_fornecedorRepo.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("já existe um fornecedor com o documento informado");
                return;
            }

            await _fornecedorRepo.Atualizar(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidator(), endereco)) return;

            await _enderecoRepo.Atualizar(endereco);
        }

        public async Task Remover(Guid id)
        {
            if (_fornecedorRepo.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("o fornecedor possui produtos cadastrados! não é possível realizar a exclusão");
                return;
            }

            var endereco = await _enderecoRepo.ObterEnderecoPorFornecedor(id);

            if (endereco != null)
            {
                await _enderecoRepo.Remover(endereco.Id);
            }

            await _fornecedorRepo.Remover(id);
        }

        public void Dispose()
        {
            _fornecedorRepo?.Dispose();
            _enderecoRepo?.Dispose();
        }
    }
}
