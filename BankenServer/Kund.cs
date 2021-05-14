using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace BankenServer
{
    class Kund : IFormatString
    {
        string namn;
        long personNummer;
        List<Konto> konton = new List<Konto>();
        public Kund(string namn, long personNummer)
        {
            this.namn = namn;
            this.personNummer = personNummer;
        }
        public Kund(string namn, long personNummer, List<Konto> konton)
        {
            this.namn = namn;
            this.personNummer = personNummer;
            this.konton = konton;
        }
        public Kund(string kundRepresentationString)
        {
            string[] arr = kundRepresentationString.Split('#');
            namn = arr[1];
            personNummer = long.Parse(arr[3]);

            for (int i = 5; i < arr.Length; i++)
            {
                Konto k;
                string kontostring = arr[i];
                
                if (kontostring.Substring(0,9) == "Sparkonto" )
                {
                    k = new Sparkonto(kontostring);                   
                }
                else
                {
                    k = new ISKkonto(kontostring);
                }            
                konton.LäggTill(k);
            }
        }
        public Kund (XmlNode xmlKund)
        {           
            namn = xmlKund.SelectSingleNode("Name").InnerText;
            personNummer = long.Parse(xmlKund.SelectSingleNode("Id").InnerText);
            XmlNode konton = xmlKund.SelectSingleNode("Accounts");
            XmlNodeList xmlSavings = konton.SelectNodes("SavingsAccount");
            foreach (XmlNode x in xmlSavings)
            {
                this.konton.LäggTill(new Sparkonto(x));
            }
            XmlNodeList xmlISK = konton.SelectNodes("ISKAccount");
            foreach (XmlNode x in xmlISK)
            {
                this.konton.LäggTill(new ISKkonto(x));
            }
        }
        public List<Konto> Konton
        {
            get { return konton; }
        }
        public string Namn
        {
            get { return namn; }
        }
        public long PersonNummer
        {
            get { return personNummer; }
        }
        public void LäggTillKonto(Konto k)
        {
            konton.LäggTill(k);
        }
        public void TaBortKonto(int idx)
        {
            konton.TaBortVid(idx - 1);
        }
        public string PresenteraKonton()
        {
            string allaKonton = "";

            for (int i = 0; i < konton.Count; i++)
            {
                string num = (i + 1).ToString();
                allaKonton += "[" + num + "]: " + konton.FåVärde(i).Saldo + "\n";
            }
            return "Namn: " + namn + "\nPersonnummer: " + personNummer + "\nKonton:\n" + allaKonton;
        }
        public string FormateraString()
        {
            string returnString = "";
            returnString += "namn: " + "#" + namn.ToString() + "#" + "Personnummer: " + "#" + personNummer.ToString() + "#" + "Konton: ";

            for (int i = 0; i < konton.Count; i++)
            {
                returnString += "#" + konton.FåVärde(i).FormateraString();
            }
            return returnString;
        }
        public XmlElement FormatToXml(XmlDocument xmlDoc)
        {
            XmlElement customer = xmlDoc.CreateElement("Customer");

            XmlElement id = xmlDoc.CreateElement("Id");
            id.InnerText = this.personNummer.ToString();
            customer.AppendChild(id);

            XmlElement name = xmlDoc.CreateElement("Name");
            name.InnerText = this.namn;
            customer.AppendChild(name);

            XmlElement accounts = xmlDoc.CreateElement("Accounts");
            for (int i = 0; i < this.konton.Count; i++)
            {
                accounts.AppendChild(this.konton.FåVärde(i).FormatToXml(xmlDoc));
            }
            customer.AppendChild(accounts);

            return customer;
        }
        
    }

}

