using UnityEngine;

namespace Assets.Scripts.Unity
{
    public class TerrainManager : MonoBehaviour
    {
        private static readonly string[] ValidScenes = {
            "Repository", "Baron_1_swe", "BeaconHill_2", "Beasleys_Plateau", "Blood_Gulch",
            "Chasms", "Crevice", "Exile", "Fort_Deen", "Frozen_Valley", "Glacial_Ravine_3",
            "Labyrinth", "Pirth_Outskirts", "RedRiver_1", "Release", "Terminal_Moraine",
            "The_Docks", "Tundra"
        };

        public static void Load(string sceneName)
        {
            if (IsValidScene(sceneName))
            {
                LoadSceneObject(sceneName);
            }
            else
            {
                Debug.LogError($"Scene name '{sceneName}' is not recognized.");
            }
        }

        private static bool IsValidScene(string sceneName)
        {
            return System.Array.Exists(ValidScenes, validScene => validScene.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase));
        }

        private static void LoadSceneObject(string sceneName)
        {
            GameObject sceneObject = Resources.Load<GameObject>($"{sceneName}/{sceneName}");
            if (sceneObject != null)
            {
                GameObject instantiatedObject = Instantiate(sceneObject);
                instantiatedObject.name = sceneName;
                Debug.Log($"Loaded scene '{sceneName}' successfully.");
            }
            else
            {
                Debug.LogError($"Scene object for '{sceneName}' not found in Resources.");
            }
        }

        public static void Delete(string sceneName)
        {
            GameObject sceneObject = GameObject.Find(sceneName);
            if (sceneObject != null)
            {
                DestroyImmediate(sceneObject);
                Debug.Log($"Deleted scene '{sceneName}' successfully.");
            }
            else
            {
                Debug.LogError($"Scene object '{sceneName}' not found in the scene.");
            }
        }
    }
}
