namespace Models
{
    public class ItemVenda
    {
        public int Id { get; set; }
        public Venda Venda { get; set; }
        public Produto Produto { get; set; }
        public float Quantidade { get; set; }
        public float PrecoUnitario { get; set; }
        public float ValorDesconto { get; set; }
        public float ValorAcrescimo { get; set; }
        public float ValorTotal { get; set; }
    }
}