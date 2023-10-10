using System;

namespace Infra
{
    public static class Logs
    {
        public static void Gravar(string _texto)
        {
            string caminhoArquivo = @"C:\Configuracao\Logs\Log" +
                                    DateTime.Now.Year.ToString() +
                                    DateTime.Now.Month.ToString("00") +
                                    DateTime.Now.Day.ToString("00") + 
                                    ".log";

            _texto = DateTime.Now.ToString() + ": " + _texto;

            new Arquivo().GravarLinhaNoFinalDoArquivo(caminhoArquivo, _texto);
        }
    }
}
