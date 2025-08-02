using System;
using System.Windows.Forms;

namespace WIndowsForm
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MenuPrincipal());
        }
    }
}