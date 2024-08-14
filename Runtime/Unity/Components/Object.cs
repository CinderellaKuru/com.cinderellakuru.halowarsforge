using UnityEngine;

using static Scenario;

public class UnObject
{
    private readonly ScnObject CurObject;
    private GameObject UGX;

    public UnObject(ScnObject scnObject)
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
            UGX.name = CurObject.EditorName;
            UGX.isStatic = true;
        }
    }

    private void CreateModel()
    {
        UGX = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (UGX != null)
        {
            Object.DestroyImmediate(UGX.GetComponent<Collider>());

            Vector3 position = ParseVector(CurObject.Position);
            Vector3 forward = ParseVector(CurObject.Forward);
            Vector3 right = ParseVector(CurObject.Right);

            Vector3 up = Vector3.Cross(right, forward);

            if (up == Vector3.zero)
            {
                Debug.LogWarning("Up vector calculated as zero. Falling back to Vector3.up.");
                up = Vector3.up;
            }

            if (Vector3.Dot(Vector3.up, up) < 0)
            {
                forward = -forward;
                up = -up;
            }

            Quaternion rotation = Quaternion.LookRotation(forward, up);

            UGX.transform.SetPositionAndRotation(position, rotation);
            UGX.transform.localScale = new Vector3(1, 10, 1);

            Material unlitMaterial = new(Shader.Find("Unlit/Color"))
            {
                color = Color.white
            };
            UGX.GetComponent<Renderer>().material = unlitMaterial;

            ScnObjectComponent scnObjectComponent = UGX.AddComponent<ScnObjectComponent>();
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

public class ScnObjectComponent : MonoBehaviour
{
    [SerializeField]
    private ScnObject CurrentObject;

    public ScnObject ScnObject
    {
        get => CurrentObject;
        private set => CurrentObject = value;
    }

    public void Initialize(ScnObject scnObject)
    {
        CurrentObject = scnObject;
    }
}
