using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Infra
{
    public class RSAHelper
    {
        public static RSAParameters GenerateRSAKey()
        {
            using (var rsa = RSA.Create())
            {
                rsa.KeySize = 2048;
                return rsa.ExportParameters(includePrivateParameters: false);
            }
        }

        public static string RSAParametersToString(RSAParameters parameters)
        {
            StringBuilder sb = new StringBuilder();

            // Append Modulus if not null
            if (parameters.Modulus != null)
                sb.AppendLine(Convert.ToBase64String(parameters.Modulus));

            // Append Exponent if not null
            if (parameters.Exponent != null)
                sb.AppendLine(Convert.ToBase64String(parameters.Exponent));

            // Append P if not null
            if (parameters.P != null)
                sb.AppendLine(Convert.ToBase64String(parameters.P));

            // Append Q if not null
            if (parameters.Q != null)
                sb.AppendLine(Convert.ToBase64String(parameters.Q));

            // Append DP if not null
            if (parameters.DP != null)
                sb.AppendLine(Convert.ToBase64String(parameters.DP));

            // Append DQ if not null
            if (parameters.DQ != null)
                sb.AppendLine(Convert.ToBase64String(parameters.DQ));

            // Append InverseQ if not null
            if (parameters.InverseQ != null)
                sb.AppendLine(Convert.ToBase64String(parameters.InverseQ));

            // Append D if not null
            if (parameters.D != null)
                sb.AppendLine(Convert.ToBase64String(parameters.D));

            return sb.ToString();
        }
        public static RSAParameters StringToRSAParameters(string chaveString)
        {
            byte[] chaveBytes = Convert.FromBase64String(chaveString);
            using (var stream = new MemoryStream(chaveBytes))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var chave = new RSAParameters();

                    chave.Modulus = reader.ReadBytes(reader.ReadInt32());
                    chave.Exponent = reader.ReadBytes(reader.ReadInt32());

                    int numP = reader.ReadInt32();
                    if (numP > 0)
                    {
                        chave.P = reader.ReadBytes(numP);
                    }

                    int numQ = reader.ReadInt32();
                    if (numQ > 0)
                    {
                        chave.Q = reader.ReadBytes(numQ);
                    }

                    int numDP = reader.ReadInt32();
                    if (numDP > 0)
                    {
                        chave.DP = reader.ReadBytes(numDP);
                    }

                    int numDQ = reader.ReadInt32();
                    if (numDQ > 0)
                    {
                        chave.DQ = reader.ReadBytes(numDQ);
                    }

                    int numInverseQ = reader.ReadInt32();
                    if (numInverseQ > 0)
                    {
                        chave.InverseQ = reader.ReadBytes(numInverseQ);
                    }

                    int numD = reader.ReadInt32();
                    if (numD > 0)
                    {
                        chave.D = reader.ReadBytes(numD);
                    }

                    return chave;
                }
            }
        }


        private static byte[] ExportParametersToBytes(RSAParameters parameters)
        {
            byte[] modulusBytes = parameters.Modulus;
            byte[] exponentBytes = parameters.Exponent;
            byte[] pBytes = parameters.P;
            byte[] qBytes = parameters.Q;
            byte[] dpBytes = parameters.DP;
            byte[] dqBytes = parameters.DQ;
            byte[] inverseQBytes = parameters.InverseQ;
            byte[] dBytes = parameters.D;

            int totalSize = modulusBytes.Length + exponentBytes.Length + pBytes.Length + qBytes.Length + dpBytes.Length + dqBytes.Length + inverseQBytes.Length + dBytes.Length;
            byte[] result = new byte[totalSize];

            int offset = 0;
            CopyToByteArray(modulusBytes, result, ref offset);
            CopyToByteArray(exponentBytes, result, ref offset);
            CopyToByteArray(pBytes, result, ref offset);
            CopyToByteArray(qBytes, result, ref offset);
            CopyToByteArray(dpBytes, result, ref offset);
            CopyToByteArray(dqBytes, result, ref offset);
            CopyToByteArray(inverseQBytes, result, ref offset);
            CopyToByteArray(dBytes, result, ref offset);

            return result;
        }

        private static RSAParameters ImportParametersFromBytes(byte[] publicKeyBytes)
        {
            RSAParameters parameters = new RSAParameters();

            int offset = 0;
            parameters.Modulus = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.Exponent = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.P = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.Q = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.DP = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.DQ = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.InverseQ = CopyFromByteArray(publicKeyBytes, ref offset);
            parameters.D = CopyFromByteArray(publicKeyBytes, ref offset);

            return parameters;
        }

        private static void CopyToByteArray(byte[] source, byte[] destination, ref int offset)
        {
            Array.Copy(source, 0, destination, offset, source.Length);
            offset += source.Length;
        }

        private static byte[] CopyFromByteArray(byte[] source, ref int offset)
        {
            int length = BitConverter.ToInt32(source, offset);
            offset += sizeof(int);

            byte[] result = new byte[length];
            Array.Copy(source, offset, result, 0, length);
            offset += length;

            return result;
        }
    }
}