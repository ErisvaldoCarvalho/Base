using Models;
using System;
using System.IO;
using System.Runtime.Remoting.Channels;

namespace DAL
{
    public static class Conexao
    {
        public static string StringDeConexao
        {
            get
            {
                if (String.IsNullOrEmpty(Constantes.StringDeConexao))
                {
                    if (File.Exists(Constantes.DiretorioStringConexao + Constantes.NomeArquivoConexao))
                        Constantes.StringDeConexao = File.ReadAllText(Constantes.DiretorioStringConexao + Constantes.NomeArquivoConexao);
                    else
                        throw new Exception("É preciso criar a string de conexão") { Data = { { "Id", 1 } } };
                }
                return Constantes.StringDeConexao;
            }
        }
    }
}
