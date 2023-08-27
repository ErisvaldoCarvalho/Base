using System;
using System.Collections.Generic;

namespace Models
{
    public class ContasAReceber
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public Venda Venda { get; set; }
        public float ValorDoTitulo { get; set; }
        public float ValorRecebido { get; set; }
        public float ValorAReceber { get; set; }
        public bool Quitado { get; set; }
        public List<HaverContasAReceber> HaverContasAReceberList { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
