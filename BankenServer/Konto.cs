using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BankenServer
{
    class Konto : IFormatString, IXmlFormat
    {
        protected double saldo;

        public Konto()
        {
            this.saldo = 0;
        }
        public Konto(double saldo)
        {
            this.saldo = saldo;
        }
        public Konto(string kontoRepresentationString)
        {
            saldo = double.Parse(kontoRepresentationString);
        }
        public double Saldo
        {
            set { saldo = value; }
            get { return saldo; }
        }
        public void SkrivUtSaldo()
        {
            Console.WriteLine("Din nuvarande saldo är: " + saldo);
        }

        public virtual string FormateraString()
        {
            string returnString = "";
            returnString += saldo.ToString();
            return returnString;
        }
        public virtual XmlElement FormatToXml(XmlDocument xmlDoc)
        {
            XmlElement account = xmlDoc.CreateElement("Account");

            XmlElement saldo = xmlDoc.CreateElement("Balance");
            saldo.InnerText = this.saldo.ToString();
            account.AppendChild(saldo);

            return account;
        }
    }
}
