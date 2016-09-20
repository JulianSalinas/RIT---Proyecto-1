using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    class Programa
    {




        // Ejecución del programa.
        [STAThread]
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Ejecuta el programa mediante interfaz de consola.
            InterfazConsola.abrirMenuPrincipal();
        }
    }
}
