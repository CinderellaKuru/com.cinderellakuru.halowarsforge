using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;

public partial class Scenario
{
    public Data ScnData { get; private set; }
    public Decoration ScnDecoration { get; private set; }
    public Sound ScnSound { get; private set; }
    public LightSet ScnLightSet { get; private set; }
    public FillLightSet ScnFillLightSet { get; private set; }

    private static readonly Dictionary<string, Action<Scenario, string>> ExtensionLoaders = new()
    {
        { ".scn", (scenario, file) => scenario.ScnData = scenario.LoadComponent<Data>(file) },
        { ".sc2", (scenario, file) => scenario.ScnDecoration = scenario.LoadComponent<Decoration>(file) },
        { ".sc3", (scenario, file) => scenario.ScnSound = scenario.LoadComponent<Sound>(file) },
        { ".gls", (scenario, file) => scenario.ScnLightSet = scenario.LoadComponent<LightSet>(file) },
        { ".fls", (scenario, file) => scenario.ScnFillLightSet = scenario.LoadComponent<FillLightSet>(file) }
    };

    public static Scenario Load(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError($"Directory not found: {path}");
            return null;
        }

        Debug.Log($"Loading files from directory: {path}");
        Scenario scenario = new();
        string[] files = Directory.GetFiles(path);

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();
            if (ExtensionLoaders.TryGetValue(extension, out Action<Scenario, string> loader))
            {
                loader(scenario, file);
            }
        }

        return scenario;
    }

    private T LoadComponent<T>(string file) where T : ScenarioComponent, new()
    {
        T component = new();
        component.Load(file);
        LogComponentDetails(component);
        return component;
    }

    private void LogComponentDetails(ScenarioComponent component)
    {
        string componentName = component.GetType().Name;
        Debug.Log($"{componentName} - Number of objects: {component.Objects?.Count ?? 0}");

        switch (component)
        {
            case Data data:
                Debug.Log($"Data - Number of design spheres: {data.DesignSpheres?.Count ?? 0}");
                Debug.Log($"Data - Number of positions: {data.Positions?.Count ?? 0}");
                break;
            case LightSet lightSet:
                Debug.Log($"LightSet - Number of lights: {lightSet.Lights?.Count ?? 0}");
                break;
            case FillLightSet fillLightSet:
                Debug.Log(fillLightSet.SHLightParameters != null ? "FillLightSet - SHLightParams loaded." : "FillLightSet - SHLightParams not found.");
                break;
        }
    }

    public abstract class ScenarioComponent
    {
        public XmlDocument XmlDoc { get; private set; }
        public List<ScnObject> Objects { get; private set; }
        public string OriginalFileName { get; private set; } 

        public virtual void Load(string filePath)
        {
            XmlDoc = new XmlDocument();
            XmlDoc.Load(filePath);
            Objects = ScnObject.GetObjects(XmlDoc);
            OriginalFileName = Path.GetFileName(filePath);
        }
    }


    public class Data : ScenarioComponent
    {
        public string TerrainName { get; private set; }
        public List<ScnDesignSphere> DesignSpheres { get; private set; }
        public List<ScnPosition> Positions { get; private set; }

        public override void Load(string filePath)
        {
            base.Load(filePath);
            TerrainName = XmlDoc.SelectSingleNode("//Terrain").InnerText;
            DesignSpheres = ScnDesignSphere.GetDesignSpheres(XmlDoc);
            Positions = ScnPosition.GetPositions(XmlDoc);
        }
    }

    public class Decoration : ScenarioComponent { }

    public class Sound : ScenarioComponent { }

    public class LightSet : ScenarioComponent
    {
        public List<ScnLight> Lights { get; private set; }

        public override void Load(string filePath)
        {
            base.Load(filePath);
            Lights = ScnLight.GetLights(XmlDoc);
        }
    }

    public class FillLightSet : ScenarioComponent
    {
        public ScnSHLightParams SHLightParameters { get; private set; }

        public override void Load(string filePath)
        {
            base.Load(filePath);
            SHLightParameters = ScnSHLightParams.GetSHLightParams(XmlDoc);
        }
    }
}
