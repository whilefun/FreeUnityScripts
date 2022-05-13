using UnityEngine;
using UnityEditor;

namespace Whilefun.Tools
{

    //
    // WFGTerrainObjectPlacementToolEditorWindow
    // An editor window that allows you to automatically update the position of an object so it sits on top of a Terrain
    //
    public class WFGTerrainObjectPlacementToolEditorWindow : EditorWindow
    {

        private bool haveError = false;
        private string errorString = "";

        private int currentNumberOfTerrainObjects = 0;
        private bool terrainObjectsDirty = false;

        private const string refreshPlacementOnTerrainButtonText = "Refresh Terrain Placements";
        private const string forceFindTerrainButtonText = "Force Objects to Find Terrains";

        // TODO:
        //private bool drawErrorGizmos = true;
        //private bool drawNearestTerrainGizmo = false;

        [MenuItem("While Fun Games/Terrain Object Placement Tool")]
        static void Init()
        {

            WFGTerrainObjectPlacementToolEditorWindow window = (WFGTerrainObjectPlacementToolEditorWindow)EditorWindow.GetWindow(typeof(WFGTerrainObjectPlacementToolEditorWindow));
            window.titleContent = new GUIContent("Terrain Object Placement Tool");
            window.Show();

        }

        void OnGUI()
        {

            GUILayout.BeginVertical();
                       
            if (GUILayout.Button(refreshPlacementOnTerrainButtonText))
            {
                refreshTerrainPlacement();
            }

            if (GUILayout.Button(forceFindTerrainButtonText))
            {
                forceObjectsToFindTerrain();
            }

            // Uncomment these if you want to test things out!

            //if (GUILayout.Button("Place Test Objects"))
            //{
            //    PlaceTestObjects();
            //}

            //if (GUILayout.Button("Delete Test Objects"))
            //{
            //    DeleteTestObjects();
            //}

            if (haveError)
            {
                GUILayout.Label(errorString);
            }

            if (terrainObjectsDirty)
            {
                GUILayout.Label("Stale Terrain Object Data! Click '"+ forceFindTerrainButtonText + "'");
            }
            

            GUILayout.EndVertical();

        }


       

        /// <summary>
        /// Finds all the WFGTerrainOPbject objects and sets them on top of the terrain
        /// </summary>
        private void refreshTerrainPlacement()
        {

            clearError();
            WFGTerrainObject[] allTestObjects = GameObject.FindObjectsOfType<WFGTerrainObject>();

            if(allTestObjects.Length == 0)
            {
                setError("No WFGTerrainObjects were found!");
            }

            for (int i = 0; i < allTestObjects.Length; i++)
            {
                allTestObjects[i].SnapToTerrain();
            }

        }


        private void forceObjectsToFindTerrain()
        {

            clearError();
            WFGTerrainObject[] allTestObjects = GameObject.FindObjectsOfType<WFGTerrainObject>();

            if (allTestObjects.Length == 0)
            {
                setError("No WFGTerrainObjects were found!");
            }

            for (int i = 0; i < allTestObjects.Length; i++)
            {
                allTestObjects[i].ForceLocateTerrain();
            }

            terrainObjectsDirty = false;


        }


        private void OnHierarchyChange()
        {

            WFGTerrainObject[] allTestObjects = GameObject.FindObjectsOfType<WFGTerrainObject>();

            // If a terrain object was added or removed from the scene, and we have terrain objects, give user a hint that they should refresh!
            if(allTestObjects.Length > 0 && allTestObjects.Length != currentNumberOfTerrainObjects)
            {

                terrainObjectsDirty = true;
                currentNumberOfTerrainObjects = allTestObjects.Length;

            }

            Repaint();

        }


        #region TEST_FUNCTIONS

        /// <summary>
        /// A simple test that places a prefab in a grid pattern
        /// </summary>
        private void PlaceTestObjects()
        {

            // Assumes test scene has 9 terrains in a 3x3 grid, origin at (0,0,0)
            // "Bottom left" of the terrain grid is (-150,0,-150)
            int totalTerrainGridWidth = 300;
            Vector3 objectPlacementOrigin = new Vector3(-150.0f, 0.0f, -150.0f);
            int objectGridSize = 50;
            float objectSpacing = (totalTerrainGridWidth / objectGridSize);

            GameObject testObjectContainer = GameObject.Find("TestObjectContainer");

            if (testObjectContainer == null)
            {
                testObjectContainer = new GameObject("TestObjectContainer");
            }

            GameObject testObjectPrefab = Resources.Load("Prefabs/MyTestTerrainObject") as GameObject;

            for (int i = 0; i < objectGridSize; i++)
            {

                for (int j = 0; j < objectGridSize; j++)
                {

                    GameObject testObjectInstance = PrefabUtility.InstantiatePrefab(testObjectPrefab) as GameObject;
                    testObjectInstance.transform.position = objectPlacementOrigin + new Vector3(i * objectSpacing, 0.0f, j * objectSpacing);
                    testObjectInstance.transform.parent = testObjectContainer.transform;
                    // Note: When creating test objects, automatically force locate terrains
                    testObjectInstance.gameObject.GetComponent<WFGTerrainObject>().ForceLocateTerrain();

                }

            }

        }

        /// <summary>
        /// Cleanly removes test objects placed by PlaceTestObjects from the scene
        /// </summary>
        private void DeleteTestObjects()
        {

            clearError();
            WFGTerrainObject[] allTestObjects = GameObject.FindObjectsOfType<WFGTerrainObject>();

            if(allTestObjects.Length == 0)
            {
                setError("No test objects found!");
            }

            for (int i = 0; i < allTestObjects.Length; i++)
            {
                DestroyImmediate(allTestObjects[i].gameObject);
            }

        }

        #endregion


        private void clearError()
        {

            errorString = "";
            haveError = false;

        }


        private void setError(string msg)
        {

            Debug.Log(msg);
            errorString = msg;
            haveError = true;

        }


    }

}