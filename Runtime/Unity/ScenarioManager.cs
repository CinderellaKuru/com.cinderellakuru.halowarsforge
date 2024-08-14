using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;

using static Scenario;

namespace Assets.Scripts.Unity
{
    [ExecuteInEditMode]
    public class ScenarioManager : MonoBehaviour
    {
        [HideInInspector]
        public string scenarioPath;

        [HideInInspector]
        public GameObject dataParent;

        [HideInInspector]
        public GameObject decorationParent;

        [HideInInspector]
        public GameObject soundParent;

        [HideInInspector]
        public GameObject lightParent;


        [HideInInspector]
        public bool showData;

        [HideInInspector]
        public bool showDecoration;

        [HideInInspector]
        public bool showSound;

        [HideInInspector]
        public bool showLights;

        [HideInInspector]
        public bool showDesignSpheres;


        private Scenario loadedScenario;

        public void LoadAndRenderScenario()
        {
            loadedScenario = Load(scenarioPath);
            TerrainManager.Load(loadedScenario.ScnData.TerrainName);

            RenderObjects(showData, loadedScenario.ScnData.Objects, dataParent);
            RenderPositions(showData, loadedScenario.ScnData.Positions, dataParent);
            RenderDesign(showDesignSpheres, loadedScenario.ScnData.DesignSpheres, dataParent);
            RenderObjects(showDecoration, loadedScenario.ScnDecoration.Objects, decorationParent);
            RenderObjects(showSound, loadedScenario.ScnSound.Objects, soundParent);
            RenderLights(showLights, loadedScenario.ScnLightSet.Lights, lightParent);
        }


        public void ClearRender()
        {
            TerrainManager.Delete(loadedScenario.ScnData.TerrainName);

            ClearChildren(dataParent);
            ClearChildren(decorationParent);
            ClearChildren(soundParent);
            ClearChildren(lightParent);
        }

        public void SaveScenario()
        {
            if (dataParent == null || loadedScenario == null)
            {
                Debug.LogError("DataParent or LoadedScenario is null. Cannot save scenario.");
                return;
            }

            SaveScenarioComponent(loadedScenario.ScnData, dataParent, loadedScenario.ScnData?.OriginalFileName);
            SaveScenarioComponent(loadedScenario.ScnDecoration, decorationParent, loadedScenario.ScnDecoration?.OriginalFileName);
            SaveScenarioComponent(loadedScenario.ScnSound, soundParent, loadedScenario.ScnSound?.OriginalFileName);
            SaveScenarioComponent(loadedScenario.ScnLightSet, lightParent, loadedScenario.ScnLightSet?.OriginalFileName);

            Debug.Log("Scenario saved successfully.");
        }

        private void SaveScenarioComponent(ScenarioComponent component, GameObject parentObject, string fileName)
        {
            if (component == null || parentObject == null)
            {
                Debug.LogError($"Parent object or scenario component is null. Cannot save {fileName ?? "unknown file"}.");
                return;
            }

            XmlDocument xmlDoc = component.XmlDoc;

            ClearExistingNodes(xmlDoc, "//Objects");
            ClearExistingNodes(xmlDoc, "//DesignObjects/Spheres");
            ClearExistingNodes(xmlDoc, "//Positions");
            ClearExistingNodes(xmlDoc, "//Lights");

            SaveScnObjects(xmlDoc, parentObject);
            SaveDesignSpheres(xmlDoc, parentObject);
            SavePositions(xmlDoc, parentObject);
            SaveLights(xmlDoc, parentObject);

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name is null or empty. Cannot save scenario.");
                return;
            }

            xmlDoc.Save(Path.Combine(scenarioPath, fileName));
        }

        private void SavePositions(XmlDocument xmlDoc, GameObject parentObject)
        {
            XmlNode positionsNode = EnsurePositionsNode(xmlDoc);

            foreach (Transform child in parentObject.transform)
            {
                if (!child.TryGetComponent<ScnPositionComponent>(out ScnPositionComponent scnPositionComponent)) continue;

                XmlElement positionElement = CreatePositionElement(xmlDoc, scnPositionComponent, child);
                _ = positionsNode.AppendChild(positionElement);
            }
        }

