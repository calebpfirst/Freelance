using System;
using System.Windows.Forms;

namespace Lab4
{
    class DriverClass
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameGUI());
        }
    }
}
