using UnityEditor;
using UnityEngine;

//
// CameraBookmarks
//
// A simple "Save my scene view camera to a bookmark, please" function for Unity.
// Inspired by this great UE4 feature noted in this tweet: https://twitter.com/HighlySpammable/status/1234998578323415041
//
[InitializeOnLoad]
public static class CameraBookmarks
{

    static CameraBookmarks()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {

        // We apparently need a control ID to avoid messing with other tools
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        if (Event.current.GetTypeForControl(controlID) == EventType.KeyDown)
        {

            if (Event.current.keyCode == KeyCode.Alpha1)
            {

                if(Event.current.modifiers == EventModifiers.Control)
                {
                    SaveCameraLocation(1);
                }
                else
                {
                    GoToCameraLocation(1);
                }

                Event.current.Use();

            }
            // Note: Alpha 2 will "break" toggling 2D mode. But you probably don't want to use this if you're making a 2D project anyway?
            else if (Event.current.keyCode == KeyCode.Alpha2)
            {

                if (Event.current.modifiers == EventModifiers.Control)
                {
                    SaveCameraLocation(2);
                }
                else
                {
                    GoToCameraLocation(2);
                }

                Event.current.Use();

            }
            // The rest are really ugly for the sake of compactness :P
            else if (Event.current.keyCode == KeyCode.Alpha3)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(3); }
                else { GoToCameraLocation(3); }
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Alpha4)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(4); }
                else { GoToCameraLocation(4); }
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Alpha5)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(5); }
                else { GoToCameraLocation(5); }
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Alpha6)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(6); }
                else { GoToCameraLocation(6); }
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Alpha7)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(7); }
                else { GoToCameraLocation(7); }
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Alpha8)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(8); }
                else { GoToCameraLocation(8); }
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Alpha9)
            {
                if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(9); }
                else { GoToCameraLocation(9); }
                Event.current.Use();
            }
            // We don't use Alpha0 because Unity does other weird things with that, apparently.
            //else if (Event.current.keyCode == KeyCode.Alpha0)
            //{
            //    if (Event.current.modifiers == EventModifiers.Control) { SaveCameraLocation(10); }
            //    else { GoToCameraLocation(10); }
            //    Event.current.Use();
            //}


        }

    }


    private static void SaveCameraLocation(int index)
    {

        CameraBookmarkData myData = Resources.Load("BookmarkData/CameraBookmarks") as CameraBookmarkData;
        myData.bookmarkedPositions[index - 1] = SceneView.lastActiveSceneView.pivot;
        myData.bookmarkedRotations[index - 1] = SceneView.lastActiveSceneView.rotation;

        EditorUtility.SetDirty(myData);

    }

    private static void GoToCameraLocation(int index)
    {

        CameraBookmarkData myData = Resources.Load("BookmarkData/CameraBookmarks") as CameraBookmarkData;
        SceneView.lastActiveSceneView.pivot = myData.bookmarkedPositions[index - 1];
        SceneView.lastActiveSceneView.rotation = myData.bookmarkedRotations[index - 1];
        SceneView.lastActiveSceneView.Repaint();

    }

}