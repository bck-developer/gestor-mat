using GestorMat.Application.DTOs;
using System.Xml.Linq;

namespace GestorMat.Application.Servicios;

public class XmlService
{
    public List<MaterialXmlDto> LeerMaterialesDesdeXml(Stream stream)
    {
        List<MaterialXmlDto> materiales = new List<MaterialXmlDto>();

        XDocument doc = XDocument.Load(stream);

        IEnumerable<XElement> items = doc.Descendants("Material");

        foreach (XElement item in items)
        {
            MaterialXmlDto material = new MaterialXmlDto
            {
                Nombre = item.Element("Nombre")?.Value ?? string.Empty,
                Precio = decimal.Parse(item.Element("Precio")?.Value ?? "0"),
                Stock = int.Parse(item.Element("Stock")?.Value ?? "0"),
                IdUnidadMedida = int.Parse(item.Element("IdUnidadMedida")?.Value ?? "0")
            };

            materiales.Add(material);
        }

        return materiales;
    }
}
