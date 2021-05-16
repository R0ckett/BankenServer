using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BankenServer
{
    class ISKkonto : Konto, IFormatString, IXmlFormat
    {
        int AntalAktier;

        public ISKkonto(double saldo, int AntalAktier) : base(saldo)
        {
            this.AntalAktier = AntalAktier;
        }
        public ISKkonto(string iskkontoString)
        {
            string[] arr = iskkontoString.Split(';');
            AntalAktier = int.Parse(arr[1]);
            saldo = double.Parse(arr[3]);
        }
        public ISKkonto(XmlNode xmlSpar)
        {
            saldo = double.Parse(xmlSpar.SelectSingleNode("Balance").InnerText);
            AntalAktier = int.Parse(xmlSpar.SelectSingleNode("Stock").InnerText);

        }
        public override string FormateraString()
        {
            return "ISK(aktier) " + ";" + AntalAktier + ";" + "(Saldo): " + ";" + base.FormateraString();
        }
        public override XmlElement FormatToXml(XmlDocument xmlDoc)
        {
            XmlElement account = xmlDoc.CreateElement("ISKAccount");
            XmlElement saldo = xmlDoc.CreateElement("Balance");
            XmlElement stocks = xmlDoc.CreateElement("Stock");
            stocks.InnerText = AntalAktier.ToString();
            saldo.InnerText = this.saldo.ToString();
            account.AppendChild(saldo);
            account.AppendChild(stocks);

            return account;
        }
    }
}
