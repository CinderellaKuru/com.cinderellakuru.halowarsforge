using System.Collections.Generic;
using System.Xml;

using UnityEngine;

public partial class Scenario
{
    [System.Serializable]
    public class ScnObject
    {
        public bool IsSquad;
        public int Player;
        public long ID;
        public int TintValue;

        [HideInInspector]
        public string EditorName;

        public string Position;

        public string Forward;

        public string Right;

        public int Group;
        public int VisualVariationIndex;
        public string InnerText;
        public List<Flag> Flags;

        public ScnObject()
        {
            Flags = new List<Flag>();
        }

        public static List<ScnObject> GetObjects(XmlDocument xmlDoc)
        {
            List<ScnObject> scnObjects = new();
            XmlNodeList objectNodes = xmlDoc.SelectNodes("//Objects/Object");

            foreach (XmlNode node in objectNodes)
            {
                ScnObject scnObject = new()
                {
                    IsSquad = GetAttributeBool(node, "IsSquad"),
                    Player = GetAttributeInt(node, "Player"),
                    ID = GetAttributeLong(node, "ID"),
                    TintValue = GetAttributeInt(node, "TintValue"),
                    EditorName = GetAttributeString(node, "EditorName"),
                    Position = GetAttributeString(node, "Position"),
                    Forward = GetAttributeString(node, "Forward"),
                    Right = GetAttributeString(node, "Right"),
                    Group = GetAttributeInt(node, "Group"),
                    VisualVariationIndex = GetAttributeInt(node, "VisualVariationIndex"),
                    InnerText = GetInnerText(node)
                };

                XmlNodeList flagNodes = node.SelectNodes("Flag");
                foreach (XmlNode flagNode in flagNodes)
                {
                    Flag flag = new() { Name = flagNode.InnerText.Trim() };
                    scnObject.Flags.Add(flag);
                }

                scnObjects.Add(scnObject);
            }

            return scnObjects;
        }

        private static string GetAttributeString(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName]?.Value ?? string.Empty;
        }

        private static int GetAttributeInt(XmlNode node, string attributeName)
        {
            return int.TryParse(GetAttributeString(node, attributeName), out int value) ? value : 0;
        }

        private static long GetAttributeLong(XmlNode node, string attributeName)
        {
            return long.TryParse(GetAttributeString(node, attributeName), out long value) ? value : 0L;
        }

        private static bool GetAttributeBool(XmlNode node, string attributeName)
        {
            return bool.TryParse(GetAttributeString(node, attributeName), out bool value) && value;
        }

        private static string GetInnerText(XmlNode node)
        {
            string innerText = string.Empty;
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Text)
                {
                    innerText += child.Value.Trim();
                }
            }
            return innerText;
        }
    }

    [System.Serializable]
    public class Flag
    {
        public string Name;
    }
}
