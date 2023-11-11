using Models;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace DAL
{
    internal class Utils
    {
        private string nomeTabela;
        private SqlCommand sqlCommand;
        public string ScriptInsert { get; set; }
        public string ScriptUpdate { get; set; }
        public string ScriptDelete { get; set; }
        public string ScriptSelect { get; set; }

        public Utils(object _objeto)
        {
            nomeTabela = _objeto.GetType().Name;
            ScriptInsert = $"INSERT INTO {nomeTabela}({Campos(_objeto).Replace("Id, ", "")}) VALUES(@{Campos(_objeto).Replace("Id, ", "").Replace(", ", ", @")})";
            ScriptSelect = $"SELECT {nomeTabela}.{Campos(_objeto).Replace(", ", $", {nomeTabela}.")} FROM {nomeTabela}";
            ScriptUpdate = Update(_objeto);
            ScriptDelete = $"DELETE FROM {nomeTabela} WHERE Id = @Id"; ;
        }
        private string Update(object _objeto)
        {
            Type type = _objeto.GetType();
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
        private string Campos(object _objeto)
        {
            Type type = _objeto.GetType();
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
        public void PopularParametros(SqlCommand _cmd, object _objeto, bool parametrosUpdate = true, bool _excluir = false)
        {
            Type type = _objeto.GetType();
            PropertyInfo[] propriedades = type.GetProperties();

            foreach (PropertyInfo item in propriedades)
            {
                if (!item.PropertyType.Name.Contains("List"))
                {
                    object valor = item.GetValue(_objeto);
                    if (_excluir)
                    {
                        _cmd.Parameters.AddWithValue($"@Id", valor ?? DBNull.Value);
                        return;
                    }
                    else if (item.Name == "Id" && parametrosUpdate)
                        _cmd.Parameters.AddWithValue($"@{item.Name}", valor ?? DBNull.Value);
                    else if (item.Name != "Id")
                        _cmd.Parameters.AddWithValue($"@{item.Name}", valor ?? DBNull.Value);
                }
            }
        }
        public void PopularParametros(SqlCommand _cmd, bool parametrosUpdate = true)
        {
            _cmd.Parameters.Clear();
            foreach (SqlParameter item in sqlCommand.Parameters)
            {
                if (item.ParameterName == "@Id" && parametrosUpdate)
                    _cmd.Parameters.Add(item);
                else if (item.ParameterName != "@Id")
                    _cmd.Parameters.Add(item);
            }
        }
    }
}
