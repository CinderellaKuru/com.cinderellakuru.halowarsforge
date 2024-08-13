using System.Collections.Generic;
using System.Xml;

using UnityEngine;

public partial class Scenario
{
    [System.Serializable]
    public class ScnPosition
    {
        public int Player;
        public int Number;

        [HideInInspector]
        public string Position;

        [HideInInspector]
        public string Forward;

        public bool DefaultCamera;
        public float CameraYaw;
        public float CameraPitch;
        public float CameraZoom;
        public long UnitStartObject1;
        public long UnitStartObject2;
        public long UnitStartObject3;
        public long UnitStartObject4;
        public long RallyStartObject;

        public static List<ScnPosition> GetPositions(XmlDocument xmlDoc)
        {
            List<ScnPosition> positions = new();
            XmlNodeList positionNodes = xmlDoc.SelectNodes("//Positions/Position");

            foreach (XmlNode node in positionNodes)
            {
                ScnPosition position = new()
                {
                    Player = GetAttributeInt(node, "Player"),
                    Number = GetAttributeInt(node, "Number"),
                    Position = GetAttributeString(node, "Position"),
                    Forward = GetAttributeString(node, "Forward"),
                    DefaultCamera = GetAttributeBool(node, "DefaultCamera"),
                    CameraYaw = GetAttributeFloat(node, "CameraYaw"),
                    CameraPitch = GetAttributeFloat(node, "CameraPitch"),
                    CameraZoom = GetAttributeFloat(node, "CameraZoom"),
                    UnitStartObject1 = GetAttributeLong(node, "UnitStartObject1"),
                    UnitStartObject2 = GetAttributeLong(node, "UnitStartObject2"),
                    UnitStartObject3 = GetAttributeLong(node, "UnitStartObject3"),
                    UnitStartObject4 = GetAttributeLong(node, "UnitStartObject4"),
                    RallyStartObject = GetAttributeLong(node, "RallyStartObject")
                };

                positions.Add(position);
            }

            return positions;
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

        private static float GetAttributeFloat(XmlNode node, string attributeName)
        {
            return float.TryParse(GetAttributeString(node, attributeName), out float value) ? value : 0f;
        }

        private static bool GetAttributeBool(XmlNode node, string attributeName)
        {
            return bool.TryParse(GetAttributeString(node, attributeName), out bool value) && value;
        }
    }
}
