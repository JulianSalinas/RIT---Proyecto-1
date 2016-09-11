using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    class Programa
    {

        [STAThread]
        static void Main() {
            Opciones.Instance.cargarOpciones();
            Console.Write(Opciones.Instance);
            Console.ReadLine();

            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());*/
        }

    }
}
