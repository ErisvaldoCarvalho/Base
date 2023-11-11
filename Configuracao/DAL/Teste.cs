using DAL;
using Models;
using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL2
{
    public class UsuarioDAL
    {
        private Utils utils;
        public UsuarioDAL()
        {
            utils = new Utils(new Usuario());
        }
        public void Salvar(Usuario _usuario, bool _excluir = false, SqlTransaction _transaction = null)
        {
            SqlTransaction transaction = _transaction;

            using (SqlConnection cn = new SqlConnection(Conexao.StringDeConexao))
            {
                string script = _usuario.Id != 0 ? utils.ScriptUpdate : utils.ScriptInsert;

                if (_excluir)
                    script = utils.ScriptDelete;

                using (SqlCommand cmd = new SqlCommand(script, cn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        utils.PopularParametros(cmd, _usuario, _usuario.Id != 0, _excluir);

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


                        int idException = 1;
                        string operacao = "inserir";

                        if (_usuario != null)
                            if (_usuario.Id == 0)
                            {
                                operacao = "alterar";
                                idException = 2;
                            }
                            else if (_excluir)
                            {
                                operacao = "excluir";
                                idException = 3;
                            }

                        throw new Exception($"Ocorreu um erro ao tentar {operacao} usuário no banco de dados.", ex) { Data = { { "Id", idException } } };
                    }
                }
            }
        }
        public void Excluir(int _id, SqlTransaction _transaction = null)
        {
            Salvar(new Usuario() { Id = _id }, true, _transaction);
        }
        public List<Usuario> BuscarTodos(SqlCommand _cmd = null)
        {
            List<Usuario> usuarioList = new List<Usuario>();
            Usuario usuario = new Usuario();
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = utils.ScriptSelect;
                cmd.CommandType = System.Data.CommandType.Text;

                if (_cmd != null)
                {
                    cmd.CommandText = _cmd.CommandText;
                    foreach (SqlCommand item in _cmd.Parameters)
                        cmd.Parameters.Add(item);
                }

                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        usuario = PreencherObjeto(rd);
                        usuarioList.Add(usuario);
                    }
                }
                return usuarioList;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar usuário no banco de dados.", ex) { Data = { { "Id", 4 } } };
            }
            finally
            {
                cn.Close();
            }
        }
        public Usuario BuscarPorId(int _id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"{utils.ScriptSelect} WHERE Usuario.Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", _id);
                    List<Usuario> usuarioList = BuscarTodos(cmd);

                    return usuarioList.Count > 0 ? usuarioList[0] : null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar usuário por Id no banco de dados.", ex) { Data = { { "Id", 4 } } };
            }
        }
        private static Usuario PreencherObjeto(SqlDataReader _rd)
        {
            Usuario usuario = new Usuario();

            for (int i = 0; i < _rd.FieldCount; i++)
            {
                string nomeCampo = _rd.GetName(i);
                object valorCampo = _rd.GetValue(i);

                PropertyInfo propriedade = usuario.GetType().GetProperty(nomeCampo);

                if (propriedade != null && valorCampo != DBNull.Value)
                {
                    // Converte o valor do campo para o tipo da propriedade
                    object valorConvertido = Convert.ChangeType(valorCampo, propriedade.PropertyType);

                    // Define o valor da propriedade
                    propriedade.SetValue(usuario, valorConvertido);
                }
            }
            usuario.GrupoUsuarios = new GrupoUsuarioDAL().BuscarPorIdUsuario(usuario.Id);

            return usuario;
        }
        //private static Usuario PreencherObjeto(SqlDataReader _rd)
        //{
        //    Usuario usuario = new Usuario();
        //    Type type = usuario.GetType();
        //    PropertyInfo[] propriedades = type.GetProperties();

        //    usuario.Id = Convert.ToInt32(_rd["Id"]);
        //    usuario.Nome = _rd["Nome"].ToString();
        //    usuario.NomeUsuario = _rd["NomeUsuario"].ToString();
        //    usuario.Email = _rd["Email"].ToString();
        //    usuario.CPF = _rd["CPF"].ToString();
        //    usuario.Ativo = Convert.ToBoolean(_rd["Ativo"]);
        //    usuario.Senha = _rd["Senha"].ToString();
        //    usuario.GrupoUsuarios = new GrupoUsuarioDAL().BuscarPorIdUsuario(usuario.Id);
        //    return usuario;
        //}
    }
}