using System;
using System.Collections.Generic;

namespace Models
{
    public class ContasAPagar
    {
        public int Id { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public Compra Compra { get; set; }
        public float ValorDoTitulo { get; set; }
        public float ValorPago { get; set; }
        public float ValorAPagar { get; set; }
        public bool Quitado { get; set; }
        public List<HaverContasAPagar> HaverContasAPagarList { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
