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
        private int docId;
        public int DocId
        {
            get { return docId; }
            set { docId = value; }
        }

        private int frecuencia;
        public int Frecuencia
        {
            get { return frecuencia; }
            set { frecuencia = value; }
        }


        private double peso;
        public double Peso
        {
            get { return peso; }
            set { peso = value; }
        }


        public Posting()
        {
            DocId = -1;
            Frecuencia = 0;
            Peso = 0;
        }

        public Posting(Documento documento, int frecuencia, double peso) {
            docId = documento.Id;
            this.frecuencia = frecuencia;
            this.peso = peso;
        }

        public override string ToString() {
            return DocId + "\t\t\t" + Frecuencia.ToString() + "\t\t\t" + Peso.ToString() + "\n";
        }
    }
}
