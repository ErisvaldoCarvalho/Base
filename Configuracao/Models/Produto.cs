namespace Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string CodigoDeBarra { get; set; }
        public float Preco { get; set; }
        public float Estoque { get; set; }
        public SubGrupo SubGrupo { get; set; }
        public Categoria Categoria { get; set; }
    }
}