using System;

namespace Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CPFCNPJ { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Fone { get; set; }
    }
}