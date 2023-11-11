﻿using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace DAL
{
    public class DALModelo<T> where T : new()
    {
        private string nomeTabela;
        protected Dictionary<Operacao, int> idExceptionSalvar;
        public string ScriptInsert { get; set; }
        public string ScriptUpdate { get; set; }
        public string ScriptDelete { get; set; }
        public string ScriptSelect { get; set; }

        public DALModelo()
        {
            T t = new T();

            idExceptionSalvar = new Dictionary<Operacao, int>();

            nomeTabela = typeof(T).Name;

            ScriptInsert = $"INSERT INTO {nomeTabela}({Campos(t).Replace("Id, ", "")}) VALUES(@{Campos(t).Replace("Id, ", "").Replace(", ", ", @")})";
            ScriptSelect = $"SELECT {nomeTabela}.{Campos(t).Replace(", ", $", {nomeTabela}.")} FROM {nomeTabela}";
            ScriptUpdate = UpdateScript(t);
            ScriptDelete = $"DELETE FROM {nomeTabela} WHERE Id = @Id";
        }
        public enum Operacao
        {
            [Description("inserir")]
            Inserir,

            [Description("alterar")]
            Alterar,

            [Description("excluir")]
            Excluir
        }
        protected string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
        private string UpdateScript(T _objeto)
        {
            Type type = typeof(T);
            PropertyInfo[] propriedades = type.GetProperties();

            StringBuilder campos = new StringBuilder();

            foreach (PropertyInfo item in propriedades)
            {
                if (item.Name != "Id" && !item.PropertyType.Name.Contains("List"))
                {
                    campos.Append($"{item.Name} = @{item.Name}");
                    campos.Append(",\n");
                }
            }

            if (campos.Length > 0)
                campos.Length -= 2; // Remove a última vírgula

            return $"UPDATE {nomeTabela} SET\n{campos.ToString()}\nWHERE Id = @Id";
        }
        private string Campos(T _objeto)
        {
            Type type = typeof(T);
            PropertyInfo[] propriedades = type.GetProperties();

            StringBuilder campos = new StringBuilder();

            foreach (PropertyInfo item in propriedades)
            {
                if (!item.PropertyType.Name.Contains("List"))
                {
                    campos.Append(item.Name);
                    campos.Append(", ");
                }
            }

            if (campos.Length > 0)
                campos.Length -= 2; // Remove a última vírgula

            return campos.ToString();
        }
        public void PopularParametros(SqlCommand _cmd, T _objeto)
        {
            _cmd.Parameters.Clear();
            Type type = typeof(T);
            PropertyInfo[] propriedades = type.GetProperties();

            foreach (PropertyInfo item in propriedades)
            {
                if (!item.PropertyType.Name.Contains("List"))
                {
                    object valor = item.GetValue(_objeto);
                    _cmd.Parameters.AddWithValue($"@{item.Name}", valor ?? DBNull.Value);
                }
            }
        }
        public void PopularParametrosExclusao(SqlCommand _cmd, T _objeto)
        {
            _cmd.Parameters.Clear();
            PropertyInfo propriedadeId = typeof(T).GetProperty("Id");
            if (propriedadeId != null)
            {
                object valorId = propriedadeId.GetValue(_objeto);
                _cmd.Parameters.AddWithValue("@Id", valorId ?? DBNull.Value);
            }
        }
        public virtual void Salvar(T _entidade, bool _excluir = false, SqlTransaction _transaction = null)
        {
            SqlTransaction transaction = _transaction;
            PropertyInfo propriedadeId = typeof(T).GetProperty("Id");
            int id = 0;

            if (propriedadeId != null)
            {
                object valorId = propriedadeId.GetValue(_entidade);
                id = (int)valorId;
            }

            using (SqlConnection cn = new SqlConnection(Conexao.StringDeConexao))
            {
                string script = id != 0 ? ScriptUpdate : ScriptInsert;

                if (_excluir)
                    script = ScriptDelete;

                using (SqlCommand cmd = new SqlCommand(script, cn))
                {
                    Operacao operacao = id == 0 ? Operacao.Inserir : Operacao.Alterar;

                    if (_excluir)
                        operacao = Operacao.Excluir;

                    try
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        PopularParametros(cmd, _entidade);

                        if (_transaction == null)
                        {
                            cn.Open();
                            transaction = cn.BeginTransaction();
                        }

                        cmd.Transaction = transaction;
                        cmd.Connection = transaction.Connection;

                        cmd.ExecuteNonQuery();

                        if (_transaction == null)
                            transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction.Connection != null && transaction.Connection.State == System.Data.ConnectionState.Open)
                            transaction.Rollback();

                        throw new Exception("Ocorreu um erro ao tentar " + operacao + " " + Constantes.Verbose(_entidade.GetType().Name).ToLower() + " no banco de dados.", ex) { Data = { { "Id", idExceptionSalvar[operacao] } } };
                    }
                }
            }
        }
        public virtual void Excluir(int _id)
        {
            T t = new T();
            PropertyInfo id = t.GetType().GetProperty("Id");

            id.SetValue(t, _id, null);

            Salvar(t, true);
        }
        public virtual List<T> BuscarTodos(SqlCommand _cmd = null)
        {
            List<T> tList = new List<T>();
            T t = new T();
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = ScriptSelect;
                cmd.CommandType = System.Data.CommandType.Text;

                if (_cmd != null)
                {
                    cmd.CommandText = _cmd.CommandText;
                    cmd.Parameters.Clear();
                    foreach (SqlParameter item in _cmd.Parameters)
                        cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
                }

                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        t = PreencherObjeto(rd);
                        tList.Add(t);
                    }
                }
                return tList;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar " + Constantes.Verbose(t.GetType().Name).ToLower() + " no banco de dados.", ex) { Data = { { "Id", 4 } } };
            }
            finally
            {
                cn.Close();
            }
        }
        public virtual T BuscarPorId(int _id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = ScriptSelect + " WHERE Produto.Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", _id);
                    List<T> tList = BuscarTodos(cmd);
                    return tList[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar " + Constantes.Verbose(typeof(T).Name).ToLower() + " por Id no banco de dados.", ex) { Data = { { "Id", 3 } } };
            }
        }
        protected T PreencherObjeto(SqlDataReader _rd)
        {
            T t = new T();

            for (int i = 0; i < _rd.FieldCount; i++)
            {
                string nomeCampo = _rd.GetName(i);
                object valorCampo = _rd.GetValue(i);

                PropertyInfo propriedade = t.GetType().GetProperty(nomeCampo);

                if (propriedade != null && valorCampo != DBNull.Value)
                {
                    // Converte o valor do campo para o tipo da propriedade
                    object valorConvertido = Convert.ChangeType(valorCampo, propriedade.PropertyType);

                    // Define o valor da propriedade
                    propriedade.SetValue(t, valorConvertido);
                    PopularEntidadesRelacionadas(t);
                }
            }

            return t;
        }
        public virtual void PopularEntidadesRelacionadas(T _objeto)
        {

        }
    }
}