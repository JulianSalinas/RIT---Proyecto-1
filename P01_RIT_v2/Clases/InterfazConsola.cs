using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    public class InterfazConsola
    {
        /// <summary>
        /// Archivo Invertido cargado en el programa.
        /// </summary>
        static private Invertido archivoInvertidoPrograma;

        /// <summary>
        /// Crea un nuevo archivo invertido y lo carga en el programa.
        /// </summary>
        /// <returns>
        /// True si el archivo invertido pudo se creado. False en caso contrario.
        /// </returns>
        static private bool crearArchivoInvertidoDefault()
        {
            string[] mensaje =
                { "--- Crear Nuevo Archivo Invertido ---\n",
                "Se creará un nuevo archivo invertido utilizando los archivos de las siguientes carpetas:",
                "Carpeta de Colección de Documentos: " + Opciones.Instance.RutaColeccion,
                "Carpeta con Stopwords: " + Opciones.Instance.RutaStopWords,
                "Presione una tecla para continuar...\n"
                };


            foreach (string linea in mensaje)
            {
                Console.WriteLine(linea);
            }

            Console.ReadKey(true);

            try
            {
                // Genera nuevo archivo invertido.
                Invertido nuevoArchivoInvertido = Invertido.generarArchivoInvertidoPorDefecto();

                // Nuevo archivo invertido ahora está cargado en el programa.
                archivoInvertidoPrograma = nuevoArchivoInvertido;

                Console.WriteLine("Archivo invertido creado. El nuevo archivo invertido ha sido cargado al programa.");
                Console.WriteLine("¿Desea guardar el archivo invertido creado? (S/N)\n");
                while (true)
                {
                    ConsoleKey teclaPresionada = Console.ReadKey(true).Key;
                    if (teclaPresionada == ConsoleKey.S)
                    {
                        guardarArchivoInvertido();

                        Console.WriteLine("El archivo invertido ha sido guardado.\n");
                        break;

                    }
                    else if (teclaPresionada == ConsoleKey.N)
                    {
                        break;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Hubo un error al crear el archivo invertido.");
            }
            return false;
        }


        /// <summary>
        /// Guarda el Archivo Invertido cargado en un archivo XML externo.
        /// </summary>
        static private void guardarArchivoInvertido()
        {
            string[] mensaje =
            {
                "--- Guardar archivo invertido ---",
                "Escriba el nombre para el archivo invertido que desea guardar.\nEl archivo invertido estará guardado en la siguiente carpeta: ",
                Opciones.Instance.RutaArchivos + "\n",
                "Nota: Si no pone ningún nombre, se utilizarán la fecha y hora del sistema.\n"
            };

            foreach (string linea in mensaje)
            {
                Console.WriteLine(linea);
            }

            Console.Write("Nombre del archivo: ");
            string nombreNuevoArchivo = Console.ReadLine();

            try
            {
                archivoInvertidoPrograma.exportarArchivoInvertido(nombreNuevoArchivo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Carga un archivo XML con el contenido de un Archivo Invertido en el programa.
        /// </summary>
        /// <returns>
        /// True si el archivo pudo ser cargado. False en caso contrario.
        /// </returns>
        static private bool cargarArchivoInvertido()
        {
            string[] mensaje =
            {
                "--- Cargar archivo invertido ---",
                "Escriba el nombre del archivo XML con la información del archivo invertido.",
                "El archivo XML debe estar guardado en la siguiente carpeta: ",
                Opciones.Instance.RutaArchivos + "\n",
                "Nota: No colocar la extensión del archivo (.xml)\n"
            };

            foreach (string linea in mensaje)
            {
                Console.WriteLine(linea);
            }

            Console.Write("Nombre del archivo: ");
            string nombreArchivo = Console.ReadLine();

            try
            {
                Invertido archivoInvertidoCargado = Invertido.importarArchivoInvertido(nombreArchivo);
                archivoInvertidoPrograma = archivoInvertidoCargado;
                Console.WriteLine("El archivo invertido ha sido cargado.");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        /// <summary>
        /// Abre el menú principal del programa.
        /// </summary>
        static public void abrirMenuPrincipal()
        {
            String[] mensaje = { "--- Menú Principal ---\nPresione alguna de las siguientes teclas numéricas para escoger una opción:\n",
                "1 - Cargar Archivo Invertido",
                "2 - Generar Archivo Invertido por defecto",
                "0 - Salir\n" };

            bool verMensajeMenu = true;     // Controla cuándo se imprime la lista de opciones.
            while (verMensajeMenu)
            {
                foreach (string linea in mensaje)
                {
                    Console.WriteLine(linea);
                }

                bool solicitarOpcion = true;    // Controla cuándo se debe solicitar una opción.
                while (solicitarOpcion)
                {
                    bool resultado = false;
                    ConsoleKey teclaPresionada = Console.ReadKey(true).Key;

                    if ((teclaPresionada == ConsoleKey.D1) || (teclaPresionada == ConsoleKey.NumPad1))
                    {
                        resultado = cargarArchivoInvertido();
                        if (resultado)
                        {
                            abrirSubMenuPrincipal();
                        }
                        solicitarOpcion = false;
                    }
                    else if ((teclaPresionada == ConsoleKey.D2) || (teclaPresionada == ConsoleKey.NumPad2))
                    {
                        resultado = crearArchivoInvertidoDefault();
                        if (resultado)
                        {
                            abrirSubMenuPrincipal();
                        }
                        solicitarOpcion = false;
                    }
                    else if ((teclaPresionada == ConsoleKey.D0) || (teclaPresionada == ConsoleKey.NumPad0))
                    {
                        Console.WriteLine("Terminando programa...");
                        solicitarOpcion = false;
                        verMensajeMenu = false;
                    }
                }
            }
        }

        /// <summary>
        /// Muestra el sub-menú principal para realizar consultas sobre el archivo invertido.
        /// </summary>
        static private void abrirSubMenuPrincipal()
        {
            String[] mensaje = { "--- Menú de Consultas ---\nPresione alguna de las siguientes teclas numéricas para escoger una opción:\n",
                "1 - Realizar una consulta vectorial",
                "2 - Realizar una consulta estructurada",
                "0 - Cerrar archivo invertido y regresar al menú principal\n" };

            bool verMensajeMenu = true;     // Controla cuándo se imprime la lista de opciones.
            while (verMensajeMenu)
            {
                foreach (string linea in mensaje)
                {
                    Console.WriteLine(linea);
                }

                bool solicitarOpcion = true;    // Controla cuándo se debe solicitar una opción.
                while (solicitarOpcion)
                {
                    bool resultado = false;
                    ConsoleKey teclaPresionada = Console.ReadKey(true).Key;

                    if ((teclaPresionada == ConsoleKey.D1) || (teclaPresionada == ConsoleKey.NumPad1))
                    {
                        Console.WriteLine("Aquí comienza la consulta vectorial.\n");
                        solicitarOpcion = false;
                    }
                    else if ((teclaPresionada == ConsoleKey.D2) || (teclaPresionada == ConsoleKey.NumPad2))
                    {
                        Console.WriteLine("Aquí comienza la consulta estructurada.\n");
                        solicitarOpcion = false;
                    }
                    else if ((teclaPresionada == ConsoleKey.D0) || (teclaPresionada == ConsoleKey.NumPad0))
                    {
                        Console.WriteLine("Cerrando menú de consultas y archivo invertido...\n");
                        archivoInvertidoPrograma = null;
                        solicitarOpcion = false;
                        verMensajeMenu = false;
                    }
                }
            }
        }
    }
}
