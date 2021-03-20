using Learning.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Learning.Scripts.Snapper
{
    public static class SnapperEditor
    {
        private const string UNDO_STR_SNAP = "Snap Objects";
        
        [MenuItem("NueTools/Learning/Editor/Snap Selected Objects %&S", isValidateFunction: true)]
        public static bool SnapTheThingsValidate()
        {
            return Selection.gameObjects.Length > 0;
        }
        
        [MenuItem("NueTools/Learning/Editor/Snap Selected Objects %&S")]
        public static void SnapTheThings()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                Undo.RecordObject(go.transform,UNDO_STR_SNAP);
                go.transform.position = go.transform.position.Round();
            }
        }

       
    }
}