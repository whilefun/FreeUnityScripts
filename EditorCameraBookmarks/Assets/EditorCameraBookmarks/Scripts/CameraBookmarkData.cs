using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookmarksData", menuName = "ScriptableObjects/EditorCameraBookmarks", order = 1)]
public class CameraBookmarkData : ScriptableObject
{

    public Vector3 BookmarkedPosition1 = Vector3.zero;
    public Quaternion BookmarkedRotation1 = Quaternion.identity;


    public Vector3[] bookmarkedPositions;
    public Quaternion[] bookmarkedRotations;

}
