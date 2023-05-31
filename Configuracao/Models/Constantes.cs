using System;
using System.IO;

namespace Models
{
    public static class Constantes
    {
        public static int IdUsuarioLogado;
        public static string StringDeConexao;
        public static string DiretorioStringConexao = "C:\\Configuracao\\";
        public static string CaminhoChavePublica = "C:\\Configuracao\\ChavePublica.txt";
        public static string CaminhoChavePrivada = "C:\\Configuracao\\ChavePrivada.txt";
        public static string NomeArquivoConexao = "configuracaoConnection.config";

        public static string CaminhoArquivoLog
        {
            get
            {
                string diretorio = Environment.CurrentDirectory + "\\Logs\\";
                Directory.CreateDirectory(diretorio);
                return diretorio + "Log" + DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Month.ToString("00") + DateTime.Now.Day.ToString("00") + ".log";
            }
        }
    }
}