        private XmlNode EnsurePositionsNode(XmlDocument xmlDoc)
        {
            XmlNode positionsNode = xmlDoc.SelectSingleNode("//Positions");
            if (positionsNode != null) return positionsNode;

            positionsNode = xmlDoc.CreateElement("Positions");
            _ = (xmlDoc.DocumentElement?.AppendChild(positionsNode));

            return positionsNode;
        }

        private XmlElement CreatePositionElement(XmlDocument xmlDoc, ScnPositionComponent scnPositionComponent, Transform child)
        {
            ScnPosition scnPosition = scnPositionComponent.ScnObject;

            XmlElement positionElement = xmlDoc.CreateElement("Position");
            positionElement.SetAttribute("Player", scnPosition.Player.ToString());
            positionElement.SetAttribute("Number", scnPosition.Number.ToString());
            positionElement.SetAttribute("Position", $"{child.position.x},{child.position.y},{child.position.z}");
            positionElement.SetAttribute("Forward", $"{child.forward.x},{child.forward.y},{child.forward.z}");
            positionElement.SetAttribute("DefaultCamera", scnPosition.DefaultCamera.ToString().ToLower());
            positionElement.SetAttribute("CameraYaw", scnPosition.CameraYaw.ToString());
            positionElement.SetAttribute("CameraPitch", scnPosition.CameraPitch.ToString());
            positionElement.SetAttribute("CameraZoom", scnPosition.CameraZoom.ToString());
            positionElement.SetAttribute("UnitStartObject1", scnPosition.UnitStartObject1.ToString());
            positionElement.SetAttribute("UnitStartObject2", scnPosition.UnitStartObject2.ToString());
            positionElement.SetAttribute("UnitStartObject3", scnPosition.UnitStartObject3.ToString());
            positionElement.SetAttribute("UnitStartObject4", scnPosition.UnitStartObject4.ToString());
            positionElement.SetAttribute("RallyStartObject", scnPosition.RallyStartObject.ToString());

            return positionElement;
        }


        private void ClearExistingNodes(XmlDocument xmlDoc, string xpath)
        {
            xmlDoc.SelectSingleNode(xpath)?.RemoveAll();
        }

        private void SaveScnObjects(XmlDocument xmlDoc, GameObject parentObject)
        {
            XmlNode objectsNode = xmlDoc.SelectSingleNode("//Objects");
            foreach (Transform child in parentObject.transform)
            {
                if (!child.TryGetComponent<ScnObjectComponent>(out ScnObjectComponent scnObjectComponent)) continue;

                XmlElement objectElement = CreateObjectElement(xmlDoc, scnObjectComponent, child);
                _ = (objectsNode?.AppendChild(objectElement));
            }
        }

        private XmlElement CreateObjectElement(XmlDocument xmlDoc, ScnObjectComponent scnObjectComponent, Transform child)
        {
            XmlElement objectElement = xmlDoc.CreateElement("Object");

            objectElement.SetAttribute("IsSquad", scnObjectComponent.ScnObject.IsSquad.ToString().ToLower());
            objectElement.SetAttribute("Player", scnObjectComponent.ScnObject.Player.ToString());
            objectElement.SetAttribute("ID", scnObjectComponent.ScnObject.ID.ToString());
            objectElement.SetAttribute("TintValue", scnObjectComponent.ScnObject.TintValue.ToString());
            objectElement.SetAttribute("EditorName", child.name);

            objectElement.SetAttribute("Position", $"{child.position.x},{child.position.y},{child.position.z}");
            objectElement.SetAttribute("Forward", $"{child.forward.x},{child.forward.y},{child.forward.z}");
            objectElement.SetAttribute("Right", $"{child.right.x},{child.right.y},{child.right.z}");

            objectElement.SetAttribute("Group", scnObjectComponent.ScnObject.Group.ToString());
            objectElement.SetAttribute("VisualVariationIndex", scnObjectComponent.ScnObject.VisualVariationIndex.ToString());

            objectElement.InnerText = scnObjectComponent.ScnObject.InnerText;

            foreach (Flag flag in scnObjectComponent.ScnObject.Flags)
            {
                XmlElement flagElement = xmlDoc.CreateElement("Flag");
                flagElement.InnerText = flag.Name;
                _ = objectElement.AppendChild(flagElement);
            }

            return objectElement;
        }

