using System.IO;

namespace Infra
{
    public class Arquivo
    {
        public void GravarLinhaNoFinalDoArquivo(string _caminhoArquivo, string _texto)
        {
            using (StreamWriter arquivo = File.AppendText(_caminhoArquivo))
            {
                arquivo.WriteLine(_texto);
            }
        }
    }
}
