using DAL;
using Models;
using System;
using System.Security.Cryptography.X509Certificates;

namespace BLL
{
    public class VendaBLL
    {
        public Venda BuscarPorId(int _idVenda)
        {
            return new VendaDAL().BuscarPorId(_idVenda);
        }

        public void Salvar(Venda _venda)
        {
            switch (_venda.Registro)
            {
                case Registro.Alterado:
                    new VendaDAL().Alterar(_venda);
                    break;
                case Registro.Excluido:
                    new VendaDAL().Excluir(_venda);
                    break;
                default:
                    new VendaDAL().Inserir(_venda);
                    break;
            }
        }
    }
}
