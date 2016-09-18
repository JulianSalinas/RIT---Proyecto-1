using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    public class Posting
    {
        public int DocId;
        public int Frecuencia;
        public double Peso;

        public Posting()
        {
            DocId = -1;
            Frecuencia = 0;
            Peso = 0;
        }

        public Posting(int docId, int frecuencia, double peso) {
            this.DocId = docId;
            this.Frecuencia = frecuencia;
            this.Peso = peso;
        }

        public Posting(byte[] bytesObjeto)
        {
            fromByteArray(bytesObjeto);
        }


        /*
        Convierte los atributos a cadenas de bytes para almacenarlos en un archivo.
        */
        public byte[] toByteArray()
        {
            byte[] bytesDocId = BitConverter.GetBytes(DocId);
            byte[] bytesFrecuencia = BitConverter.GetBytes(Frecuencia);
            byte[] bytesPeso = BitConverter.GetBytes(Peso);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesDocId);
                Array.Reverse(bytesFrecuencia);
                Array.Reverse(bytesPeso);
            }

            byte[] bytesObjeto = new byte[bytesDocId.Length + bytesFrecuencia.Length + bytesPeso.Length];
            System.Buffer.BlockCopy(bytesDocId, 0, bytesObjeto, 0, bytesDocId.Length);
            System.Buffer.BlockCopy(bytesFrecuencia, 0, bytesObjeto, bytesDocId.Length, bytesFrecuencia.Length);
            System.Buffer.BlockCopy(bytesPeso, 0, bytesObjeto, bytesDocId.Length + bytesFrecuencia.Length, bytesPeso.Length);

            return bytesObjeto;
        }

        /*
        Obtiene los datos para el objeto leyendo un array de bytes.
        El array debe tener 16 bytes: 4 para el docId, 4 para la frecuencia, 8 para el peso.
        */
        public void fromByteArray(byte[] byteArray)
        {
            if (byteArray.Length == 16)
            {
                byte[] bytesDocId = new byte[4];
                byte[] bytesFrecuencia = new byte[4];
                byte[] bytesPeso = new byte[8];

                Array.Copy(byteArray, 0, bytesDocId, 0, 4);
                Array.Copy(byteArray, 4, bytesFrecuencia, 0, 4);
                Array.Copy(byteArray, 8, bytesPeso, 0, 8);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytesDocId);
                    Array.Reverse(bytesFrecuencia);
                    Array.Reverse(bytesPeso);
                }

                this.DocId = BitConverter.ToInt32(bytesDocId, 0);
                this.Frecuencia = BitConverter.ToInt32(bytesFrecuencia, 0);
                this.Peso = BitConverter.ToDouble(bytesPeso, 0);
            }
        }

        public override string ToString() {
            return DocId + "\t\t\t" + Frecuencia.ToString() + "\t\t\t" + Peso.ToString() + "\n";
        }
    }
}
