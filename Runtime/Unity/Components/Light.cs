using UnityEngine;

using static Scenario;

public class UnLight
{
    private readonly ScnLight CurLight;
    private GameObject UGX;

    public UnLight(ScnLight scnObject)
    {
        if (scnObject == null)
        {
            Debug.LogError("ScnObject is null!");
            return;
        }

        CurLight = scnObject;
    }

    public void Render(GameObject parent)
    {
        if (CurLight == null)
        {
            Debug.LogError("Cannot render as ScnObject is null!");
            return;
        }

        CreateModel();

        if (UGX != null)
        {
            UGX.transform.SetParent(parent.transform);
            UGX.name = CurLight.Name;
        }
    }

    private void CreateModel()
    {
        UGX = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        if (UGX != null)
        {
            Object.DestroyImmediate(UGX.GetComponent<Collider>());

            UGX.transform.position = ParseVector(CurLight.Position);
            UGX.transform.localScale = new Vector3(CurLight.Radius / 3.5f, CurLight.Radius / 3.5f, CurLight.Radius / 3.5f);

            Material glowMaterial = new Material(Shader.Find("Standard"));

            glowMaterial.color = ParseColor(CurLight.Color);

            glowMaterial.EnableKeyword("_EMISSION");
            glowMaterial.SetColor("_EmissionColor", ParseColor(CurLight.Color) * 4.0f);

            UGX.GetComponent<Renderer>().material = glowMaterial;

            ScnLightComponent scnObjectComponent = UGX.AddComponent<ScnLightComponent>();
            scnObjectComponent.Initialize(CurLight);

            Light light = UGX.AddComponent<Light>();
            light.type = LightType.Point;

            light.intensity = CurLight.Intensity;
            light.color = ParseColor(CurLight.Color);
            light.range = CurLight.Radius;
            light.shadows = LightShadows.Hard;
            light.shadowStrength = CurLight.ShadowDarkness;
        }
    }


    private Color ParseColor(string colorString)
    {
        if (colorString.Contains(","))
        {
            string[] rgb = colorString.Split(',');

            if (rgb.Length == 3)
            {
                if (int.TryParse(rgb[0], out int r) &&
                    int.TryParse(rgb[1], out int g) &&
                    int.TryParse(rgb[2], out int b))
                {
                    return new Color(r / 255f, g / 255f, b / 255f);
                }
            }
        }

        Debug.LogError("Failed to parse color: " + colorString);
        return Color.white;
    }

    private Vector3 ParseVector(string vectorString)
    {
        string[] coordinates = vectorString.Split(',');

        if (coordinates.Length == 3)
        {
            if (float.TryParse(coordinates[0], out float x) &&
                float.TryParse(coordinates[1], out float y) &&
                float.TryParse(coordinates[2], out float z))
            {
                return new Vector3(x, y, z);
            }
        }

        Debug.LogError("Failed to parse vector: " + vectorString);
        return Vector3.zero;
    }
}

public class ScnLightComponent : MonoBehaviour
{
    [SerializeField]
    private ScnLight CurrentLight;

    public ScnLight ScnObject
    {
        get => CurrentLight;
        set => CurrentLight = value;
    }

    public void Initialize(ScnLight scnObject)
    {
        ScnObject = scnObject;
    }
}