        private void SaveDesignSpheres(XmlDocument xmlDoc, GameObject parentObject)
        {
            XmlNode spheresNode = EnsureSpheresNode(xmlDoc);

            foreach (Transform child in parentObject.transform)
            {
                if (!child.TryGetComponent<UnDesignSphereComponent>(out UnDesignSphereComponent scnSphereComponent)) continue;

                XmlElement sphereElement = CreateSphereElement(xmlDoc, scnSphereComponent, child);
                _ = spheresNode.AppendChild(sphereElement);
            }
        }

        private XmlNode EnsureSpheresNode(XmlDocument xmlDoc)
        {
            XmlNode spheresNode = xmlDoc.SelectSingleNode("//DesignObjects/Spheres");
            if (spheresNode != null) return spheresNode;

            XmlNode designObjectsNode = xmlDoc.SelectSingleNode("//DesignObjects") ?? xmlDoc.CreateElement("DesignObjects");
            if (designObjectsNode.ParentNode == null)
            {
                _ = (xmlDoc.DocumentElement?.AppendChild(designObjectsNode));
            }

            spheresNode = xmlDoc.CreateElement("Spheres");
            _ = designObjectsNode.AppendChild(spheresNode);

            return spheresNode;
        }

        private XmlElement CreateSphereElement(XmlDocument xmlDoc, UnDesignSphereComponent scnSphereComponent, Transform child)
        {
            ScnDesignSphere scnSphere = scnSphereComponent.ScnObject;

            XmlElement sphereElement = xmlDoc.CreateElement("Sphere");
            sphereElement.SetAttribute("ID", scnSphere.ID.ToString());
            sphereElement.SetAttribute("Name", scnSphere.Name);
            sphereElement.SetAttribute("Position", $"{child.position.x},{child.position.y},{child.position.z}");
            sphereElement.SetAttribute("Radius", scnSphere.Radius.ToString());

            XmlElement dataElement = xmlDoc.CreateElement("Data");
            AddDataElement(xmlDoc, dataElement, "Type", scnSphere.Type);
            AddDataElement(xmlDoc, dataElement, "Attract", scnSphere.Attract.ToString());
            AddDataElement(xmlDoc, dataElement, "Repel", scnSphere.Repel.ToString());
            AddDataElement(xmlDoc, dataElement, "ChokePoint", scnSphere.ChokePoint.ToString());
            AddDataElement(xmlDoc, dataElement, "Value", scnSphere.Value.ToString());

            for (int i = 1; i <= 12; i++)
            {
                _ = dataElement.AppendChild(xmlDoc.CreateElement($"Value{i}"));
            }

            _ = sphereElement.AppendChild(dataElement);
            return sphereElement;
        }

        private void SaveLights(XmlDocument xmlDoc, GameObject parentObject)
        {
            RemoveExistingLightNodes(xmlDoc);

            foreach (Transform child in parentObject.transform)
            {
                if (!child.TryGetComponent<ScnLightComponent>(out ScnLightComponent scnLightComponent)) continue;

                XmlElement lightElement = CreateLightElement(xmlDoc, scnLightComponent, child);
                _ = (xmlDoc.DocumentElement?.AppendChild(lightElement));
            }
        }

        private void RemoveExistingLightNodes(XmlDocument xmlDoc)
        {
            XmlNodeList lightNodes = xmlDoc.SelectNodes("//Light");
            foreach (XmlNode lightNode in lightNodes)
            {
                _ = (lightNode.ParentNode?.RemoveChild(lightNode));
            }
        }

