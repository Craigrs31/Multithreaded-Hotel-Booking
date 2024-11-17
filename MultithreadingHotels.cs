using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;
using System.IO;


namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://www.public.asu.edu/~crseward/Hotels.xml";
        public static string xmlErrorURL = "https://www.public.asu.edu/~crseward/HotelsErrors.xml";
        public static string xsdURL = "https://www.public.asu.edu/~crseward/Hotels.xsd";

        public static void Main(string[] args)
        {
            // Verify the valid XML file
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);  22

            // Verify the XML file with errors
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Convert the valid XML file to JSON
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1 Verification method to validate XML against XSD
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add("", xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(schemaSet);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }
                return "No Error";  // Return if no validation errors are found
            }
            catch (XmlException ex)
            {
                return $"XML Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // Callback for handling validation errors
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            throw new Exception($"Validation Error: {e.Message}");
        }

        // Q2.2 Xml2Json method to convert XML content to JSON format
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlUrl);

                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Error converting XML to JSON: {ex.Message}";
            }
        }
    }
}

