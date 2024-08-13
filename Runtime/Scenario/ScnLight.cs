using System.Collections.Generic;
using System.Xml;

using UnityEngine;

public partial class Scenario
{
    [System.Serializable]
    public class ScnLight
    {
        public string Name;
        public string Type;
        public int Priority;

        [HideInInspector]
        public string Position;

        public float Radius;
        public bool Specular;
        public float Intensity;
        public string Color;
        public bool Shadows;
        public float ShadowDarkness;
        public bool Fogged;
        public bool FoggedShadows;
        public bool LightBuffered;
        public bool TerrainOnly;
        public float FarAttnStart;
        public float DecayDist;
        public string Direction;
        public float OuterAngle;
        public float InnerAngle;

        public static List<ScnLight> GetLights(XmlDocument xmlDoc)
        {
            List<ScnLight> lights = new();
            XmlNodeList lightNodes = xmlDoc.SelectNodes("//Light");

            foreach (XmlNode node in lightNodes)
            {
                ScnLight light = new()
                {
                    Name = GetNodeText(node, "Name"),
                    Type = GetNodeText(node, "Type"),
                    Priority = GetNodeInt(node, "Priority"),
                    Position = GetNodeText(node, "Position"),
                    Radius = GetNodeFloat(node, "Radius"),
                    Specular = GetNodeBool(node, "Specular"),
                    Intensity = GetNodeFloat(node, "Intensity"),
                    Color = GetNodeText(node, "Color"),
                    Shadows = GetNodeBool(node, "Shadows"),
                    ShadowDarkness = GetNodeFloat(node, "ShadowDarkness"),
                    Fogged = GetNodeBool(node, "Fogged"),
                    FoggedShadows = GetNodeBool(node, "FoggedShadows"),
                    LightBuffered = GetNodeBool(node, "LightBuffered"),
                    TerrainOnly = GetNodeBool(node, "TerrainOnly"),
                    FarAttnStart = GetNodeFloat(node, "FarAttnStart"),
                    DecayDist = GetNodeFloat(node, "DecayDist"),
                    Direction = GetNodeText(node, "Direction"),
                    OuterAngle = GetNodeFloat(node, "OuterAngle"),
                    InnerAngle = GetNodeFloat(node, "InnerAngle")
                };

                lights.Add(light);
            }

            return lights;
        }

        private static string GetNodeText(XmlNode node, string xpath)
        {
            return node.SelectSingleNode(xpath)?.InnerText ?? string.Empty;
        }

        private static int GetNodeInt(XmlNode node, string xpath)
        {
            return int.TryParse(GetNodeText(node, xpath), out int value) ? value : 0;
        }

        private static float GetNodeFloat(XmlNode node, string xpath)
        {
            return float.TryParse(GetNodeText(node, xpath), out float value) ? value : 0f;
        }

        private static bool GetNodeBool(XmlNode node, string xpath)
        {
            return bool.TryParse(GetNodeText(node, xpath), out bool value) && value;
        }

    }
}