using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public List<string> LerLinhasArquivo(string _caminhoArquivo, bool _criptografado = false)
        {
            List<string> linhas = new List<string>();

            using (StreamReader sr = new StreamReader(_caminhoArquivo))
            {
                string linha;
                while ((linha = sr.ReadLine()) != null)
                {
                    if (_criptografado)
                        linhas.Add(new Criptografia().Descriptografar(linha));
                    else
                        linhas.Add(linha);
                }
            }

            return linhas;
        }
        public void ExcluirArquivo(string _arquivo)
        {
            if (File.Exists(_arquivo))
                File.Delete(_arquivo);
        }
        public void GravarBytesNoFinalDoArquivo(string _caminhoArquivo, byte[] _bytes)
        {
            ExcluirArquivo(_caminhoArquivo);

            using (FileStream arquivo = new FileStream(_caminhoArquivo, FileMode.Append))
            {
                if (!File.Exists(_caminhoArquivo))
                {
                    byte[] novaLinhaBytes = Encoding.UTF8.GetBytes(Environment.NewLine);
                    arquivo.Write(novaLinhaBytes, 0, novaLinhaBytes.Length);
                }

                arquivo.Write(_bytes, 0, _bytes.Length);
            }
        }

        public string[] ListarArquivos(string _diretorio, string _filtro = "")
        {
            try
            {              
                return Directory.Exists(_diretorio) ? Directory.GetFiles(_diretorio, _filtro) : new string[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar arquivos do diretório", ex) { Data = { { "Id", 5 } } };
            }
        }

    }
}
