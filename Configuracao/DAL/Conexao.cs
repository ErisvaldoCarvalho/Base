﻿namespace DAL
{
    public static class Conexao
    {
        public static string StringDeConexao 
        { 
            get
            {
                return @"User ID=Erisvaldo;
                         Initial Catalog=Configuracao;
                         Data Source=.\SQLEXPRESS2019;
                         Password=123";
            }
        }
    }
}