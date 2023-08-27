using System;
using System.Collections.Generic;

namespace Models
{
    public class Venda
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Data { get; set; }
        public float ValorTotalDosProdutos { get; set; }
        public float ValorDesconto { get; set; }
        public float ValorAcrescimo { get; set; }
        public float ValorTotal { get; set; }
        public List<ItemVenda> ItemVendaList { get; set; }
        public bool Faturada { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
