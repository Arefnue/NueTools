using Learning.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Learning.Scripts.Snapper
{

    public class SnapperTool : EditorWindow
    {
        [MenuItem("NueTools/Learning/Window/Snap Tool")]
        public static void OpenTheThing() => GetWindow<SnapperTool>("Snapper");

        private const string UNDO_STR_SNAP = "Snap Objects";

        public float gridSize = 1f;

        private SerializedObject so;
        private SerializedProperty propGridSize;

        private void OnEnable()
        {
            so = new SerializedObject(this);
            propGridSize = so.FindProperty("gridSize");
            Selection.selectionChanged += Repaint;
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
            SceneView.duringSceneGui -= DuringSceneGUI;
        }

        private void DuringSceneGUI(SceneView sceneView)
        {
            //Handles.DrawLine(Vector3.zero, Vector3.up);
        }

        
        private void OnGUI()
        {

            so.Update();
            EditorGUILayout.PropertyField(propGridSize);
            so.ApplyModifiedProperties();
            
            using (new EditorGUI.DisabledScope( Selection.gameObjects.Length == 0))
            {
                if (GUILayout.Button("Snap Selection"))
                {
                    SnapSelection();
                }
            }
        }

        private void SnapSelection()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                Undo.RecordObject(go.transform,UNDO_STR_SNAP);
                go.transform.position = go.transform.position.Round();
            }
        }
    }
}