using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FDEV.Rules.Demo.Core.Serialization
{
    #region DataContractSeralizable

    /// <summary>
    /// Xml Serializer to be implemented by inheritance, making the serialization methods available directly on the derived type
    /// </summary>
    public class DataContractSerializable<T> where T : class
    {
        public static T LoadXml(string filename, bool emitUTF8Identifier = false)
        {
            if (!File.Exists(filename)) return null;

            using var stream = new StreamReader(filename);
            var xml = stream.ReadToEnd();
            try
            {
                return FromXml(xml, emitUTF8Identifier);
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        public static T FromXml(string xml, bool emitUTF8Identifier = false)
        {
            using var memoryStream = new MemoryStream(new UTF8Encoding(emitUTF8Identifier).GetBytes(xml));
            var reader = XmlDictionaryReader.CreateTextReader(memoryStream, new UTF8Encoding(emitUTF8Identifier), new XmlDictionaryReaderQuotas(), null);
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            var result = serializer.ReadObject(reader);
            return (T)result;
        }

        public void SaveXml(string filename, bool emitUTF8Identifier = false)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir == null) return;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var xmlString = AsXml(emitUTF8Identifier);
            File.WriteAllText(filename, xmlString, new UTF8Encoding(emitUTF8Identifier));
        }

        public string AsXml(bool emitUtf8Identifier = false)
        {
            using var memoryStream = new MemoryStream();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            serializer.WriteObject(memoryStream, this);

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(memoryStream, new UTF8Encoding(emitUtf8Identifier));
            return streamReader.ReadToEnd();
        }

        public T Copy(bool emitUTF8Identifier = false) => FromXml(AsXml(emitUTF8Identifier), emitUTF8Identifier);}

    /// <summary>
    /// Xml Serializer to be used in a static context
    /// </summary>
    public static class DataContractSerializer
    {
        /// <summary>
        /// Load xml file into a type
        /// </summary>
        public static T LoadXmlFile<T>(string filename, bool emitUTF8Identifier = false)
        {
            if (!File.Exists(filename)) return default;

            using var stream = new StreamReader(filename);
            var xml = stream.ReadToEnd();
            try
            {
                return LoadXmlString<T>(xml, emitUTF8Identifier);
            }
            catch (SerializationException)
            {
                //MessageBox.Show(@"Message: " + exception.Message, @"Error loading configuration from XML");
                return default;
            }
        }

        public static T LoadXmlString<T>(string xml, bool emitUTF8Identifier = false) 
        {
            using var memoryStream = new MemoryStream(new UTF8Encoding(emitUTF8Identifier).GetBytes(xml));
            var reader = XmlDictionaryReader.CreateTextReader(memoryStream, new UTF8Encoding(emitUTF8Identifier), new XmlDictionaryReaderQuotas(), null);
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            var result = serializer.ReadObject(reader);
            return (T)result;
        }

        public static void SaveXmlFile<T>(object element, string filename, bool emitUTF8Identifier = false) 
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir == null) return;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var xmlString = ToXmlString<T>(element, emitUTF8Identifier);
            File.WriteAllText(filename, xmlString, new UTF8Encoding(emitUTF8Identifier));
        }

        public static string ToXmlString<T>(object element, bool emitUTF8Identifier = false)
        {
            using var memoryStream = new MemoryStream();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            serializer.WriteObject(memoryStream, element);
            memoryStream.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(memoryStream, new UTF8Encoding(emitUTF8Identifier));
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Serialize any object
        /// </summary>
        public static XElement AnyObjectToXml(this object sourceObject)
        {
            var sourceType = sourceObject.GetType();
            var extraTypes = sourceType.GetProperties().Where(p => p.PropertyType.IsInterface).Select(p => p.GetValue(sourceObject, null)?.GetType()).ToArray();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(sourceType, extraTypes);
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            serializer.WriteObject(xmlWriter, sourceObject);
            return XElement.Parse(stringWriter.ToString());
        }

        public static T Copy<T>(T element, bool emitUTF8Identifier = false) => LoadXmlString<T>(ToXmlString<T>(element, emitUTF8Identifier), emitUTF8Identifier);
    }

    /// <summary>
    /// Xml Serializer to facilitate on the fly conversion in and out of XML. Open for extensions, so we can use this class only, securing consistency and much less code.
    /// </summary>
    [DataContract(Namespace = "")]
    public class XmlSerializable<T> where T : class
    {
        public static T LoadXml(string filename)
        {
            if (!File.Exists(filename)) return null;

            using var streamReader = new StreamReader(filename);
            var xml = streamReader.ReadToEnd();
            try
            {
                return FromXml(xml);
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        public static T FromXml(string xmlString)
        {
            using var reader = XmlReader.Create(new StringReader(xmlString));
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(reader);
        }

        public void SaveXml(string filename)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir == null) return;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var xmlString = AsXml();
            File.WriteAllText(filename, xmlString, Encoding.UTF8);
        }

        public string AsXml()
        {
            using var memStm = new MemoryStream();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            serializer.WriteObject(memStm, this);

            memStm.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(memStm, Encoding.UTF8);
            return streamReader.ReadToEnd();
        }

        public T Copy() => FromXml(AsXml());
    }

    #endregion 

    #region SerializationUtility

    internal static class SerializationUtility
    {
        /// <summary>
        /// Serializes an object instance to a file.
        /// </summary>
        public static bool SerializeObject(object instance, string fileName, bool binarySerialization)
        {
            var response = true;
            if (!binarySerialization)
            {
                XmlTextWriter writer = null;
                try
                {
                    var serializer = new XmlSerializer(instance.GetType());
                    Stream fileStream = new FileStream(fileName, FileMode.Create);
                    writer = new XmlTextWriter(fileStream, new UTF8Encoding()) {Formatting = Formatting.Indented, IndentChar = ' ', Indentation = 3};
                    serializer.Serialize(writer, instance);
                }
                catch(Exception ex)
                {
                    Debug.Write("SerializeObject failed with : " + ex.Message, "West Wind");
                    response = false;
                }
                finally
                {
                    writer?.Close();
                }
            }
            else
            {
                Stream fs = null;
                try
                {
                    var serializer = new XmlSerializer(instance.GetType()); //CF: Check if XmlSerializer can replace BinaryFormatter in this case?
                    fs = new FileStream(fileName, FileMode.Create);
                    serializer.Serialize(fs, instance);
                }
                catch
                {
                    response = false;
                }
                finally
                {
                    fs?.Close();
                }
            }
            return response;
        }

        /// <summary>
        /// Overload that supports passing in an XML TextWriter. 
        /// </summary>
        /// <remarks>
        /// Note the Writer is not closed when serialization is complete so the caller needs to handle closing.
        /// </remarks>
        /// <param name="instance">object to serialize</param>
        /// <param name="writer">XmlTextWriter instance to write output to</param>       
        /// <param name="throwExceptions">Determines whether false is returned on failure or an exception is thrown</param>
        /// <returns></returns>
        public static bool SerializeObject(object instance, XmlTextWriter writer, bool throwExceptions)
        {
            var response = true;
            try
            {
                var serializer = new XmlSerializer(instance.GetType());
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = ' ';
                writer.Indentation = 3;
                serializer.Serialize(writer, instance);
            }
            catch (Exception ex)
            {            
                Debug.Write($"SerializeObject failed with : {ex.GetBaseException().Message}\r\n{(ex.InnerException != null ? ex.InnerException.Message : "")}", "West Wind");

                if (throwExceptions) throw;
                response = false;
            }
            return response;
        }
        
        /// <summary>
        /// Serializes an object into an XML string variable for easy 'manual' serialization
        /// </summary>
        /// <param name="instance">object to serialize</param>
        /// <param name="xmlResultString">resulting XML string passed as an out parameter</param>
        /// <returns>true or false</returns>
        public static bool SerializeObject(object instance, out string xmlResultString) => SerializeObject(instance, out xmlResultString, false);

        /// <summary>
        /// Serializes an object into a string variable for easy 'manual' serialization
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="xmlResultString">Out parameters that holds resulting XML string</param>
        /// <param name="throwExceptions">If true causes exceptions rather than returning false</param>
        /// <returns></returns>
        public static bool SerializeObject(object instance, out string xmlResultString, bool throwExceptions)
        {            
            xmlResultString = string.Empty;
            var memoryStream = new MemoryStream();
            var writer = new XmlTextWriter(memoryStream, new UTF8Encoding());
            if (!SerializeObject(instance, writer,throwExceptions))
            {
                memoryStream.Close();
                return false;
            }
            xmlResultString = Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            memoryStream.Close();
            writer.Close();
            return true;
        }

        /// <summary>
        /// Serializes an object instance to a file.
        /// </summary>
        public static bool SerializeObject(object instance, out byte[] resultBuffer, bool throwExceptions = false)
        {
            var response = true;
            MemoryStream ms = null;
            try
            {
                var serializer = new XmlSerializer(instance.GetType()); //CF: Check if XmlSerializer can replace BinaryFormatter in this case?
                ms = new MemoryStream();
                serializer.Serialize(ms, instance);
            }
            catch(Exception ex)
            {                
                Debug.Write("SerializeObject failed with : " + ex.GetBaseException().Message, "West Wind");
                response = false;

                if (throwExceptions) throw;
            }
            finally
            {
                ms?.Close();
            }
            resultBuffer = ms?.ToArray();
            return response;
        }

        /// <summary>
        /// Serializes an object to an XML string. Unlike the other SerializeObject overloads
        /// this methods *returns a string* rather than a bool result!
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="throwExceptions">Determines if a failure throws or returns null</param>
        /// <returns>
        /// null on error otherwise the Xml String.         
        /// </returns>
        /// <remarks>
        /// If null is passed in null is also returned so you might want
        /// to check for null before calling this method.
        /// </remarks>
        public static string SerializeObjectToString(object instance, bool throwExceptions = false) => !SerializeObject(instance, out string xmlResultString, throwExceptions) ? null : xmlResultString;

        public static byte[] SerializeObjectToByteArray(object instance) => !SerializeObject(instance, out byte[] byteResult) ? null : byteResult;

        /// <summary>
        /// Deserialize an object from file and returns a reference.
        /// </summary>
        /// <param name="fileName">name of the file to serialize to</param>
        /// <param name="objectType">The Type of the object. Use typeof(YourObject class)</param>
        /// <param name="binarySerialization">determines whether we use Xml or Binary serialization</param>
        /// <returns>Instance of the deserialized object or null. Must be cast to your object type</returns>
        public static object DeSerializeObject(string fileName, Type objectType, bool binarySerialization) => DeSerializeObject(fileName, objectType, binarySerialization, false);

        /// <summary>
        /// Deserializes an object from file and returns a reference.
        /// </summary>
        /// <param name="fileName">name of the file to serialize to</param>
        /// <param name="objectType">The Type of the object. Use typeof(YourObject class)</param>
        /// <param name="binarySerialization">determines whether we use Xml or Binary serialization</param>
        /// <param name="throwExceptions">determines whether failure will throw rather than return null on failure</param>
        /// <returns>Instance of the deserialized object or null. Must be cast to your object type</returns>
        public static object DeSerializeObject(string fileName, Type objectType, bool binarySerialization, bool throwExceptions)
        {
            object instance = null;
            if (!binarySerialization)
            {
                XmlReader reader = null;
                FileStream fileStream = null;
                try
                {
                    var serializer = new XmlSerializer(objectType);
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    reader = new XmlTextReader(fileStream);

                    instance = serializer.Deserialize(reader);
                }
                catch(Exception ex)
                {
                    if (throwExceptions) throw;

                    var message = ex.Message;
                    return null;
                }
                finally
                {
                    fileStream?.Close();
                    reader?.Close();
                }
            }
            else
            {
                FileStream fileStream = null;
                try
                {
                    var serializer = new XmlSerializer(instance.GetType()); //CF: Check if XmlSerializer can replace BinaryFormatter in this case?
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    instance = serializer.Deserialize(fileStream);
                }
                catch
                {
                    return null;
                }
                finally
                {
                    fileStream?.Close();
                }
            }
            return instance;
        }

        public static object DeSerializeObject(XmlReader reader, Type objectType)
        {
            var serializer = new XmlSerializer(objectType);
            var instance = serializer.Deserialize(reader);
            reader.Close();
            return instance;
        }

        public static object DeSerializeObject(string xml, Type objectType)
        {
            var reader = new XmlTextReader(xml, XmlNodeType.Document, null);
            return DeSerializeObject(reader, objectType);
        }

        /// <summary>
        /// Deserializes a binary object from a byte array
        /// </summary>
        public static object DeSerializeObject(byte[] buffer, Type objectType, bool throwExceptions = false)
        {
            MemoryStream memoryStream = null;
            try
            {
                var serializer = new XmlSerializer(objectType); //CF: Check if XmlSerializer can replace BinaryFormatter in this case?
                memoryStream = new MemoryStream(buffer);
                return serializer.Deserialize(memoryStream);
            }
            catch
            {
                if (throwExceptions) throw;
                return null;
            }
            finally
            {
                memoryStream?.Close();
            }
        }

        /// <summary>
        /// Returns a string of all the field value pairs of a given object. Works only on non-statics.
        /// </summary>
        public static string ObjectToString(object instance, string separator, ObjectToStringType type)
        {
            var fieldsOnType = instance.GetType().GetFields();
            var output = string.Empty;
            if (type == ObjectToStringType.Properties || type == ObjectToStringType.PropertiesAndFields)
            {
                foreach (var property in instance.GetType().GetProperties())
                {
                    try
                    {
                        output += $"{property.Name}:{property.GetValue(instance, null)}{separator}";
                    }
                    catch
                    {
                        output += $"{property.Name}: n/a{separator}";
                    }
                }
            }

            if (type != ObjectToStringType.Fields && type != ObjectToStringType.PropertiesAndFields) return output;

            foreach (var field in fieldsOnType)
            {
                try
                {
                    output = $"{output}{field.Name}: {field.GetValue(instance)}{separator}";
                }
                catch
                {
                    output = $"{output}{field.Name}: n/a{separator}";
                }
            }
            return output;
        }

        #endregion SerializationUtility

        public enum ObjectToStringType
        {
            Properties,
            PropertiesAndFields,
            Fields
        }
    }
}