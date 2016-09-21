using MetroFramework.Forms;
using P01_RIT_v2.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static P01_RIT_v2.Clases.Vectorial;

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
            Invertido.exportarArchivoInvertido(Opciones.Instance.Prefijo + textBoxNombreInvertido.Text);
            //Console.Write(Invertido.Instance.ToString());
            MessageBox.Show("Indexación finalizada");
        }

        private void buttonColeccion_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoge la carpeda donde esta almacenada la colección";
            folderBrowserDialog.ShowDialog();
            textBoxColeccion.Text = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.RutaColeccion = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.guardarOpciones();
        }

        private void buttonInvertido_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoge la carpeda donde almacenará el archivo invertido";
            folderBrowserDialog.ShowDialog();
            textBoxInvertido.Text = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.RutaArchivos = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.guardarOpciones();
        }

        private void buttonInvertidoConsultas_Click( object sender, EventArgs e ) {
            folderBrowserDialog.Description = "Escoge la carpeda donde almacenarán los resultados de las consultas";
            folderBrowserDialog.ShowDialog();
            textBoxInvertidoConsultas.Text = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.RutaConsultas = folderBrowserDialog.SelectedPath + "\\";
            Opciones.Instance.guardarOpciones();
        }

        private void buttonRutaArchivoInvertido_Click( object sender, EventArgs e ) {
            try {
                if ( openFileDialog.FileName != "" || openFileDialog.FileName != null ) {
                    openFileDialog.Title = "Escoge la carpeda donde almacenarán los resultados de las consultas";
                    openFileDialog.ShowDialog();
                    textBoxRutaArchivoInvertido.Text = openFileDialog.FileName;
                    Invertido.importarArchivoInvertido(openFileDialog.SafeFileName);
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonConsultaVectorial_Click( object sender, EventArgs e ) {
            Vectorial consulta = new Vectorial(textBoxInvertidoConsultas.Text);
            List<TerminoConsulta> terminosConsulta = consulta.procesarTerminosDesdeConsulta();

            /*Comprobar terminos de la consulta*/
            foreach ( TerminoConsulta term in terminosConsulta )
                Console.Write("Peso: " + term.Peso + "\tTermino: " + term.Contenido + "\n");
        }

        private void textBoxPrefijo_TextChanged( object sender, EventArgs e ) {
            Opciones.Instance.Prefijo = textBoxPrefijo.Text;
        }

        /*Este es del boton de consultas estructuradas*/
        private void metroTextButton1_Click( object sender, EventArgs e ) {

        }

    }
}
