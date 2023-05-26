using BLL;
using Models;
using System;
using System.Windows.Forms;

namespace WindowsFormsAppPrincipal
{
    public partial class FormCadastroUsuario : Form
    {
        public int Id;

        public FormCadastroUsuario(int _id = 0)
        {
            InitializeComponent();
            Id = _id;
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Id == 0 && senhaTextBox.Text == "*******")
                    throw new Exception("A senha informada é fraca, informe uma senha mais forte.") { Data = { { "Id", 1 } } };

                if (Id == 0 || (Id != 0 && senhaTextBox.Text != "*******"))
                    ((Usuario)usuarioBindingSource.Current).Senha = senhaTextBox.Text;

                usuarioBindingSource.EndEdit();

                if (Id == 0)
                    new UsuarioBLL().Inserir((Usuario)usuarioBindingSource.Current, textBoxConfirmarSenha.Text);
                else if (textBoxConfirmarSenha.Text != "*******")
                    new UsuarioBLL().Alterar((Usuario)usuarioBindingSource.Current, textBoxConfirmarSenha.Text);
                else
                    new UsuarioBLL().Alterar((Usuario)usuarioBindingSource.Current);

                MessageBox.Show("Registro salvo com sucesso!");
                Close();

            }
            catch (Exception ex)
            {
                int idErro = new TratarErro().GetId(ex);
                if (idErro == 1)
                {
                    textBoxConfirmarSenha.Text =
                    senhaTextBox.Text = "";
                    senhaTextBox.Focus();
                }

                MessageBox.Show(ex.Message);
            }
        }
        private void FormCadastroUsuario_Load(object sender, EventArgs e)
        {
            try
            {
                if (Id == 0)
                    usuarioBindingSource.AddNew();
                else
                {
                    usuarioBindingSource.DataSource = new UsuarioBLL().BuscarPorId(Id);
                    textBoxConfirmarSenha.Text =
                    senhaTextBox.Text = "*******";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormCadastroUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void senhaTextBox_TextChanged(object sender, EventArgs e)
        {
            if (textBoxConfirmarSenha.Text == "*******")
                textBoxConfirmarSenha.Text = "";
        }
    }
}
