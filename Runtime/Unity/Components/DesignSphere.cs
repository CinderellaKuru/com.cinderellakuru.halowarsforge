using UnityEngine;

using static Scenario;

public class UnDesignSphere
{
    private readonly ScnDesignSphere CurObject;
    private GameObject UGX;

    public UnDesignSphere(ScnDesignSphere scnObject)
    {
        CurObject = scnObject ?? throw new System.ArgumentNullException(nameof(scnObject), "ScnObject is null!");
    }

    public void Render(GameObject parent)
    {
        if (UGX == null)
        {
            CreateModel();
        }

        if (UGX != null)
        {
            UGX.transform.SetParent(parent.transform);
            UGX.name = CurObject.Name;
            UGX.isStatic = true;
        }
    }

    private void CreateModel()
    {
        // Create an empty GameObject as the placeholder for the Gizmo
        UGX = new GameObject(CurObject.Name);

        if (UGX != null)
        {
            Vector3 position = ParseVector(CurObject.Position);

            UGX.transform.position = position;
            UGX.transform.localScale = new Vector3(1, CurObject.Radius, 1);

            UnDesignSphereComponent scnObjectComponent = UGX.AddComponent<UnDesignSphereComponent>();
            scnObjectComponent.Initialize(CurObject);
        }
    }

    private static Vector3 ParseVector(string vectorString)
    {
        string[] coordinates = vectorString.Split(',');

        if (coordinates.Length == 3 &&
            float.TryParse(coordinates[0], out float x) &&
            float.TryParse(coordinates[1], out float y) &&
            float.TryParse(coordinates[2], out float z))
        {
            return new Vector3(x, y, z);
        }

        Debug.LogError("Failed to parse vector: " + vectorString);
        return Vector3.zero;
    }
}

public class UnDesignSphereComponent : MonoBehaviour
{
    [SerializeField]
    private ScnDesignSphere CurrentObject;

    public ScnDesignSphere ScnObject
    {
        get => CurrentObject;
        private set => CurrentObject = value;
    }

    public void Initialize(ScnDesignSphere scnObject)
    {
        CurrentObject = scnObject;
    }

    // Draw the Gizmo in the Scene view
    private void OnDrawGizmos()
    {
        if (CurrentObject == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CurrentObject.Radius);
    }
}
