using System.IO;
using System.Runtime.Serialization;

namespace SSA.Core.Extensions
{
    public static class DataContractExtensions
    {
        public static T DataContractSerialization<T>(T obj)
        {
            var dcSer = new DataContractSerializer(obj.GetType());
            var memoryStream = new MemoryStream();
            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;
            var newObject = (T) dcSer.ReadObject(memoryStream);
            return newObject;
        }
    }
}