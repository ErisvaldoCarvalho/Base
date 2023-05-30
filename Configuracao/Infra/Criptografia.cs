using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace Infra
{

    public class Criptografia
    {
        public List<string> GerarChave()
        {
            List<string> bytes = new List<string>();

            using (var aesAlg = Aes.Create())
            {
                // Gerar uma chave aleatória
                aesAlg.GenerateKey();
                byte[] key = aesAlg.Key;
                bytes.Add(Convert.ToBase64String(key));
                // Gerar um IV aleatório
                aesAlg.GenerateIV();
                byte[] iv = aesAlg.IV;
                bytes.Add(Convert.ToBase64String(iv));
            }

            Key = Convert.FromBase64String(bytes[0]);
            IV = Convert.FromBase64String(bytes[1]);
            return bytes;
        }

        private byte[] Key = Encoding.UTF8.GetBytes("chave de criptografia"); // Chave de criptografia (deve ter 16, 24 ou 32 bytes)
        private byte[] IV = Encoding.UTF8.GetBytes("vetor de inicialização"); // Vetor de inicialização (deve ter 16 bytes)

        public string Criptografar(string _texto)
        {
            string texto = _texto;
            
            texto += "4mH0jI6";
            texto = Cifrar(_texto);
            texto = ReverterTexto(texto);
            texto += "4mH0jI6";
            texto = Cifrar(_texto);
            texto = ReverterTexto(texto);
            texto += "4mH0jI6";
            texto = Cifrar(_texto);

            return texto;
        }
        private string Cifrar(string _texto)
        {
            byte[] key = Convert.FromBase64String("Shabc4mH0jI6Vho6C1NnvNZypZih9azH7s0FlyuqBAE=");
            byte[] iv = Convert.FromBase64String("Oko6z3dxYm/TTz9WZ+eNvg==");

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(_texto);
                        csEncrypt.Write(bytes, 0, bytes.Length);
                    }

                    var encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public string Descriptografar(string _texto)
        {
            string texto = _texto;
            
            texto = Descifrar(_texto);
            
            if (!texto.Contains("4mH0jI6"))
                return texto;
            texto = texto.Replace("4mH0jI6", "");

            texto = ReverterTexto(texto);
            texto = Descifrar(_texto);

            if (!texto.Contains("4mH0jI6"))
                return texto;
            texto = texto.Replace("4mH0jI6", "");

            texto = ReverterTexto(texto);
            texto = Descifrar(_texto);

            if (!texto.Contains("4mH0jI6"))
                return texto;
            texto = texto.Replace("4mH0jI6", "");



            return texto;
        }
        private string Descifrar(string _texto)
        {
            byte[] encryptedBytes = Convert.FromBase64String(_texto);

            byte[] key = Convert.FromBase64String("Shabc4mH0jI6Vho6C1NnvNZypZih9azH7s0FlyuqBAE=");
            byte[] iv = Convert.FromBase64String("Oko6z3dxYm/TTz9WZ+eNvg==");

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new System.IO.MemoryStream(encryptedBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }



        public string CriptografarSenha(string _senha)
        {
            string retorno = _senha;

            for (int i = 0; i < _senha.Length; i++)
            {
                retorno = GerarHash(retorno);
                retorno = ReverterTexto(retorno);
                retorno = GerarHash(retorno);
                retorno = ReverterTexto(retorno);
            }

            return GerarHash(retorno);
        }
        
        private string GerarHash(string _texto)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(_texto));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        private string ReverterTexto(string _texto)
        {
            char[] charArray = _texto.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}