using Models;

namespace DAL
{
    public class ProdutoDAL : DALModelo<Produto>
    {
        public ProdutoDAL()
        {
            idExceptionSalvar[Operacao.Inserir] = -1;
            idExceptionSalvar[Operacao.Alterar] = -2;
            idExceptionSalvar[Operacao.Excluir] = -3;
        }
    }
}