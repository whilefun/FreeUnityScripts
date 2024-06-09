using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Whilefun.Audio
{


    [CustomEditor(typeof(WFGRandomSoundContainer), true)]
    public class WFGRandomSoundContainerEditor : Editor
    {

        [SerializeField]
        private AudioSource _previewer;

        public void OnEnable()
        {
            _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        public void OnDisable()
        {
            DestroyImmediate(_previewer.gameObject);
        }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

            if (GUILayout.Button("Preview"))
            {
                ((WFGRandomSoundContainer)target).Play(_previewer);
            }

            EditorGUI.EndDisabledGroup();

        }

    }

}