using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    /// <summary>
    /// Clase del Archivo Invertido.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "ArchivoInvertido")]
    public class Invertido
    {
        /// <summary>
        /// Colección de documentos.
        /// </summary>
        public List<Documento> Documentos;

        /// <summary>
        /// Términos registrados.
        /// </summary>
        public List<Termino> Diccionario;

        /// <summary>
        /// Listado de Postings del término registrado.
        /// </summary>
        public List<Posting> Postings;

        /// <summary>
        /// Ruta del archivo invertido en almacenamiento (si el archivo invertido fue exportado o importado).
        /// </summary>
        private string rutaArchivoInvertido; 

        [XmlIgnore]
        public string RutaArchivoInvertido
        {
            get { return rutaArchivoInvertido; }
            set { rutaArchivoInvertido = value; }
        }

        /// <summary>
        /// Instancia reservada para usar patrón Singleton.
        /// </summary>
        private static Invertido instance;

        [XmlIgnore]
        public static Invertido Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Invertido();
                    instance.indexarColeccion();
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// Constructor por defecto. Necesario para serializar y deserializar archivo invertido.
        /// </summary>
        public Invertido()
        {
            Diccionario = new List<Termino>();
            Postings = new List<Posting>();
            Documentos = new List<Documento>();
        }

        /// <summary>
        /// Genera los Postings del archivo invertido.
        /// </summary>
        private void indexarColeccion()
        {
            cargarDocumentos();
            crearDiccionario();

            int currentBegin = 0;
            foreach (Termino term in Diccionario)
            {
                int currentNi = 0;
                term.Ni = calcularNi(term);
                foreach (Documento doc in Documentos)
                {
                    if (doc.hasTermino(term.Contenido))
                    {
                        ++currentNi;
                        int freq = doc.obtenerCuentaTermino(term.Contenido);
                        double peso = calcularPeso(term, doc);
                        Postings.Add(new Posting(doc, freq, peso));
                    }
                }
                term.Inicio = currentBegin;
                currentBegin += currentNi;
            }

        }

        /// <summary>
        /// Calcula el valor Ni: Apariciones de un término en diferentes documentos.
        /// </summary>
        /// <param name="term">Término a consultar.</param>
        /// <returns>Cantidad de documentos donde aparece el término.</returns>
        private int calcularNi(Termino term)
        {
            int Ni = 0;
            foreach (Documento doc in Documentos)
                if (doc.hasTermino(term.Contenido))
                    ++Ni;
            return Ni;
        }

        /// <summary>
        /// Calcula el peso correspondiente para un término asociado a un documento.
        /// </summary>
        /// <param name="term">Término registrado.</param>
        /// <param name="doc">Documento registrado.</param>
        /// <returns>Peso calculado para el término en el documento.</returns>
        private double calcularPeso(Termino term, Documento doc)
        {
            int N = Documentos.Count;
            int frecuencia = doc.obtenerCuentaTermino(term.Contenido);
            int Ni = term.Ni;

            // Fórmula a utilizar según especificación del programa.
            return (1 + (Math.Log(frecuencia, 2) * Math.Log((N / Ni), 2)));
        }

        /// <summary>
        /// Lee la carpeta de la coleccion y carga el id y ruta de cada documento.
        /// </summary>
        private void cargarDocumentos()
        {
            int currentId = 0;
            foreach (string file in Directory.GetFiles(Opciones.Instance.RutaColeccion, "*.xml").ToList())
            {
                FileInfo fileInfo = new FileInfo(file);
                Documento doc = new Documento(currentId, fileInfo.FullName);
                Documentos.Add(doc);
                currentId++;
            }
        }

        /// <summary>
        /// Obtiene una sola copia de todas las palabras en la coleccion.
        /// </summary>
        private void crearDiccionario()
        {
            foreach (Documento doc in Documentos)
                foreach (string termino in doc.indizarDocumento())
                    if (!Diccionario.Exists(x => x.Contenido == termino))
                        Diccionario.Add(new Termino(termino, 0, 0));
        }

        /// <summary>
        /// Exporta el archivo invertido a un archivo XML.
        /// </summary>
        /// <param name="rutaArchivo">
        /// Ruta del archivo XML que será creado.
        /// Si no se ingresa un nombre se utiliza la fecha y hora del sistema y el archivo será guardado en la carpeta por defecto (...\\Archivos).
        /// </param>
        /// <param name="rutaAbsoluta">
        /// Parámetro opcional para indicar si se utiliza una ruta absoluta. True para usar una ruta absoluta, False para usar una ruta relativa.
        /// Si utiliza una ruta relativa implica que el archivo será guardado en la carpeta por defecto (...\\Archivos) y no se debe agregar la extensión (*.xml).
        /// Si utiliza una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        public void exportarArchivoInvertido(string rutaArchivo = "", bool rutaAbsoluta = false)
        {
            rutaArchivoInvertido = EntradaSalidaXml.exportarComoXml(this, rutaArchivo, rutaAbsoluta);
        }

        /// <summary>
        /// Importa el contenido de un archivo XML para generar un archivo invertido.
        /// </summary>
        /// <param name="rutaArchivo">
        /// Ruta del archivo que será abierto.
        /// </param>
        /// <param name="rutaAbsoluta">
        /// Indica si se utiliza una ruta absoluta (completa) de archivo. True para usar una ruta absoluta o false para usar una ruta relativa.
        /// Si utiliza una ruta relativa implica que el archivo ingresado está guardado en la carpeta por defecto (...\\Archivos) y no se debe agregar la extensión (*.xml).
        /// Si utiliza una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        /// <returns>
        /// Archivo invertido importado.
        /// </returns>
        public static Invertido importarArchivoInvertido(string rutaArchivo, bool rutaAbsoluta = false)
        {
            Invertido archivoInvertidoImportado = EntradaSalidaXml.importarDesdeXml<Invertido>(rutaArchivo, rutaAbsoluta);
            if (!rutaAbsoluta)
            {
                rutaArchivo = Opciones.Instance.RutaArchivos + rutaArchivo;
            }
            archivoInvertidoImportado.RutaArchivoInvertido = rutaArchivo;
            return archivoInvertidoImportado;
        }

        /// <summary>
        /// Busca la entrada del término en el Archivo Invertido.
        /// </summary>
        /// <param name="terminoBusqueda">
        /// Término a buscar.
        /// </param>
        /// <returns>
        /// Objeto Termino encontrado. Null si el término no existe.
        /// </returns>
        public Termino obtenerTerminoExacto(string terminoBusqueda)
        {
            foreach (Termino terminoEncontrado in Diccionario)
            {
                if (terminoEncontrado.Contenido.Equals(terminoBusqueda))
                {
                    return terminoEncontrado;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene una lista con los términos que comparten un prefijo.
        /// </summary>
        /// <param name="prefijo">
        /// Prefijo de los términos buscados.
        /// </param>
        /// <returns>
        /// Lista con objetos Término encontrados. Si no se encontró ningún término, la lista estará vacía.
        /// </returns>
        public List<Termino> obtenerTerminosConPrefijo(string prefijo)
        {
            if (prefijo.Equals("") || prefijo == null)
            {
                return null;
            }
            else
            {
                string strRegex = @"^(" + prefijo + ")";
                Regex regex = new Regex(strRegex, RegexOptions.Compiled);

                List<Termino> listaTerminos = new List<Termino>();

                foreach (Termino terminoEncontrado in Diccionario)
                {
                    if (regex.IsMatch(terminoEncontrado.Contenido))
                    {
                        listaTerminos.Add(terminoEncontrado);
                    }
                }
                return listaTerminos;
            }
        }

        /// <summary>
        /// Obtiene la lista de Postings asociados al registro del término.
        /// </summary>
        /// <param name="entradaTermino">
        /// Objeto Término registrado.
        /// </param>
        /// <returns>
        /// Lista de Postings del término.
        /// </returns>
        List<Posting> obtenerPostingsTerminoExacto(Termino entradaTermino)
        {
            if (entradaTermino == null)
            {
                return null;
            }
            else
            {
                List<Posting> postingsTermino = new List<Posting>();

                for (int posicionBusqueda = entradaTermino.Inicio;
                        posicionBusqueda < entradaTermino.Inicio + entradaTermino.Ni;
                        posicionBusqueda++)
                {
                    postingsTermino.Add(Postings.ElementAt(posicionBusqueda));
                }
                return postingsTermino;
            }
        }

        /// <summary>
        /// Obtiene los Postings de un término buscado en el Archivo Invertido.
        /// </summary>
        /// <param name="terminoBusqueda">
        /// Término a buscar dentro del Archivo Invertido.
        /// </param>
        /// <returns>
        /// Lista de Postings del término. Si el término no pudo ser encontrado, se retorna Null.
        /// </returns>
        public List<Posting> obtenerPostingsTerminoExacto(string terminoBusqueda)
        {
            if ((terminoBusqueda == null) || (terminoBusqueda.Equals("")))
            {
                return null;
            }

            Termino terminoEncontrado = obtenerTerminoExacto(terminoBusqueda);
            return obtenerPostingsTerminoExacto(terminoEncontrado);
        }


        /// <summary>
        /// Obtiene todos los postings asociados a un documento.
        /// </summary>
        /// <param name="idDocumento">Identificador del documento a consultar.</param>
        /// <returns>Lista de postings del documento. Si el documento no tiene postings asociados, se retorna una lista vacía.</returns>
        public List<Posting> obtenerPostingsDocumento(int idDocumento)
        {
            List<Posting> postingsDocumento = new List<Posting>();

            foreach(Posting posting in Postings)
            {
                if (posting.DocId == idDocumento)
                {
                    postingsDocumento.Add(posting);
                }
            }
            return postingsDocumento;
        }

        /// <summary>
        /// Exporta el Archivo Invertido a un String. Utilizar para probar si el Archivo Invertido fue creado correctamente.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "Diccionario: \n";
            str += "Inicio\t\t\tNi\t\t\tPalabra\n";

            foreach (Termino term in Diccionario)
                str += term.ToString();

            str += "\nPostings: \n";
            str += "Identif\t\t\tFreq\t\t\tPeso\n";

            foreach (Posting post in Postings)
                str += post.ToString();

            return str;
        }
    }
}
