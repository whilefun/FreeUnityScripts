using UnityEngine;

namespace Whilefun.Tools
{

#if UNITY_EDITOR

    //
    // WFGTerrainObject
    // A mostly empty class that allows us to find objects in a scene that should be moved by WFGTerrainObjectPlacementToolEditorWindow
    //
    public class WFGTerrainObject : MonoBehaviour
    {

        [SerializeField, Tooltip("The vertical offset this object will have from the terrain. Default is zero.")]
        private float _verticalOffset = 0.0f;
        public float VerticalOffset { get { return _verticalOffset; } }

    }

#endif

}
