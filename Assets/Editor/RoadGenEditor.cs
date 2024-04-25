using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateRoundCurve))]
public class RingsAndRoadsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateRoundCurve script = (GenerateRoundCurve)target;

        EditorGUILayout.Space();

        if (script.ringsAndRoads != null)
        {
            EditorGUILayout.LabelField("Rings And Roads:");

            for (int i = 0; i < script.ringsAndRoads.Count; i++)
            {
                EditorGUILayout.LabelField("Ring " + i + ":");

                for (int j = 0; j < script.ringsAndRoads[i].Count; j++)
                {
                    EditorGUILayout.ObjectField("Element " + j, script.ringsAndRoads[i][j], typeof(GameObject), false);
                }
            }
        }
    }
}
