using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ThirdParty.WebServices.Magaya.Business
{
    public class Serializer
    {
        //Convert xml to object
        public T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        //Convert object to xml
        public string Serialize<T>(T ObjectToSerialize)
        {

            //create an instance of an XmlSerializer
            //we will use the Type constructor for this example
            XmlSerializer serializer = new XmlSerializer(ObjectToSerialize.GetType());

            //specify our namespace
            //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            //create an XmlWriterSettings object to specify the
            //encoding and the indentation
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding();
            settings.Indent = true;

            //create an XmlWriter that utilizes a StringWriter to
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, ObjectToSerialize);
                    return stringWriter.ToString();
                }
            }
        }

        //Convert object to xml2
        public string ConvertObjectToXMLString(object ObjectToSerialize) 
        {

            //specify our namespace
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            //encoding and the indentation
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding();
            settings.Indent = true;

            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, ObjectToSerialize, namespaces);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }
    }

}
