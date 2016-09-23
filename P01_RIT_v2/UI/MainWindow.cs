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
            MessageBox.Show("Espere mientras se indexa la coleccion");

            // Instance (Singleton) exporta su contenido.
            Invertido.Instance.exportarArchivoInvertido(Opciones.Instance.Prefijo + textBoxNombreInvertido.Text);

            
            //Console.Write(Invertido.Instance.ToString());
            MessageBox.Show("Indexación finalizada");
        }

        private void buttonColeccion_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoga la carpeta donde está almacenada la colección";
            folderBrowserDialog.ShowDialog();
            textBoxColeccion.Text = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.RutaColeccion = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.guardarOpciones();
        }

        private void buttonInvertido_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoge la carpeta donde almacenará el archivo invertido";
            folderBrowserDialog.ShowDialog();
            textBoxInvertido.Text = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.RutaArchivos = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.guardarOpciones();
        }

        private void buttonInvertidoConsultas_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoge la carpeta donde almacenarán los resultados de las consultas";
            folderBrowserDialog.ShowDialog();
            textBoxInvertidoConsultas.Text = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.RutaConsultas = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.guardarOpciones();
        }

        private void buttonRutaArchivoInvertido_Click( object sender, EventArgs e ) {
            try {
                if ( openFileDialog.FileName != "" || openFileDialog.FileName != null ) {
                    openFileDialog.Title = "Escoga el archivo XML con los detalles del archivo invertido";
                    openFileDialog.ShowDialog();
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
                    throw new Exception("No puede realizar una consulta sin términos.");
                }
                else
                {
                    BusquedaVectorial nuevaBusqueda = new BusquedaVectorial(Invertido.Instance, Opciones.Instance.RutaColeccion, textBoxConsultaVectorial.Text);
                    string rutaAchivoXmlGenerado = Opciones.Instance.RutaConsultas + Opciones.Instance.Prefijo +
                        " Busqueda Vect " + nuevaBusqueda.FechaHoraBusqueda.ToString("dd-MM-yyyy hh-mm-ss tt") + ".xml";
                    nuevaBusqueda.exportarComoXml(rutaAchivoXmlGenerado, true);
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
                    throw new Exception("No puede realizar una consulta sin términos.");
                }
                else
                {
                    BusquedaVectorial busquedaVectorial = null;
                    // Solicita el archivo de consulta vectorial que desea utilizar.
                    try
                    {
                        if (openFileDialog.FileName != "" || openFileDialog.FileName != null)
                        {
                            openFileDialog.Title = "Escoga el archivo XML con los detalles de la consulta vectorial";
                            openFileDialog.ShowDialog();

                            busquedaVectorial = BusquedaVectorial.importarDesdeXml(openFileDialog.FileName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    BusquedaEstructurada nuevaBusqueda = new BusquedaEstructurada(busquedaVectorial, textBoxConsultaEstruct.Text);


                    string rutaAchivoXmlGenerado = Opciones.Instance.RutaConsultas + Opciones.Instance.Prefijo +
                        " Busqueda Estruct " + nuevaBusqueda.FechaHoraBusquedaEstructurada.ToString("dd-MM-yyyy hh-mm-ss tt") + ".xml";
                    nuevaBusqueda.exportarComoXml(rutaAchivoXmlGenerado, true);
                }
            }
            catch (Exception ex)
            {
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
    }
}
