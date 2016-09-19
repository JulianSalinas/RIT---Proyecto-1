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
            Console.Write(Opciones.Instance);
            Console.ReadLine();

            Invertido nuevoInvertido = new Invertido();
                
            nuevoInvertido.indexarColeccion();
            Console.Write(nuevoInvertido);
            Console.ReadLine();

            nuevoInvertido.exportarArchivoInvertido("Prueba.xml");

            // Invertido.importarDesdeXml("Prueba.xml");


            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());*/
        }

    }
}
