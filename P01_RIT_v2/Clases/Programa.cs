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

        [STAThread]
        static void Main() {
            //Para visualizar las rutas en la consola
            //Console.Write(Opciones.Instance);

            //Crear un nuevo archivo invertido
            //Invertido nuevoInvertido = new Invertido();
            //nuevoInvertido.indexarColeccion();
            //nuevoInvertido.exportarArchivoInvertido("Prueba.xml");

            //Obtener un archivo invertido previamente creado
            //Invertido nuevoInvertido = Invertido.importarArchivoInvertido("Prueba.xml");

            //Visualizar el archivo invertido
            //Console.Write(nuevoInvertido);
            //Console.ReadLine();

            //Opciones.Instance.guardarOpciones();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

    }
}
