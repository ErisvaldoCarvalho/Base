using DAL;
using Models;
using System.Collections.Generic;

namespace BLL
{
    public class ProdutoBLL
    {
        private void ValidarDados(Produto _produto)
        {

        }
        public void Salvar(Produto _produto)
        {
            ValidarDados(_produto);
            new ProdutoDAL().Salvar(_produto);
        }
        public void Excluir(int _id)
        {
            new ProdutoDAL().Excluir(_id);
        }
        public List<Produto> BuscarTodos()
        {
            return new ProdutoDAL().BuscarTodos();
        }
        public Produto BuscarPorId(int _id)
        {
            return new ProdutoDAL().BuscarPorId(_id);
        }
    }
}