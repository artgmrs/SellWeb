using System;

namespace SellWeb.Business.Models
{
    public class Produto : Entity
    {
        public Guid FornecedorId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadasto { get; set; }
        public bool Ativo { get; set; }
        public Fornecedor Fornecedor { get; set; }

    }
}
