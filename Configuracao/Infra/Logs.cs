using System;

namespace Infra
{
    public static class Logs
    {
        public static void Gravar(string _texto, Exception _ex = null)
        {
            if (string.IsNullOrEmpty(_texto) && _ex != null)
                GravarNoArquivo(_ex.Message);
            else
                GravarNoArquivo(_texto);

            if (_ex != null && _ex.InnerException != null)
                GravarException(_ex.InnerException);
        }
        private static void GravarException(Exception _ex)
        {
            GravarNoArquivo(_ex.Message, false);

            if (_ex.InnerException != null)
                GravarException(_ex.InnerException);
        }

        private static void GravarNoArquivo(string _texto, bool _adicionarData = true)
        {
            string caminhoArquivo = @"C:\Configuracao\Logs\Log" +
                                    DateTime.Now.Year.ToString() +
                                    DateTime.Now.Month.ToString("00") +
                                    DateTime.Now.Day.ToString("00") +
                                    ".log";

            _texto = _adicionarData ? $"{DateTime.Now.ToString()}: {_texto}" : $"\n\t\t\t{_texto}";

            new Arquivo().GravarLinhaNoFinalDoArquivo(caminhoArquivo, _texto);
        }
    }
}
