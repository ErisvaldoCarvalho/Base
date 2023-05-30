using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public static class Log
    {
        public static void Gravar(string _texto)
        {
            string diretorio = Environment.CurrentDirectory + "\\Logs\\Log";
            Directory.CreateDirectory(diretorio);
            string caminhoArquivo = diretorio + DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Month.ToString("00") + DateTime.Now.Day.ToString("00") + ".log";
            new Arquivo().GravarLinhaNoFinalDoArquivo(caminhoArquivo, new Criptografia().Criptografar(DateTime.Now + ": " + _texto));
        }
    }
}
