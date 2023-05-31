using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppPrincipal
{
    public partial class FormExibirLogs : Form
    {
        public FormExibirLogs()
        {
            InitializeComponent();
        }

        private void FormExibirLogs_Load(object sender, EventArgs e)
        {
            textBoxLogs.Text = string.Join(Environment.NewLine, new Infra.Arquivo().LerLinhasArquivo(Constantes.CaminhoArquivoLog, true));
        }
    }
}
