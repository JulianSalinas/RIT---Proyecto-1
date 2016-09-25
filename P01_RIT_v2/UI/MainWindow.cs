using MetroFramework.Forms;
using P01_RIT_v2.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace P01_RIT_v2.UI
{
    public partial class MainWindow : MetroForm
    {
        public MainWindow() {
            InitializeComponent();
            textBoxPrefijo.Text = Opciones.Instance.Prefijo;
            textBoxColeccion.Text = Opciones.Instance.RutaColeccion;
            textBoxInvertido.Text = Opciones.Instance.RutaArchivos;
            textBoxInvertidoConsultas.Text = Opciones.Instance.RutaConsultas;
        }

        private void buttonIndexar_Click( object sender, EventArgs e ) {
            MessageBox.Show("Por favor espere mientras se indiza la colección y se genera el archivo invertido.");

            // Instance (Singleton) exporta su contenido.
            Invertido.Instance.exportarArchivoInvertido(Opciones.Instance.Prefijo + textBoxNombreInvertido.Text);


            //Console.Write(Invertido.Instance.ToString());
            MessageBox.Show("El archivo invertido ha sido creado.");
        }

        private void buttonColeccion_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoga la carpeta que contiene la colección de documentos:";
            folderBrowserDialog.ShowDialog();
            
            if (!folderBrowserDialog.SelectedPath.Equals(""))
            {
                textBoxColeccion.Text = folderBrowserDialog.SelectedPath + "\\";
                Opciones.Instance.RutaColeccion = folderBrowserDialog.SelectedPath + "\\";
                Opciones.Instance.guardarOpciones();
            }
        }

        private void buttonInvertido_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoga la carpeta donde se almacenará el archivo invertido generado:";
            folderBrowserDialog.ShowDialog();
            
            if (!folderBrowserDialog.SelectedPath.Equals(""))
            {
                textBoxInvertido.Text = folderBrowserDialog.SelectedPath + "\\";
                Opciones.Instance.RutaArchivos = folderBrowserDialog.SelectedPath + "\\";
                Opciones.Instance.guardarOpciones();
            }
        }

        private void buttonInvertidoConsultas_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoga la carpeta donde se almacenarán los resultados de las consultas:";
            folderBrowserDialog.ShowDialog();
            
            if (!folderBrowserDialog.SelectedPath.Equals(""))
            {
                textBoxInvertidoConsultas.Text = folderBrowserDialog.SelectedPath + "\\";
                Opciones.Instance.RutaConsultas = folderBrowserDialog.SelectedPath + "\\";
                Opciones.Instance.guardarOpciones();
            }

        }

        private void buttonRutaArchivoInvertido_Click( object sender, EventArgs e ) {
            try {
                openFileDialog.Title = "Escoga el archivo XML con los detalles del archivo invertido";
                openFileDialog.Filter = "XML File|*.xml";
                openFileDialog.FileName = "";
                openFileDialog.ShowDialog();

                if (!openFileDialog.FileName.Equals(""))
                {
                    textBoxRutaArchivoInvertido.Text = openFileDialog.FileName;
                    Invertido.Instance = Invertido.importarArchivoInvertido(openFileDialog.SafeFileName);
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonConsultaVectorial_Click(object sender, EventArgs e) {
            try
            {
                if (textBoxConsultaVectorial.Equals(""))
                {
                    throw new Exception("No puede realizar una consulta sin especificar términos de búsqueda.");
                }
                if (textBoxRutaArchivoInvertido.Equals(""))
                {
                    throw new Exception("Necesita cargar un archivo invertido para realizar una consulta vectorial.");
                }
                else
                {
                    BusquedaVectorial nuevaBusqueda = new BusquedaVectorial(Invertido.Instance, Opciones.Instance.RutaColeccion, textBoxConsultaVectorial.Text);

                    string rutaArchivosGenerados = Opciones.Instance.RutaConsultas + Opciones.Instance.Prefijo +
                        " Busqueda Vectorial " + nuevaBusqueda.FechaHoraBusquedaVectorial.ToString("dd-MM-yyyy HH-mm-ss");

                    nuevaBusqueda.exportarComoXml(rutaArchivosGenerados + ".xml", true);
                    nuevaBusqueda.generarHTML(rutaArchivosGenerados + ".html", true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*Este es del boton de consultas estructuradas*/
        private void buttonConsultaEstruct_Click( object sender, EventArgs e ) {
            try
            {
                if (textBoxConsultaEstruct.Equals(""))
                {
                    throw new Exception("No puede realizar una consulta estructurada sin especificar cláusulas de consulta.");
                }
                else
                {
                    BusquedaVectorial busquedaVectorial = null;

                    // Solicita el archivo de consulta vectorial que desea utilizar.
                    openFileDialog.Title = "Escoga el archivo XML de la consulta vectorial que utilizará";
                    openFileDialog.Filter = "XML File|*.xml";
                    openFileDialog.ShowDialog();

                    if (openFileDialog.FileName.Equals(""))
                    {
                        throw new Exception("No puede realizar una consulta estructurada sin abrir un archivo de consulta vectorial.");
                    }

                    busquedaVectorial = BusquedaVectorial.importarDesdeXml(openFileDialog.FileName, true);

                    BusquedaEstructurada nuevaBusqueda = new BusquedaEstructurada(busquedaVectorial, textBoxConsultaEstruct.Text);
                    string rutaArchivosGenerados = Opciones.Instance.RutaConsultas + Opciones.Instance.Prefijo +
                        " Busqueda Estructurada " + nuevaBusqueda.FechaHoraBusquedaEstructurada.ToString("dd-MM-yyyy HH-mm-ss");

                    nuevaBusqueda.exportarComoXml(rutaArchivosGenerados + ".xml", true);
                    nuevaBusqueda.generarHTML(rutaArchivosGenerados + ".html", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPrefijo_TextChanged( object sender, EventArgs e ) {
            Opciones.Instance.Prefijo = textBoxPrefijo.Text;
        }

        private void metroTextButton1_Click( object sender, EventArgs e ) {
            try { 
                openFileDialog.Title = "Escoga el HTML para abrir";
                openFileDialog.Filter = "HTML File|*.html";
                openFileDialog.ShowDialog();

                if ( !openFileDialog.FileName.Equals("") ) {
                    textboxhtml.Text = openFileDialog.FileName;
                    webBrowser1.Navigate(openFileDialog.FileName);
                }

            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroButton1_Click( object sender, EventArgs e ) {
            webBrowser1.GoBack();
        }
    }
}
