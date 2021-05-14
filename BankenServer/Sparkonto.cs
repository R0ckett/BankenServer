using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BankenServer
{
    class Sparkonto : Konto, IFormatString
    {
        double ränta;
        
        public Sparkonto(double saldo, double ränta) : base(saldo)
        {
            this.ränta = ränta;
        }
        public Sparkonto(string sparkontoString)
        {
            string[] arr = sparkontoString.Split(';');
            ränta = int.Parse(arr[1]);         
            saldo = double.Parse(arr[3]);
        }
        public Sparkonto(XmlNode xmlSpar)
        {
            saldo = double.Parse(xmlSpar.SelectSingleNode("Balance").InnerText);
            ränta = long.Parse(xmlSpar.SelectSingleNode("Interest").InnerText);
            
        }
        public override string FormateraString()
        {
            return "Sparkonto (ränta): " + ";" + ränta + ";" + "(Saldo): " + ";" + base.FormateraString();
        }
        public override XmlElement FormatToXml(XmlDocument xmlDoc)
        {
            XmlElement account = xmlDoc.CreateElement("SavingsAccount");
            XmlElement saldo = xmlDoc.CreateElement("Balance");
            XmlElement interest = xmlDoc.CreateElement("Interest");
            interest.InnerText = ränta.ToString();
            saldo.InnerText = this.saldo.ToString();
            account.AppendChild(saldo);
            account.AppendChild(interest);

            return account;
        }

    }
}
