using System.Collections.Generic;
using System;

namespace Models
{
    public class Compra
    {
        public int Id { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public DateTime Data { get; set; }
        public float ValorTotalDosProdutos { get; set; }
        public float ValorDesconto { get; set; }
        public float ValorAcrescimo { get; set; }
        public float ValorTotal { get; set; }
        public List<ItemCompra> ItemCompraList { get; set; }
        public bool Faturada { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}