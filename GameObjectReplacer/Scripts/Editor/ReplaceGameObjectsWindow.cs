using UnityEngine;
using UnityEditor;

//
// ReplaceGameObjectsWindow
// A simple Unity Editor utility that allows you to select objects in your scene to be replaced by other objects.
// Ideal for cleaning up a scene with duplicated non-prefab objects or old prefabs and replacing them with a nice clean prefab.
//
// Made with Unity 5.5.4p3. Probably works in older versions, but was not tested with them.
//
// Usage:
// Select one or more Objects in your scene you want to replace
// Select Tools -> Replace GameObjects
// Focusing on the Window will update the selected array of Objects with your scene hierarchy selection.
// Choose a replacement object (e.g. a prefab from the Project Explorer)
// Press the "Replace..." button to replace the objects
// If you need to undo, press Ctrl+Z or go to Edit->Undo.
//
public class ReplaceGameObjectsWindow : EditorWindow
{
    
    public Transform[] ObjectsToReplace;
    public GameObject ReplacementObject;

    private bool groupEnabled;

    private bool copyPosition = true;
    private bool copyRotation = true;
    private bool copyScale = true;
    private string errorString = "";
    private Vector2 objectScrollPosition = Vector2.zero;

    [MenuItem("Tools/Replace GameObjects")]
    public static void ShowReplaceGameObjectsWindow()
    {

        // Use this to make dockable style window instead of utility style window
        //EditorWindow myWindow = GetWindow(typeof(ReplaceGameObjectsWindow));
        //myWindow.ShowUtility();

        ReplaceGameObjectsWindow window = ScriptableObject.CreateInstance(typeof(ReplaceGameObjectsWindow)) as ReplaceGameObjectsWindow;
        window.minSize = new Vector2(640f, 320f);
        window.ShowUtility();

    }

    private void OnGUI()
    {

        Transform[] preSelectedObjects = Selection.transforms;

        if (preSelectedObjects != null && preSelectedObjects.Length > 0)
        {
            GUILayout.Label(preSelectedObjects.Length + " Objects selected for replacement.");
            ObjectsToReplace = preSelectedObjects;
        }
        else
        {
            ObjectsToReplace = null;
            GUILayout.Label("No Objects selected for replacement.");
        }

        // Options
        GUILayout.Label("Copy From Original(s):", EditorStyles.boldLabel);
        copyPosition = EditorGUILayout.Toggle("Position", copyPosition);
        copyRotation = EditorGUILayout.Toggle("Rotation", copyRotation);
        copyScale = EditorGUILayout.Toggle("Scale", copyScale);

        // Divider hack
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

        // Create a serialized reference to our Editor Window (which is just a SerializedObject child class, apparently)
        ScriptableObject windowObject = this;
        SerializedObject serializedWindowObject = new SerializedObject(windowObject);

        // Selected Objects to replace
        GUILayout.Label("Objects to replace:", EditorStyles.boldLabel);
        objectScrollPosition = GUILayout.BeginScrollView(objectScrollPosition);
        SerializedProperty myObjectProperties = serializedWindowObject.FindProperty("ObjectsToReplace");
        EditorGUILayout.PropertyField(myObjectProperties, true);
        serializedWindowObject.ApplyModifiedProperties();
        GUILayout.EndScrollView();

        // Replacement Object
        GUILayout.Label("Replacement Object (e.g. a nice clean prefab):", EditorStyles.boldLabel);
        SerializedProperty myReplacementObjectProperties = serializedWindowObject.FindProperty("ReplacementObject");
        EditorGUILayout.PropertyField(myReplacementObjectProperties, true);
        serializedWindowObject.ApplyModifiedProperties();

        // The 'Go' button
        if ((ObjectsToReplace != null && ObjectsToReplace.Length > 0) && (ReplacementObject != null))
        {

            errorString = "";

            if (GUILayout.Button("Replace objects with '"+ReplacementObject.name+"'"))
            {
                ReplaceTheObjects();
            }

        }
        else
        {

            if(ObjectsToReplace == null)
            {
                errorString = "You must assign at least one object in the 'Objects To Replace' list!";
            }
            if (ReplacementObject == null)
            {
                errorString = "You must assign an object (e.g. prefab) to the Replacement Object field!";
            }

            GUI.enabled = false;
            if (GUILayout.Button("Cannot Replace objects (see Error below)"))
            {
            }

        }

        GUI.enabled = true;

        if (errorString != "")
        {
            GUILayout.Label("  " + "Error:" + errorString);
        }

    }

    /// <summary>
    /// Actually replaces the selected objects with the selected object/prefab
    /// </summary>
    private void ReplaceTheObjects()
    {

        for (int i = 0; i < ObjectsToReplace.Length; i++)
        {

            GameObject newObject;
            newObject = (GameObject)PrefabUtility.InstantiatePrefab(ReplacementObject);

            if (copyPosition)
            {
                newObject.transform.position = ObjectsToReplace[i].transform.position;
            }

            if (copyRotation)
            {
                newObject.transform.rotation = ObjectsToReplace[i].transform.rotation;
            }

            if (copyScale)
            {
                newObject.transform.localScale = ObjectsToReplace[i].transform.localScale;
            }

            newObject.transform.parent = ObjectsToReplace[i].transform.parent;

            // This ensures that the user can undo
            Undo.RegisterCreatedObjectUndo(newObject, "Replaced " + ObjectsToReplace.Length + " objects with '"+ ReplacementObject.name + "'");
            Undo.DestroyObjectImmediate(ObjectsToReplace[i].gameObject);

        }

    }

}