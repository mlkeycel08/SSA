using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SSA.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string PropertyList(this object obj)
        {
            var props = obj.GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (var p in props)
            {
                sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
            }
            return sb.ToString();
        }

        public static string SerializeXML(this object theObject)
        {
            // Exceptions are handled by the caller

            using (var oStream = new MemoryStream())
            {
                var oSerializer = new XmlSerializer(theObject.GetType());

                oSerializer.Serialize(oStream, theObject);

                return Encoding.Default.GetString(oStream.ToArray());
            }
        }

        public static string GetReflectedPropertyValue(this object subject, string field)
        {
            object reflectedValue = subject.GetType().GetProperty(field).GetValue(subject, null);
            return reflectedValue?.ToString() ?? "";
        }
    }
}