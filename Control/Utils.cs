using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HtmlAgilityPack;
using IDPParser.Model;

namespace IDPParser.Control
{
    public class Utils
    {
        public static void RetrieveClubs(string fileName)
        {
            const string baseUrl = "http://www.transfermarkt.de/test/startseite/verein/";
            const int maxRetryCount = 2;

            var clubList = new List<TMClub>();
            var retryCount = 0;
            for (var count = 1; count < 50000; count++)
            {
                Debug.WriteLine("Status : Count({0}), Club List Size({1})",count, clubList.Count);

                var currUrl = baseUrl + count;
                var htmlData = string.Empty;
                try
                {
                    htmlData = GetHtmlFromUrl(currUrl);
                }
                catch (WebException e)
                {
                    Debug.WriteLine("Club ({0}) threw a web exception {1}",count, e.Message);
                    if (((HttpWebResponse) e.Response).StatusCode != HttpStatusCode.NotFound && retryCount < maxRetryCount)
                    {
                        Debug.WriteLine("Club ({0}), Trying again", count);
                        count--;
                        retryCount++;
                    }
                    continue;
                }
                

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlData);

                if (htmlDoc.DocumentNode == null)
                {
                    continue;
                }
                var clubNameDivNode = htmlDoc.DocumentNode.SelectSingleNode("//div[" + GenerateClassSelectorString("spielername-profil") + "]");
                
                if(clubNameDivNode == null) continue;

                var clubName = clubNameDivNode.InnerText.Trim();

                var shortClubNameTdNode = htmlDoc.DocumentNode.SelectSingleNode("//tr[" + GenerateClassSelectorString("table-highlight") + "]/td[" + GenerateClassSelectorString("no-border-links hauptlink") + "]");
                if (shortClubNameTdNode == null)
                {
                    clubList.Add(new TMClub(count.ToString(), clubName, null));
                    continue;
                }

                var shortClubNameANode = shortClubNameTdNode.SelectSingleNode("a");
                var shortClubName = shortClubNameANode.InnerText.Trim();


                clubList.Add(new TMClub(count.ToString(), clubName, shortClubName));

                //Reset retry and for the next pages
                retryCount = 0;

            }
            SerializeObject(clubList, fileName);
        }

        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                var xmlDocument = new XmlDocument();
                var serializer = new XmlSerializer(serializableObject.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }

        public static string GetHtmlFromUrl(string urlAddress)
        {
            var request = (HttpWebRequest)WebRequest.Create(urlAddress);

            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
            var response = (HttpWebResponse)request.GetResponse();

            // Get the stream associated with the response.
            var receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);

            Console.WriteLine("Response stream received.");
            var source = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            return source;
        }


        public static string GenerateClassSelectorString(string className)
        {
            return "contains(concat(' ',normalize-space(@class),' '),' " + className + " ')";
        }
    }
}
