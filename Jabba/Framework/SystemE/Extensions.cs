using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EnderPi.SystemE
{
    public static class Extensions
    {
        /// <summary>
        /// Deep copies any object using serialization.  This is potentially slow, but conceptually very simple.
        /// A future update may use reflection instead.
        /// </summary>
        /// <remarks>
        /// Note that any class this is used for must have attributes correctly marked for serialization.
        /// </remarks>
        /// <typeparam name="T">Any object type that is serializable</typeparam>
        /// <param name="obj">This object, which must be serializable</param>
        /// <returns>A deep copy of the given object</returns>
        public static T DeepCopy<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
            }
        }
    }
}
