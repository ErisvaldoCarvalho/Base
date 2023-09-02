using BLL;
using Models;
using System;
using System.Windows.Forms;

namespace WindowsFormsAppPrincipal
{
    public partial class FormPDV : Form
    {
        private int idVenda;
        public FormPDV(int _idVenda = 0)
        {
            InitializeComponent();
            idVenda = _idVenda;
        }

        private void FormPDV_Load(object sender, System.EventArgs e)
        {
            if (idVenda == 0)
                vendaBindingSource.AddNew();
            else
                vendaBindingSource.DataSource = new VendaBLL().BuscarPorId(idVenda);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    ((ItemVenda)itemVendaListBindingSource.Current).Produto = new ProdutoBLL().BuscarPorCodigoDeBarras(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
