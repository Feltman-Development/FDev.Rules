using System.Linq;
using System.Xml.Linq;

namespace FDEV.Rules.Demo.Core.Serialization
{
    public static class XmlUtility
    {
        public static bool ContainsElement(XDocument xDocument, string elementName)
        {
            var nodes = xDocument.Nodes();
            var elements = xDocument.Elements();
            return elements.Any(x => x.Name == elementName);
        }
    }
}