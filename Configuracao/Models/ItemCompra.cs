namespace Models
{
    public class ItemCompra
    {
        public int Id { get; set; }
        public Compra Compra { get; set; }
        public Produto Produto { get; set; }
        public float Quantidade { get; set; }
        public float PrecoUnitario { get; set; }
        public float ValorDesconto { get; set; }
        public float ValorAcrescimo { get; set; }
        public float ValorTotal { get; set; }
    }
}