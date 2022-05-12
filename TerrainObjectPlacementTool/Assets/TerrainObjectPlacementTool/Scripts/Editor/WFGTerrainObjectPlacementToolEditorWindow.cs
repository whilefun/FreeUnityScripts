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

        public Terrain targetTerrain;

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

            GUILayout.Label("Selected Terrain");
            targetTerrain = (Terrain)EditorGUILayout.ObjectField(targetTerrain, typeof(Terrain), true);

            if (GUILayout.Button("Auto Find Terrain"))
            {
                FindTerrain();
            }
           
            if (GUILayout.Button("Refresh Placement on Terrain"))
            {
                RefreshTerrainPlacement();
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

            GUILayout.EndVertical();

        }


        /// <summary>
        /// Lazily finds first Terrain in the scene
        /// </summary>
        private void FindTerrain()
        {

            clearError();

            targetTerrain = GameObject.FindObjectOfType<Terrain>();

            if (targetTerrain == null)
            {
                setError("No Terrain found! Was is disabled in the scene?");
            }

        }

        /// <summary>
        /// Finds all the WFGTerrainOPbject objects and sets them on top of the terrain
        /// </summary>
        private void RefreshTerrainPlacement()
        {

            if (targetTerrain != null)
            {

                clearError();
                WFGTerrainObject[] allTestObjects = GameObject.FindObjectsOfType<WFGTerrainObject>();

                if(allTestObjects.Length == 0)
                {
                    setError("No WFGTerrainObjects were found!");
                }

                for (int i = 0; i < allTestObjects.Length; i++)
                {

                    Vector3 refreshedPosition = allTestObjects[i].transform.position;
                    refreshedPosition.y = targetTerrain.SampleHeight(refreshedPosition);
                    allTestObjects[i].transform.position = refreshedPosition + new Vector3(0.0f, allTestObjects[i].VerticalOffset, 0.0f);

                }

            }
            else
            {
                setError("No Terrain object set!");
            }

        }


        #region TEST_FUNCTIONS

        /// <summary>
        /// A simple test that places a prefab in a grid pattern
        /// </summary>
        private void PlaceTestObjects()
        {

            Vector3 objectPlacementOrigin = new Vector3(-500.0f, 0.0f, -500.0f);
            float objectSpacing = 25.0f;

            GameObject testObjectContainer = GameObject.Find("TestObjectContainer");

            if (testObjectContainer == null)
            {
                testObjectContainer = new GameObject("TestObjectContainer");
            }

            GameObject testObjectPrefab = Resources.Load("Prefabs/MyTestTerrainObject") as GameObject;

            for (int i = 0; i < 25; i++)
            {

                for (int j = 0; j < 25; j++)
                {

                    GameObject testObjectInstance = PrefabUtility.InstantiatePrefab(testObjectPrefab) as GameObject;
                    testObjectInstance.transform.position = objectPlacementOrigin + new Vector3(i * objectSpacing, 0.0f, j * objectSpacing);

                    testObjectInstance.transform.parent = testObjectContainer.transform;

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