using SellWeb.Business.Models;
using System;
using System.Threading.Tasks;

namespace SellWeb.Business.Interfaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
