using Models;
using System.Reflection;
using System.Text;
using System;
using System.Linq;

public class TabModelo
{
    public string ScriptInsert
    {
        get
        {
            return $"INSERT INTO {NomeTabela()}({Campos()}) VALUES(@{Campos().Replace("Id, ", "").Replace(", ", ", @")})";
        }
    }
    public string ScriptUpdate
    {
        get
        {
            Type type = this.GetType();
            PropertyInfo[] propriedades = type.GetProperties();

            StringBuilder campos = new StringBuilder();

            foreach (PropertyInfo item in propriedades)
            {
                if (item.Name != "Id" && !item.PropertyType.Name.Contains("List"))
                {
                    campos.Append($"{item.Name} = @{item.Name}");
                    campos.Append(", ");
                }
            }

            if (campos.Length > 0)
                campos.Length -= 2; // Remove a última vírgula

            return $"UPDATE {NomeTabela()} SET {campos.ToString()} WHERE Id = @Id";
        }
    }
    public string ScriptDelete
    {
        get
        {
            return $"DELETE FROM {NomeTabela()} WHERE Id = @Id";
        }
    }
    public string ScriptSelect
    {
        get
        {
            return "";// $"SELECT {Campos()} FROM {NomeTabela()}";
        }
    }
    private string NomeTabela()
    {
        return this.GetType().Name;
    }
    private string Campos()
    {
        return "";
        //Type type = this.GetType();
        //PropertyInfo[] propriedades = type.GetProperties();

        //StringBuilder campos = new StringBuilder();

        //foreach (PropertyInfo item in propriedades)
        //{
        //    if (!item.PropertyType.Name.Contains("List"))
        //    {
        //        campos.Append(item.Name);
        //        campos.Append(", ");
        //    }
        //}

        //if (campos.Length > 0)
        //    campos.Length -= 2; // Remove a última vírgula

        //return campos.ToString();
    }
}