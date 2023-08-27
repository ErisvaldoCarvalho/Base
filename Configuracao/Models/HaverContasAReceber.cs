using System;

namespace Models
{
    public class HaverContasAReceber
    {
        public int Id { get; set; }
        public float Valor { get; set; }
        public ContasAReceber ContasAReceber { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}