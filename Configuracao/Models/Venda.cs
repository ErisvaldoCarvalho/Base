using System;
using System.Collections.Generic;

namespace Models
{
    public enum Registro
    {
        Novo,
        Alterado,
        Excluido,
        Carregado
    }
    public class Venda
    {
        public Registro Registro { get; set; }
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
        public Venda()
        {
            this.Registro = new Registro();
            this.Registro = Registro.Novo;
            this.Cliente = new Cliente();
            this.ItemVendaList = new List<ItemVenda>();
        }
    }
}