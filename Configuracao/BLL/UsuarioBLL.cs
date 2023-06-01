using DAL;
using Infra;
using Models;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;

namespace BLL
{
    public class UsuarioBLL
    {
        public void Inserir(Usuario _usuario, string _confirmacaoDeSenha)
        {
            ValidarPermissao(2);
            ValidarDados(_usuario, _confirmacaoDeSenha);

            _usuario.Senha = new Criptografia().CriptografarSenha(_usuario.Senha);

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            usuarioDAL.Inserir(_usuario);
        }
        public void Alterar(Usuario _usuario, string _confirmacaoDeSenha = "n1Xc09TSv3e2GxqNApdZNbmbrWJbucjNI2p+kvUHLOY=")
        {
            ValidarPermissao(3);
            if (_confirmacaoDeSenha != "n1Xc09TSv3e2GxqNApdZNbmbrWJbucjNI2p+kvUHLOY=")
                ValidarDados(_usuario, _confirmacaoDeSenha);
            else
                ValidarDados(_usuario, _usuario.Senha);

            if (_confirmacaoDeSenha != "n1Xc09TSv3e2GxqNApdZNbmbrWJbucjNI2p+kvUHLOY=")
                _usuario.Senha = new Criptografia().CriptografarSenha(_usuario.Senha);

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            usuarioDAL.Alterar(_usuario);
        }
        public void Excluir(int _id)
        {
            ValidarPermissao(4);
            new UsuarioDAL().Excluir(_id);
        }
        public List<Usuario> BuscarTodos()
        {
            ValidarPermissao(1);
            return new UsuarioDAL().BuscarTodos();
        }
        public Usuario BuscarPorId(int _id)
        {
            ValidarPermissao(1);
            return new UsuarioDAL().BuscarPorId(_id);
        }
        public Usuario BuscarPorCPF(string _cPF)
        {
            ValidarPermissao(1);
            return new UsuarioDAL().BuscarPorCPF(_cPF);
        }
        public List<Usuario> BuscarPorNome(string _nome)
        {
            ValidarPermissao(1);
            return new UsuarioDAL().BuscarPorNome(_nome);
        }
        public Usuario BuscarPorNomeUsuario(string _nomeUsuario)
        {
            ValidarPermissao(1);
            return new UsuarioDAL().BuscarPorNomeUsuario(_nomeUsuario);
        }
        private void ValidarDados(Usuario _usuario, string _confirmacaoDeSenha)
        {
            if (_usuario.Senha != _confirmacaoDeSenha)
                throw new Exception("A senha e a confirmação da senha devem ser iguais.") { Data = { { "Id", 1 } } };

            if (_usuario.Senha.Length <= 3)
                throw new Exception("A senha deve ter mais de 3 caracteres.") { Data = { { "Id", 1 } } };

            if (_usuario.Nome.Length <= 2)
                throw new Exception("A nome deve ter mais de 2 caracteres.");
        }
        public void ValidarPermissao(int _idPermissao)
        {
            if (!new UsuarioDAL().ValidarPermissao(Constantes.IdUsuarioLogado, _idPermissao))
                throw new Exception("Você não tem permissão de realizar essa operação. Procure o administrador do sistema.");
        }
        public void AdicionarGrupoUsuario(int _idUsuario, int _idGrupoUsuario)
        {
            if (!new UsuarioDAL().UsuarioPertenceAoGrupo(_idUsuario, _idGrupoUsuario))
                new UsuarioDAL().AdicionarGrupoUsuario(_idUsuario, _idGrupoUsuario);
        }
        public void RemoverGrupoUsuario(int _idUsuario, int _idGrupoUsuario)
        {
            new UsuarioDAL().RemoverGrupoUsuario(_idUsuario, _idGrupoUsuario);
        }
        public void Altenticar(string _nomeUsuario, string _senha)
        {
            Log.Gravar("Método Altenticar usuário: " + _nomeUsuario);
            Usuario usuario = new UsuarioDAL().BuscarPorNomeUsuario(_nomeUsuario);
            if (new Criptografia().CriptografarSenha(_senha) == usuario.Senha && usuario.Ativo)
            {
                Constantes.IdUsuarioLogado = usuario.Id;
                Log.Gravar("O usuário altenticou como: " + usuario.NomeUsuario);
            }
            else
                throw new Exception("Usuario ou senha inválido.");
        }
    }
}
