using Learning.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Learning.Scripts.Snapper
{

    public class SnapperTool : EditorWindow
    {
        [MenuItem("NueTools/Learning/Window/Snap Tool")]
        public static void OpenTheThing() => GetWindow<SnapperTool>("Snapper");

        private const string UNDO_STR_SNAP = "Snap Objects";

        public float gridSize = 1f;

        //public Vector3[] points = new Vector3[4];

        private SerializedObject _so;
        private SerializedProperty _propGridSize;
        //private SerializedProperty _propPoints;

        private void OnEnable()
        {
            _so = new SerializedObject(this);
            _propGridSize = _so.FindProperty("gridSize");
            //_propPoints = _so.FindProperty("points");
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
            // // Array
            // _so.Update();
            // points = Handles.PositionHandle(points, Quaternion.identity);
            // for (int i = 0; i < _propPoints.arraySize; i++)
            // {
            //     SerializedProperty prop = _propPoints.GetArrayElementAtIndex(i);
            //     prop.vector3Value = Handles.PositionHandle(prop.vector3Value, Quaternion.identity);
            // }
            //
            // _so.ApplyModifiedProperties();
            
            
            if (Event.current.type == EventType.Repaint)
            {
                Handles.zTest = CompareFunction.LessEqual;
            
                const float gridDrawExtent = 16;

                int lineCount = Mathf.RoundToInt((gridDrawExtent * 2) / gridSize);
                if (lineCount %2 == 0)
                {
                    lineCount++;
                }
                int halfLineCount = lineCount / 2;
            
            
                for (int i = 0; i < lineCount; i++)
                {
                    int intOffset = i-halfLineCount;
                    float xCoord = intOffset * gridSize;
                    float zCoord0 = halfLineCount * gridSize;
                    float zCoord1 = -halfLineCount * gridSize;

                    Vector3 p0 = new Vector3(xCoord, 0f, zCoord0);
                    Vector3 p1 = new Vector3(xCoord, 0f, zCoord1);
                    Handles.DrawAAPolyLine(p0,p1);
                
                    p0 = new Vector3(zCoord0, 0f, xCoord);
                    p1 = new Vector3( zCoord1, 0f,xCoord);
                    Handles.DrawAAPolyLine(p0,p1);
                }
            }
           
        }

        
        private void OnGUI()
        {

            _so.Update();
            EditorGUILayout.PropertyField(_propGridSize);
            _so.ApplyModifiedProperties();
            
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
                go.transform.position = go.transform.position.Round(gridSize); 
            }
        }
    }
}