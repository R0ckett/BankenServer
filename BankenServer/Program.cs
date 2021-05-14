using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Net.Sockets;
using System.IO;

namespace BankenServer
{
    class Program
    {
        static TcpListener tcpListener;

        static void Main(string[] args)
        {

            List<Kund> KundList = LoadXml();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);
            IPAddress theIp = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(theIp, 8001);
            tcpListener.Start();

            while (true)
            {
                try
                {
                    Console.WriteLine("väntar på ansluting...");

                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("Ansluting accepterad från " + socket.RemoteEndPoint);

                    Byte[] bMessage = new Byte[256];
                    int messageSize = socket.Receive(bMessage);
                    string message = "";
                    for (int i = 0; i < messageSize; i++)
                    {
                        message += Convert.ToChar(bMessage[i]);
                    }

                    if (message == "RequestMessages")
                    {                       
                        SkickaMeddelande(socket, KundList);
                    }
                    else
                    {
                        KundList = SparaMeddelande(message, KundList);
                    }
                    socket.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }
        static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            tcpListener.Stop();
            Console.WriteLine("Servern stänger ner!");
        }
        public static List<Kund> SparaMeddelande(string messageString, List<Kund> KundList)
        {
            Console.WriteLine("Saving messages");

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlMessages = xmlDoc.CreateElement("messages");
            
            string[] KundStrings = messageString.Split('%');
            for (int i = 0; i < KundStrings.Length; i++)
            {
                Console.WriteLine(KundStrings[i]);
                KundList.LäggTill(new Kund(KundStrings[i]));
            }

            for (int i = 0; i < KundList.Count; i++)
            {
                xmlMessages.AppendChild(KundList.FåVärde(i).FormatToXml(xmlDoc));               
            }
            
            xmlDoc.AppendChild(xmlMessages);

            xmlDoc.Save("messages.xml");

            return KundList;
        }
        public static List<Kund> LoadXml()
        {
            List<Kund> KundList = new List<Kund>();
            XmlDocument xmlDoc = new XmlDocument();

            // OBS!! KÖR KODEN UTAN DET SOM STÅR UNDER (xmlDoc.Load("messages.xml");) OCH SKRIV NÅT I KLIENTPROGRAMMET FÖR ATT SKAPA EN XML FIL
            
            try
            {
                xmlDoc.Load("messages.xml");
            }
            catch
            {
               // throw new xmlException();
            }
            
            XmlNodeList xmlMessages = xmlDoc.SelectNodes("messages/Customer");

            foreach (XmlNode xmlKund in xmlMessages)
            {
                KundList.LäggTill(new Kund(xmlKund));
            }

            return KundList;
        }
        public static void SkickaMeddelande(Socket socket, List<Kund> KundList)
        {
            string message = "";
            for (int i = 0; i < KundList.Count; i++)
            {
                message += KundList.FåVärde(i).FormateraString();
                if (i != KundList.Count - 1)
                {
                    message += "%";
                }
            }
            Console.WriteLine("hej");
            Console.WriteLine(message);
            Byte[] bMessage = Encoding.ASCII.GetBytes(message);
            socket.Send(bMessage);
        }


    }
}
