using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Helpers
{
    public static class Xml
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
                return string.Empty;

            try
            {
                using (var stringWriter = new StringWriter())
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    new XmlSerializer(typeof(T)).Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}