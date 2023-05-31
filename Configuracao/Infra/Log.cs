using Models;
using System;

namespace Infra
{
    public static class Log
    {
        public static void Gravar(string _texto)
        {         
            new Arquivo().GravarLinhaNoFinalDoArquivo(Constantes.CaminhoArquivoLog, new Criptografia().Criptografar(DateTime.Now + ": " + _texto));
        }
    }
}
