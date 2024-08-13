using System.Collections.Generic;
using System.Xml;

using UnityEngine;

public partial class Scenario
{
    [System.Serializable]
    public class ScnDesignSphere
    {
        public long ID;
        public string Name;

        [HideInInspector]
        public string Position;

        public float Radius;
        public string Type;
        public int Attract;
        public int Repel;
        public int ChokePoint;
        public int Value;
        public List<string> Values;

        public static List<ScnDesignSphere> GetDesignSpheres(XmlDocument xmlDoc)
        {
            List<ScnDesignSphere> spheres = new();
            XmlNodeList sphereNodes = xmlDoc.SelectNodes("//DesignObjects/Spheres/Sphere");

            foreach (XmlNode node in sphereNodes)
            {
                ScnDesignSphere sphere = new()
                {
                    ID = GetAttributeLong(node, "ID"),
                    Name = GetAttributeString(node, "Name"),
                    Position = GetAttributeString(node, "Position"),
                    Radius = GetAttributeFloat(node, "Radius"),
                    Type = GetNodeText(node, "Data/Type"),
                    Attract = GetNodeInt(node, "Data/Attract"),
                    Repel = GetNodeInt(node, "Data/Repel"),
                    ChokePoint = GetNodeInt(node, "Data/ChokePoint"),
                    Value = GetNodeInt(node, "Data/Value")
                };

                spheres.Add(sphere);
            }

            return spheres;
        }

        private static string GetAttributeString(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName]?.Value ?? string.Empty;
        }

        private static long GetAttributeLong(XmlNode node, string attributeName)
        {
            return long.TryParse(GetAttributeString(node, attributeName), out long value) ? value : 0L;
        }

        private static float GetAttributeFloat(XmlNode node, string attributeName)
        {
            return float.TryParse(GetAttributeString(node, attributeName), out float value) ? value : 0f;
        }

        private static string GetNodeText(XmlNode node, string xpath)
        {
            return node.SelectSingleNode(xpath)?.InnerText ?? string.Empty;
        }

        private static int GetNodeInt(XmlNode node, string xpath)
        {
            return int.TryParse(GetNodeText(node, xpath), out int value) ? value : 0;
        }
    }
}
