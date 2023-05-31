using Infra;
using System;
using System.Windows.Forms;

namespace WindowsFormsAppPrincipal
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Log.Gravar("Usuário abriu o sistema.");
            
            Application.Run(new FormLogin());
        }
    }
}