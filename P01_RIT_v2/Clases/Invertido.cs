using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    [XmlRoot(ElementName = "ArchivoInvertido")]
    public class Invertido
    {
        public List<Documento> Documentos;
        public List<Termino> Diccionario;
        public List<Posting> Postings;

        private static Invertido instance;

        public static Invertido Instance {
            get {
                if (instance == null)
                    instance = new Invertido();
                return instance;
            }
        }

        public Invertido() {
            Diccionario = new List<Termino>();
            Postings = new List<Posting>();
            Documentos = new List<Documento>();
        }

        /*Despues de indexar se debe guardar en archivos xml los posting y diccionario*/

        public void indexarColeccion() {
            cargarDocumentos();
            crearDiccionario();

            int currentBegin = 0;
            foreach (Termino term in Diccionario) {
                int currentNi = 0;
                term.Ni = calcularNi(term);
                foreach (Documento doc in Documentos) {
                    if (doc.hasTermino(term.Contenido)) {
                        currentNi++;
                        int freq = doc.countTermino(term.Contenido);
                        double peso = calcularPeso(term, doc);
                        Postings.Add(new Posting(doc.Id, freq, peso));
                    }
                }
                term.Inicio = currentBegin;
                currentBegin = currentBegin + currentNi;
            }

        }

        /*Numero de veces que aparace un termino en la coleccion*/

        public int calcularNi(Termino term) {
            int Ni = 0;
            foreach (Documento doc in Documentos)
                if (doc.hasTermino(term.Contenido))
                    Ni++;
            return Ni;
        }

        /*Formula ( 1 + Math.Log(freq) ) * Math.Log((N+0.5)/(ni-0.5))*/

        public double calcularPeso(Termino term, Documento doc) {
            int N = Documentos.Count;
            int Freq = doc.countTermino(term.Contenido);
            int Ni = term.Ni;
            return (1 + Math.Log10(Freq)) * Math.Log10((N + 0.5) / (Ni - 0.5));
        }

        /*Lee la carpeta de la coleccion y carga el id y ruta de cada documento*/

        public void cargarDocumentos() {
            int currentId = 0;
            foreach (string file in Directory.GetFiles(Opciones.Instance.RutaColeccion, "*.xml").ToList()) {
                FileInfo fileInfo = new FileInfo(file);
                Documento doc = new Documento(currentId, fileInfo.FullName);
                Documentos.Add(doc);
                currentId++;
            }
        }

        /*Obtiene una sola copia de todas las palabras en la coleccion*/

        public void crearDiccionario() {
            foreach (Documento doc in Documentos)
                foreach (string termino in doc.getTerminos())
                    if (!Diccionario.Exists(x => x.Contenido == termino))
                        Diccionario.Add(new Termino(termino, 0, 0));
        }

        /* Exporta el archivo invertida en un archivo XML
         * nombreArchivoPostings : Nombre del archivo XML a ser creado.
         */
        public void exportarArchivoInvertido(String nombreArchivoPostings)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializador = new System.Xml.Serialization.XmlSerializer(typeof(Invertido));
                System.IO.FileStream archivoSalida = System.IO.File.Create(Opciones.Instance.RutaArchivos + nombreArchivoPostings);
                serializador.Serialize(archivoSalida, this);
                archivoSalida.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("El archivo XML no existe.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al escribir el archivo.");
            }
        }

        /*
         * Importa el contenido del archivo XML que contiene los detalles de un archivo invertido.
         * nombreArchivoPostings : Nombre del archivo XML que será carga
         */
        public static Invertido importarArchivoInvertido(String nombreArchivoPostings)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer deserializador = new System.Xml.Serialization.XmlSerializer(typeof(Invertido));
                System.IO.FileStream archivoEntrada = System.IO.File.OpenRead(Opciones.Instance.RutaArchivos + nombreArchivoPostings);
                Invertido invertidoAbierto = (Invertido)deserializador.Deserialize(archivoEntrada);
                archivoEntrada.Close();
                return invertidoAbierto;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("El archivo XML no existe.");
                return null;
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al leer el archivo.");
                return null;
            }
        }

        /*Para comprobar que se ha creado bien*/
        public override string ToString() {
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


        /*
         *  Los siguientes algoritmos sirven para hacer búsquedas booleanas en el Archivo Invertido.
         *  Son para obtener los postings para los términos consultados.
         *  Los postings obtenidos luego serán utilizados para algoritmos de consulta.
         * /

        /*
         * Busca la entrada del término en el archivo invertido.
         * Recibe un string como término de búsqueda. 
         */
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

        /*
         * Obtiene una lista con todas las entradas de términos que tienen un mismo prefijo.
         * Prefijo no debe tener asterisco al final (*)
         */
        public List<Termino> obtenerTerminosConPrefijo(string prefijo)
        {
            if (prefijo.Equals("") || prefijo == null)
            {
                return null;
            }
            else
            {
                String strRegex = "^(" + prefijo + ")";
                Regex regex = new Regex(strRegex, RegexOptions.Compiled);

                List<Termino> listaTerminos = new List<Termino>();
                
                foreach(Termino terminoEncontrado in Diccionario)
                {
                    if (regex.IsMatch(terminoEncontrado.Contenido))
                    {
                        listaTerminos.Add(terminoEncontrado);
                    }
                }

                return listaTerminos;
            }
        }


        /*
         * Obtiene la lista de postings asociados a una entrade de término del archivo invertido.
         * Si la entrada del término es nula, se retorna NULL.
         */
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

        /*
         * Obtiene las listas con los postings para todos los términos que tienen un mismo prefijo.
         * Si la lista del términos encontrados es nula, se retorna NULL.
         */
        List<List<Posting>> obtenerPostingsTerminosConPrefijo(List<Termino> terminosEncontrados)
        {
            if (terminosEncontrados == null)
            {
                return null;
            }
            else
            {
                List<List<Posting>> postingsTerminos = new List<List<Posting>>();

                foreach (Termino terminoEncontrado in terminosEncontrados)
                {
                    List<Posting> postingsTermino = obtenerPostingsTerminoExacto(terminoEncontrado);
                    postingsTerminos.Add(postingsTermino);
                }

                return postingsTerminos;
            }
        }


        /*
         * Interfaz pública de obtenerPostingsTerminoExacto.
         */
        public List<Posting> obtenerPostingsTerminoExacto(string terminoBusqueda)
        {
            if ((terminoBusqueda == null) || (terminoBusqueda.Equals(""))) {
                return null;
            }

            Termino terminoEncontrado = obtenerTerminoExacto(terminoBusqueda);
            return obtenerPostingsTerminoExacto(terminoEncontrado);
        }

      
        /*
         * Interfaz pública de obtenerPostingsConPrefijo.
         */
        public List<List<Posting>> obtenerPostingsTerminosConPrefijo(string prefijo)
        {
            if ((prefijo == null) || (prefijo.Equals("")))
            {
                return null;
            }

            List<Termino> terminosEncontrados = obtenerTerminosConPrefijo(prefijo);
            return obtenerPostingsTerminosConPrefijo(terminosEncontrados);

            
        }
    }
}
