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
            _filepath = filepath;

            _serialiser = new XmlSerializer(typeof(T));
        }

        public T Deserialise()
        {
            TextReader reader = new StreamReader(_filepath);
            T result = (T)_serialiser.Deserialize(reader);
            reader.Close();

            return result;
        }

        public void Serialise(T data)
        {
            TextWriter writer = new StreamWriter(_filepath);
            _serialiser.Serialize(writer, data);
            writer.Close();
        }
    }
}
