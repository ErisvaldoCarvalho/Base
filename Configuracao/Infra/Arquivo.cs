using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public class Arquivo
    {
        public void GravarLinhaNoFinalDoArquivo(string _arquivo, string _texto)
        {
            try
            {
                using (StreamWriter arquivo = File.AppendText(_arquivo))
                {
                    arquivo.WriteLine(_texto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar gravar linha no final do arquivo", ex) { Data = { { "Id", 3 } } };
            }
        }
        public string LerArquivo(string _arquivo)
        {
            try
            {
                if (File.Exists(_arquivo))
                    return File.ReadAllText(_arquivo);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar ler informações do arquivo", ex) { Data = { { "Id", 3 } } };
            }
        }
        public void ExcluirArquivo(string _arquivo)
        {
            if (File.Exists(_arquivo))
                File.Delete(_arquivo);
        }
    }
}
