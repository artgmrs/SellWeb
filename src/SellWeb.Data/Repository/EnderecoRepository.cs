using Microsoft.EntityFrameworkCore;
using SellWeb.Business.Interfaces;
using SellWeb.Business.Models;
using SellWeb.Data.Context;
using System;
using System.Threading.Tasks;

namespace SellWeb.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(SellWebDbContext context) : base(context)
        {
        }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(e => e.FornecedorId == fornecedorId);
        }
    }
}
