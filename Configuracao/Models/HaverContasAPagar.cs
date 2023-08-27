using System;

namespace Models
{
    public class HaverContasAPagar
    {
        public int Id { get; set; }
        public float Valor { get; set; }
        public ContasAPagar ContasAPagar { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}