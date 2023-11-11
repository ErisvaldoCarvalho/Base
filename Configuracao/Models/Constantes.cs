using System;
using System.Collections.Generic;

namespace Models
{
    public static class Constantes
    {
        public static int IdUsuarioLogado;
        public static string StringDeConexao;
        public static string DiretorioStringConexao = "C:\\Configuracao\\";
        public static string NomeArquivoConexao = "configuracaoConnection.config";
        private static Dictionary<string, string> dicionario;

        public static void PopularDicionario()
        {
            dicionario = new Dictionary<string, string>();
            dicionario.Add("Codigo", "Código");
            dicionario.Add("Descricao", "Descrição");
            dicionario.Add("Usuario", "Usuário");
        }
        public static string Verbose(string _texto)
        {
            string retorno = dicionario[_texto];
            if (!string.IsNullOrEmpty(retorno))
                return retorno;

            //TODO: Gravar um log informando de um nome não encontrado
            return _texto;
        }
    }
}
