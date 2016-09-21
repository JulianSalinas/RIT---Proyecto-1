using P01_RIT_v2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P01_RIT_v2.Clases
{
    class Programa
    {
        // Ejecución del programa.
        [STAThread]
        static void Main()
        {
            //Para visualizar las rutas en la consola
            //Console.Write(Opciones.Instance);

            //Opciones.Instance.guardarOpciones();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
