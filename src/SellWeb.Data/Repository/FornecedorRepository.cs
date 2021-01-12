using Microsoft.EntityFrameworkCore;
using SellWeb.Business.Interfaces;
using SellWeb.Business.Models;
using SellWeb.Data.Context;
using System;
using System.Threading.Tasks;

namespace SellWeb.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(SellWebDbContext context) : base(context)
        {
        }
        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Produtos)
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
