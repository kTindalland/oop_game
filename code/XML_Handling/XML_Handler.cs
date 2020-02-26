using System.IO;
using System.Xml.Serialization;

namespace XML_Handling
{
    public class XML_Handler<T>
    {
        private string _filepath;
        private XmlSerializer _serialiser;

        public XML_Handler(string filepath)
        {
            // INitialise the fields
            _filepath = filepath;

            _serialiser = new XmlSerializer(typeof(T));
        }

        public T Deserialise()
        {
            // Open the reader, get the data, close the reader, then return the data
            TextReader reader = new StreamReader(_filepath);
            T result = (T)_serialiser.Deserialize(reader);
            reader.Close();

            return result;
        }

        public void Serialise(T data)
        {
            // Open the writer, write the data, then close the writer.
            TextWriter writer = new StreamWriter(_filepath);
            _serialiser.Serialize(writer, data);
            writer.Close();
        }
    }
}
