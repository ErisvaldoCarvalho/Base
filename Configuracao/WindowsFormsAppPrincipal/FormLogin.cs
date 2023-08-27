using BLL;
using System;
using System.Windows.Forms;
using Infra;

namespace WindowsFormsAppPrincipal
{
    public partial class FormLogin : Form
    {
        public bool Logou;

        public FormLogin()
        {
            InitializeComponent();
            Logou = false;
        }

        private void buttonEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                Log.Gravar("buttonEntrar_Click");
                new UsuarioBLL().Altenticar(textBoxUsuario.Text, textBoxSenha.Text);
                Logou = true;
                Close();
            }
            catch (Exception ex)
            {
                Log.Gravar(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void textBoxUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxSenha.Focus();
        }

        private void textBoxSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonEntrar_Click(null, null);
        }
    }
}