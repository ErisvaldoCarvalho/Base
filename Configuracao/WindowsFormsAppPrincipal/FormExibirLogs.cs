using Infra;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            string[] arquivos = new Arquivo().ListarArquivos(Constantes.DiretorioArquivoLog, "Log*.log");
            dataGridView1.Columns.Add("DataArquivo", "Data");

            dataGridView1.Rows.Clear();

            foreach (string arquivoDeLog in arquivos)
            {
                string nomeArquivo = Path.GetFileName(arquivoDeLog);
                string dataArquivo = nomeArquivo.Substring(9, 2) + "/" + nomeArquivo.Substring(7, 2) + "/" + nomeArquivo.Substring(3, 4);
                dataGridView1.Rows.Add(dataArquivo);
            }

            //textBoxLogs.Text = string.Join(Environment.NewLine, new Infra.Arquivo().LerLinhasArquivo(Constantes.CaminhoArquivoLog, true));
        }
    }
}
