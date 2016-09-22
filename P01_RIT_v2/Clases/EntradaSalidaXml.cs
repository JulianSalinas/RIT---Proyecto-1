using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    /// <summary>
    /// Clase para exportar e importar clases serializables de forma simplificada.
    /// </summary>
    public class EntradaSalidaXml
    {
        private EntradaSalidaXml() { }

        /// <summary>
        /// Exporta un objeto serializable a un archivo XML
        /// </summary>
        /// <typeparam name="T">Tipo del objeto serializable.</typeparam>
        /// <param name="objeto">
        /// Objeto serializable a exportar.
        /// </param>
        /// <param name="rutaArchivo">
        /// Ruta del archivo XML que será creado.
        /// Si no se ingresa un nombre el archivo será guardado utilizando la carpeta por defecto (...\\Archivos), el nombre del tipo del objeto, fecha y hora del sistema.
        /// </param>
        /// <param name="rutaAbsoluta">
        /// Parámetro opcional para indicar si se utiliza una ruta absoluta. True para usar una ruta absoluta, False para usar una ruta relativa.
        /// Si utiliza una ruta relativa implica que el archivo será guardado en la carpeta por defecto (...\\Archivos) y no se debe agregar la extensión (*.xml).
        /// Si utiliza una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        /// <returns>Ruta absoluta del archivo guardado.</returns>
        public static string exportarComoXml<T>(T objeto, string rutaArchivo = "", bool rutaAbsoluta = false)
        {
            // Regex para verificar que archivo tiene nombre válido.
            Regex caracteresInvalidos = new Regex("[" + Regex.Escape(Path.GetInvalidFileNameChars().ToString() + Path.GetInvalidPathChars().ToString()) + "]");

            if (!rutaAbsoluta)
            {
                if (rutaArchivo == "")
                {
                    rutaArchivo = typeof(T).Name;
                    rutaArchivo += DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
                }
                else if (caracteresInvalidos.IsMatch(rutaArchivo))
                {
                    throw new Exception("El nombre de archivo ingresado no es válido.");
                }
                rutaArchivo = Opciones.Instance.RutaArchivos + rutaArchivo + ".xml";
            }
            else
            {
                if (caracteresInvalidos.IsMatch(rutaArchivo))
                {
                    throw new Exception("El nombre del archivo ingresado no es válido.");
                }
            }

            try
            {
                XmlSerializer serializador = new XmlSerializer(typeof(T));
                FileStream archivoSalida = File.Create(rutaArchivo);
                serializador.Serialize(archivoSalida, objeto);
                rutaArchivo = archivoSalida.Name;
                archivoSalida.Close();

                return rutaArchivo;
            }
            catch (PathTooLongException e)
            {
                throw new Exception("La ruta del archivo tiene un nombre muy largo.");
            }
            catch (NotSupportedException e)
            {
                throw new Exception("No hay soporte para crear el archivo.");
            }
            catch (IOException e)
            {
                throw new Exception("Error al crear el archivo.");
            }
        }


        /// <summary>
        /// Importa el contenido de un archivo XML para generar un objeto serializable.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto serializable.</typeparam>
        /// <param name="rutaArchivo">
        /// Ruta del archivo que será abierto.
        /// </param>
        /// <param name="usarRutaAbsoluta">
        /// Indica si se utiliza una ruta absoluta (completa) de archivo. True para usar una ruta absoluta o false para usar una ruta relativa.
        /// Si utiliza una ruta relativa implica que el archivo ingresado está guardado en la carpeta por defecto (...\\Archivos) y no se debe agregar la extensión (*.xml).
        /// Si utiliza una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        /// <returns>
        /// Archivo invertido importado.
        /// </returns>
        public static T importarDesdeXml<T>(string rutaArchivo, bool rutaAbsoluta = false)
        {
            // Regex para verificar que archivo tiene nombre válido.
            Regex caracteresInvalidos =
                new Regex("[" + Regex.Escape(Path.GetInvalidFileNameChars().ToString() + Path.GetInvalidPathChars().ToString()) + "]");

            if (rutaArchivo == "")
            {
                throw new Exception("El nombre del archivo no puede estar vacío.");
            }
            else if (caracteresInvalidos.IsMatch(rutaArchivo))
            {
                throw new Exception("El nombre de archivo ingresado no es válido.");
            }


            if (!rutaAbsoluta)
            {
                rutaArchivo = Opciones.Instance.RutaArchivos + rutaArchivo;
            }

            try
            {
                XmlSerializer deserializador = new XmlSerializer(typeof(T));
                FileStream archivoEntrada = File.OpenRead(rutaArchivo);
                T objetoRecuperado = (T) deserializador.Deserialize(archivoEntrada);
                archivoEntrada.Close();

                return objetoRecuperado;
            }
            catch (FileNotFoundException e)
            {
                throw new Exception("El archivo XML no existe.");
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("El archivo XML ingresado no contiene la estructura necesaria para recuperar el dato solicitado.");
            }
            catch (IOException e)
            {
                throw new Exception("Error al leer el archivo.");
            }
        }
    }
}
