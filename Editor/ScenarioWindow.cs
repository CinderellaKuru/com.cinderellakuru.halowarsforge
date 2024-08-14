using UnityEditor;

using UnityEngine;

namespace Assets.Scripts.Unity
{
    public class ScenarioWindow : EditorWindow
    {
        private SerializedObject serializedObject;
        private SerializedProperty scenarioPath;
        private SerializedProperty dataParent;
        private SerializedProperty decorationParent;
        private SerializedProperty soundParent;
        private SerializedProperty lightParent;
        private SerializedProperty showData;
        private SerializedProperty showDecoration;
        private SerializedProperty showSound;
        private SerializedProperty showLights;
        private SerializedProperty showDesignSpheres;

        private ScenarioManager scenarioManager;
        private Vector2 scrollPosition;

        [MenuItem("Halo Wars/Scenario Editor")]
        public static void ShowWindow()
        {
            ScenarioWindow window = GetWindow<ScenarioWindow>("Scenario Editor");
            window.Show();
        }

        private void OnEnable()
        {
            FindScenarioManager();
            if (scenarioManager != null)
            {
                InitializeSerializedProperties();
            }
        }

        private void FindScenarioManager()
        {
            scenarioManager = FindObjectOfType<ScenarioManager>();
        }

        private void InitializeSerializedProperties()
        {
            serializedObject = new SerializedObject(scenarioManager);
            scenarioPath = serializedObject.FindProperty("scenarioPath");
            dataParent = serializedObject.FindProperty("dataParent");
            decorationParent = serializedObject.FindProperty("decorationParent");
            soundParent = serializedObject.FindProperty("soundParent");
            lightParent = serializedObject.FindProperty("lightParent");
            showData = serializedObject.FindProperty("showData");
            showDecoration = serializedObject.FindProperty("showDecoration");
            showSound = serializedObject.FindProperty("showSound");
            showLights = serializedObject.FindProperty("showLights");
            showDesignSpheres = serializedObject.FindProperty("showDesignSpheres");
        }

        private void OnGUI()
        {
            scenarioManager = (ScenarioManager)EditorGUILayout.ObjectField("ScenarioManager", scenarioManager, typeof(ScenarioManager), true);

            if (scenarioManager == null)
            {
                DisplayManagerNotFoundMessage();
                return;
            }

            serializedObject.Update();

            // Begin scroll view
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawUI();

            // End scroll view
            EditorGUILayout.EndScrollView();

            _ = serializedObject.ApplyModifiedProperties();
        }

        private void DisplayManagerNotFoundMessage()
        {
            EditorGUILayout.HelpBox("ScenarioManager script not found in the scene!", MessageType.Warning);
            if (GUILayout.Button("Retry"))
            {
                OnEnable();
            }
        }

        private void DrawUI()
        {
            DrawScenarioSection();
            DrawContainersSection();
            DrawRenderingOptionsSection();

            EditorGUILayout.Space();
            DrawSaveButton();
        }

        private void DrawScenarioSection()
        {
            EditorGUILayout.LabelField("Scenario", EditorStyles.boldLabel);
            _ = EditorGUILayout.PropertyField(scenarioPath, new GUIContent("Scenario Path", "Path to the scenario file."));
            EditorGUILayout.Space();

            DrawScenarioButtons();

            EditorGUILayout.Space();
        }

        private void DrawScenarioButtons()
        {
            if (GUILayout.Button("Load and Render Scenario"))
            {
                scenarioManager.LoadAndRenderScenario();
            }

            if (GUILayout.Button("Clear Rendered Scenario"))
            {
                scenarioManager.ClearRender();
            }
        }

        private void DrawContainersSection()
        {
            EditorGUILayout.LabelField("Containers", EditorStyles.boldLabel);
            _ = EditorGUILayout.PropertyField(dataParent, new GUIContent("Data Parent", "Parent object for data objects."));
            _ = EditorGUILayout.PropertyField(decorationParent, new GUIContent("Decoration Parent", "Parent object for decoration objects."));
            _ = EditorGUILayout.PropertyField(soundParent, new GUIContent("Sound Parent", "Parent object for sound objects."));
            _ = EditorGUILayout.PropertyField(lightParent, new GUIContent("Light Parent", "Parent object for light objects."));
            EditorGUILayout.Space();
        }

        private void DrawRenderingOptionsSection()
        {
            EditorGUILayout.LabelField("Rendering Options", EditorStyles.boldLabel);
            _ = EditorGUILayout.PropertyField(showData, new GUIContent("Render Data (.scn)", "Toggle rendering of data objects."));
            _ = EditorGUILayout.PropertyField(showDesignSpheres, new GUIContent("Render Design (.scn)", "Toggle rendering of design objects."));
            _ = EditorGUILayout.PropertyField(showDecoration, new GUIContent("Render Decoration (.sc2)", "Toggle rendering of decoration objects."));
            _ = EditorGUILayout.PropertyField(showSound, new GUIContent("Render Sound (.sc3)", "Toggle rendering of sound objects."));
            _ = EditorGUILayout.PropertyField(showLights, new GUIContent("Render Lights (.gls)", "Toggle rendering of light objects."));
            EditorGUILayout.Space();
        }

        private void DrawSaveButton()
        {
            if (GUILayout.Button("Save Scenario (experimental)"))
            {
                scenarioManager.SaveScenario();
            }
        }
    }
}
