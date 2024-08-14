using System.Xml;

public partial class Scenario
{
    public class ScnSHLightParams
    {
        public string[] R { get; set; }
        public string[] G { get; set; }
        public string[] B { get; set; }

        public static ScnSHLightParams GetSHLightParams(XmlDocument XmlDoc)
        {
            XmlNode shLightParamsNode = XmlDoc.SelectSingleNode("//SHLightParams");
            return shLightParamsNode != null
                ? new ScnSHLightParams
                {
                    R = shLightParamsNode.SelectSingleNode("R").InnerText.Split(','),
                    G = shLightParamsNode.SelectSingleNode("G").InnerText.Split(','),
                    B = shLightParamsNode.SelectSingleNode("B").InnerText.Split(',')
                }
                : null;
        }
    }

}
