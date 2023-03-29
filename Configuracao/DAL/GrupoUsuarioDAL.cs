﻿using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class GrupoUsuarioDAL
    {
        public void Inserir(GrupoUsuario _grupoUsuario)
        {
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "INSERT INTO GrupoUsuario(NomeGrupo) VALUES(@NomeGrupo)";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Parameters.AddWithValue("@NomeGrupo", _grupoUsuario.NomeGrupo);

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu erro ao tentar inserir um grupo de usuário no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public List<GrupoUsuario> BuscarTodos()
        {
            List<GrupoUsuario> grupoUsuarios = new List<GrupoUsuario>();
            GrupoUsuario grupoUsuario;

            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT Id, NomeGrupo FROM GrupoUsuario";
                cmd.CommandType = System.Data.CommandType.Text;

                cn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        grupoUsuario = new GrupoUsuario();
                        grupoUsuario.Id = Convert.ToInt32(rd["Id"]);
                        grupoUsuario.NomeGrupo = rd["NomeGrupo"].ToString();
                        grupoUsuarios.Add(grupoUsuario);
                    }
                }
                return grupoUsuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar todos os grupos de usuários no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public List<GrupoUsuario> BuscarPorNomeGrupo(string _nomeGrupo)
        {
            List<GrupoUsuario> grupoUsuarios = new List<GrupoUsuario>();
            GrupoUsuario grupoUsuario;

            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT Id, NomeGrupo FROM GrupoUsuario WHERE NomeGrupo LIKE @NomeGrupo";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@NomeGrupo", "%" + _nomeGrupo + "%");
                cn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        grupoUsuario = new GrupoUsuario();
                        grupoUsuario.Id = Convert.ToInt32(rd["Id"]);
                        grupoUsuario.NomeGrupo = rd["NomeGrupo"].ToString();
                        grupoUsuario.Permissoes = new PermissaoDAL().BuscarPorIdGrupo(grupoUsuario.Id);
                        grupoUsuarios.Add(grupoUsuario);
                    }
                }
                return grupoUsuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar grupos de usuário por nome do grupo no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public List<GrupoUsuario> BuscarPorId(int _id)
        {
            List<GrupoUsuario> grupoUsuarios = new List<GrupoUsuario>();
            GrupoUsuario grupoUsuario;

            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT Id, NomeGrupo FROM GrupoUsuario WHERE Id LIKE @Id";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", _id);
                cn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        grupoUsuario = new GrupoUsuario();
                        grupoUsuario.Id = Convert.ToInt32(rd["Id"]);
                        grupoUsuario.NomeGrupo = rd["NomeGrupo"].ToString();
                        grupoUsuarios.Add(grupoUsuario);
                    }
                }
                return grupoUsuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar grupos de usuários pr Id no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public void Alterar(GrupoUsuario _grupoUsuario)
        {
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UPDATE GrupoUsuario SET NomeGrupo = @NomeGrupo WHERE Id = @Id";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Parameters.AddWithValue("@NomeGrupo", _grupoUsuario.NomeGrupo);
                cmd.Parameters.AddWithValue("@Id", _grupoUsuario.Id);

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu erro ao tentar alterar um grupo de usuário no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public void Excluir(int _idGrupoUsuario, SqlTransaction _transaction = null)
        {
            using (SqlConnection cn = new SqlConnection(Conexao.StringDeConexao))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM GrupoUsuario WHERE Id = @Id", cn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idGrupoUsuario;

                    if (_transaction != null)
                    {
                        cmd.Transaction = _transaction;
                    }
                    else
                    {
                        cn.Open();
                        _transaction = cn.BeginTransaction();
                        cmd.Transaction = _transaction;
                    }

                    try
                    {
                        RemoverTodasPermissoes(_idGrupoUsuario, _transaction);
                        cmd.ExecuteNonQuery();
                        _transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _transaction.Rollback();
                        throw new Exception("Ocorreu erro ao tentar excluir um grupo de usuário no banco de dados.", ex);
                    }
                }
            }
        }

        private void RemoverTodasPermissoes(int _idGrupoUsuario, SqlTransaction _transaction = null)
        {
            using (SqlConnection cn = new SqlConnection(Conexao.StringDeConexao))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM PermissaoGrupoUsuario WHERE IdGrupoUsuario = @IdGrupoUsuario", cn))
                {
                    cmd.Parameters.Add("@IdGrupoUsuario", SqlDbType.Int).Value = _idGrupoUsuario;

                    if (_transaction != null)
                    {
                        cmd.Transaction = _transaction;
                    }
                    else
                    {
                        cn.Open();
                        _transaction = cn.BeginTransaction();
                        cmd.Transaction = _transaction;
                    }

                    try
                    {
                        cmd.ExecuteNonQuery();
                        _transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _transaction.Rollback();
                        throw new Exception("Ocorreu um erro ao tentar excluir todas as permissões do grupo no banco de dados.", ex) { Data = { { "Id", -1 } } };
                    }
                }
            }
        }


        //public void Excluir(int _id)
        //{
        //    SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
        //    try
        //    {
        //        SqlCommand cmd = cn.CreateCommand();
        //        cmd.CommandText = "DELETE FROM GrupoUsuario WHERE Id = @Id";
        //        cmd.CommandType = System.Data.CommandType.Text;

        //        cmd.Parameters.AddWithValue("@Id", _id);

        //        cmd.Connection = cn;
        //        cn.Open();

        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Ocorreu erro ao tentar excluir um grupo de usuário no banco de dados.", ex);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }
        //}

        public List<GrupoUsuario> BuscarPorIdUsuario(int _idUsuario)
        {
            List<GrupoUsuario> grupoUsuarios = new List<GrupoUsuario>();
            GrupoUsuario grupoUsuario;

            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"SELECT GrupoUsuario.Id, GrupoUsuario.NomeGrupo FROM GrupoUsuario
                                    INNER JOIN UsuarioGrupoUsuario ON GrupoUsuario.Id = UsuarioGrupoUsuario.IdGrupoUsuario
                                    WHERE UsuarioGrupoUsuario.IdUsuario = @IdUsuario";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@IdUsuario", _idUsuario);
                cn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        grupoUsuario = new GrupoUsuario();
                        grupoUsuario.Id = Convert.ToInt32(rd["Id"]);
                        grupoUsuario.NomeGrupo = rd["NomeGrupo"].ToString();
                        grupoUsuarios.Add(grupoUsuario);
                    }
                }
                return grupoUsuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar buscar grupos de usuários por Id do usuário no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public bool PermissaoVinculada(int _idGrupo, int _idPermissao)
        {
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"SELECT 1 FROM PermissaoGrupoUsuario 
                                    WHERE IdGrupoUsuario = @IdGrupoUsuario AND IdPermissao = @IdPermissao";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@IdGrupoUsuario", _idGrupo);
                cmd.Parameters.AddWithValue("@IdPermissao", _idPermissao);
                cn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    return rd.Read();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar verificar vinculo entre permissão e grupo de usuário no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public void AdicionarPermissao(int _idGrupo, int _idPermissao)
        {
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "INSERT INTO PermissaoGrupoUsuario(IdGrupoUsuario, IdPermissao) VALUES(@IdGrupoUsuario, @IdPermissao)";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Parameters.AddWithValue("@IdGrupoUsuario", _idGrupo);
                cmd.Parameters.AddWithValue("@IdPermissao", _idPermissao);

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu erro ao tentar inserir permissão um grupo de usuário no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
        public void RemoverPermissao(int _idGrupo, int _idPermissao)
        {
            SqlConnection cn = new SqlConnection(Conexao.StringDeConexao);
            try
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = @"DELETE FROM PermissaoGrupoUsuario 
                                    WHERE IdGrupoUsuario = @IdGrupoUsuario AND IdPermissao = @IdPermissao";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@IdGrupoUsuario", _idGrupo);
                cmd.Parameters.AddWithValue("@IdPermissao", _idPermissao);

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu erro ao tentar excluir a permissão de um grupo de usuário no banco de dados.", ex);
            }
            finally
            {
                cn.Close();
            }
        }
    }
}