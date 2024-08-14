using UnityEngine;

public class UnPosition
{
    private readonly Scenario.ScnPosition CurPosition;
    private GameObject UGX;

    public UnPosition(Scenario.ScnPosition scnPosition)
    {
        CurPosition = scnPosition ?? throw new System.ArgumentNullException(nameof(scnPosition), "ScnPosition is null!");
    }

    public void Render(GameObject parent)
    {
        if (CurPosition == null)
        {
            Debug.LogError("Cannot render as ScnPosition is null!");
            return;
        }

        CreateModel();

        if (UGX != null)
        {
            UGX.transform.SetParent(parent.transform);
            UGX.name = $"Position_{CurPosition.Player}_{CurPosition.Number}";
        }
    }

    private void CreateModel()
    {
        UGX = new GameObject($"Position_{CurPosition.Player}_{CurPosition.Number}");

        if (UGX != null)
        {
            // Set the position and scale based on the ScnPosition data
            UGX.transform.position = ParseVector(CurPosition.Position);
            UGX.transform.forward = ParseVector(CurPosition.Forward);
            UGX.transform.localScale = Vector3.one * 2.0f;  // Adjust the scale as needed

            // Attach a component to store the ScnPosition data
            ScnPositionComponent scnPositionComponent = UGX.AddComponent<ScnPositionComponent>();
            scnPositionComponent.Initialize(CurPosition);
        }
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

public class ScnPositionComponent : MonoBehaviour
{
    [SerializeField]
    private Scenario.ScnPosition CurrentPosition;

    public Scenario.ScnPosition ScnObject
    {
        get => CurrentPosition;
        private set => CurrentPosition = value;
    }

    public void Initialize(Scenario.ScnPosition scnPosition)
    {
        ScnObject = scnPosition;
    }

    // This method is called by Unity to draw gizmos in the scene view.
    private void OnDrawGizmos()
    {
        if (CurrentPosition == null)
            return;

        Gizmos.color = Color.green;

        // Draw a green plane as the gizmo
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(5, 0.01f, 5));  // Adjust the size as needed
    }
}
