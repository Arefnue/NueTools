using System;
using UnityEditor;
using UnityEngine;

namespace Learning.Scripts
{

    public class SnapperTool : EditorWindow
    {
        [MenuItem("NueTools/Learning/Window/Snap Tool")]
        public static void OpenTheThing() => GetWindow<SnapperTool>("Snapper");

        private const string UNDO_STR_SNAP = "Snap Objects";

        private void OnEnable()
        {
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
    
    
    public static class Snapper
    {
        private const string UNDO_STR_SNAP = "Snap Objects";
        
        [MenuItem("Edit/Learning", isValidateFunction: true)]
        public static bool SnapTheThingsValidate()
        {
            return Selection.gameObjects.Length > 0;
        }
        
        [MenuItem("Edit/Learning", isValidateFunction: true)]
        public static void SnapTheThings()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                Undo.RecordObject(go.transform,UNDO_STR_SNAP);
                go.transform.position = go.transform.position.Round();
            }
        }

        public static Vector3 Round(this Vector3 v)
        {
            v.x = Mathf.Round(v.x);
            v.y = Mathf.Round(v.y);
            v.z = Mathf.Round(v.z);
            return v;
        }
    }
}