        private XmlElement CreateLightElement(XmlDocument xmlDoc, ScnLightComponent scnLightComponent, Transform child)
        {
            ScnLight scnLight = scnLightComponent.ScnObject;

            XmlElement lightElement = xmlDoc.CreateElement("Light");

            AddDataElement(xmlDoc, lightElement, "Name", scnLight.Name);
            AddDataElement(xmlDoc, lightElement, "Type", scnLight.Type);
            AddDataElement(xmlDoc, lightElement, "Priority", scnLight.Priority.ToString());
            AddDataElement(xmlDoc, lightElement, "Position", $"{child.position.x},{child.position.y},{child.position.z}");
            AddDataElement(xmlDoc, lightElement, "Radius", scnLight.Radius.ToString());
            AddDataElement(xmlDoc, lightElement, "Specular", scnLight.Specular.ToString().ToLower());
            AddDataElement(xmlDoc, lightElement, "Intensity", scnLight.Intensity.ToString());
            AddDataElement(xmlDoc, lightElement, "Color", scnLight.Color);
            AddDataElement(xmlDoc, lightElement, "Shadows", scnLight.Shadows.ToString().ToLower());
            AddDataElement(xmlDoc, lightElement, "ShadowDarkness", scnLight.ShadowDarkness.ToString());
            AddDataElement(xmlDoc, lightElement, "Fogged", scnLight.Fogged.ToString().ToLower());
            AddDataElement(xmlDoc, lightElement, "FoggedShadows", scnLight.FoggedShadows.ToString().ToLower());
            AddDataElement(xmlDoc, lightElement, "LightBuffered", scnLight.LightBuffered.ToString().ToLower());
            AddDataElement(xmlDoc, lightElement, "TerrainOnly", scnLight.TerrainOnly.ToString().ToLower());
            AddDataElement(xmlDoc, lightElement, "FarAttnStart", scnLight.FarAttnStart.ToString());
            AddDataElement(xmlDoc, lightElement, "DecayDist", scnLight.DecayDist.ToString());
            AddDataElement(xmlDoc, lightElement, "Direction", scnLight.Direction);
            AddDataElement(xmlDoc, lightElement, "OuterAngle", scnLight.OuterAngle.ToString());
            AddDataElement(xmlDoc, lightElement, "InnerAngle", scnLight.InnerAngle.ToString());

            return lightElement;
        }

        private void AddDataElement(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string innerText)
        {
            XmlElement element = xmlDoc.CreateElement(elementName);
            element.InnerText = innerText;
            _ = parentElement.AppendChild(element);
        }



        private void ClearChildren(GameObject parent)
        {
            if (parent != null)
            {
                for (int i = parent.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(parent.transform.GetChild(i).gameObject);
                }
            }
        }

        private void RenderObjects(bool shouldRender, IEnumerable<ScnObject> objects, GameObject parent)
        {
            if (shouldRender && objects != null)
            {
                foreach (ScnObject obj in objects)
                {
                    UnObject unObject = new(obj);
                    unObject.Render(parent);
                }
            }
        }

        private void RenderLights(bool shouldRender, IEnumerable<ScnLight> lights, GameObject parent)
        {
            if (shouldRender && lights != null)
            {
                foreach (ScnLight light in lights)
                {
                    UnLight unLight = new(light);
                    unLight.Render(parent);
                }
            }
        }

        private void RenderDesign(bool shouldRender, IEnumerable<ScnDesignSphere> spheres, GameObject parent)
        {
            if (shouldRender && spheres != null)
            {
                foreach (ScnDesignSphere sphere in spheres)
                {
                    UnDesignSphere unSphere = new(sphere);
                    unSphere.Render(parent);
                }
            }
        }

        private void RenderPositions(bool shouldRender, IEnumerable<ScnPosition> positions, GameObject parent)
        {
            if (shouldRender && positions != null)
            {
                foreach (ScnPosition position in positions)
                {
                    UnPosition unPosition = new(position);
                    unPosition.Render(parent);
                }
            }
        }
    }
}
